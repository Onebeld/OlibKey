using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using OlibKey.Core.Helpers.Safety;
using OlibKey.Core.Models.StorageUnits;
using OlibKey.Core.Settings;
using OlibKey.SystemHardwareInfoProvider;

namespace OlibKey.Core.Helpers;

/// <summary>
/// The class is responsible for saving and loading the storage file.
/// </summary>
/// 
/// <remarks>
/// Version: 1.0
/// </remarks>
public static class OlibFileLoader
{
	private const uint MagicNumber = 0xE36296C5;
	
	private const byte MajorVersion = 1;
	private const byte MinorVersion = 0;

	private const int BufferSize = 1024 * 4;
	
	/// <summary>
	/// Saves the storage to a file using the specified path and master password. The file is encoded in ASCII.
	/// </summary>
	/// 
	/// <param name="storage">The <see cref="Storage"/> to save.</param>
	/// <param name="path">The path to save the file to.</param>
	/// <param name="masterPassword">The master password used for encryption.</param>
	/// 
	/// <exception cref="IOException">Thrown if an I/O error occurs while writing to the file.</exception>
	/// <exception cref="InvalidDataException">Thrown if the file format is invalid.</exception>
	/// 
	/// <remarks>
	/// Data size must not exceed 2 GB
	/// </remarks>
	public static void Save(Storage storage, string path, string masterPassword)
	{
		if (storage.Settings.UseHardwareBinding)
			masterPassword = CreateHashWithSecretKey(GetDeviceIdentifiers(), masterPassword);
		
		byte[] encryptJson;

		{
			string storageJson = ToJson(storage);
			byte[] compressedJson = Compressor.Compress(Encoding.UTF8.GetBytes(storageJson));
			encryptJson = Encryptor.Encrypt(compressedJson, masterPassword, storage.Settings.Iterations);
		}

		using BinaryWriter writer = new(new FileStream(path, FileMode.Create, FileAccess.Write), Encoding.ASCII);
		
		WriteVersion(writer);
		WriteName(writer, storage.Settings.Name);
		
		writer.Write(storage.Settings.Iterations);
		
		WriteData(writer, encryptJson);
		
		writer.Write(storage.Settings.UseHardwareBinding);
		writer.Write(storage.Settings.UseTrashcan);
		
		WriteData(writer, storage.Settings.ImageData, true);
	}
	
	/// <summary>
	/// Loads a storage from a file using the specified path and master password. The file is decoded from ASCII
	/// </summary>
	/// 
	/// <param name="path">The path to the file.</param>
	/// <param name="masterPassword">The master password used for decryption.</param>
	/// 
	/// <returns>The loaded storage object.</returns>
	/// 
	/// <exception cref="InvalidDataException">Thrown if the file format is invalid.</exception>
	///
	/// <remarks>
	/// Data size must not exceed 2 GB
	/// </remarks>
	public static Storage Load(string path, string masterPassword)
	{
		using BinaryReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.ASCII);
		
		// Read magic number
		uint magicNumber = reader.ReadUInt32();
		
		if (magicNumber != MagicNumber)
			throw new InvalidDataException();

		Version version = ReadVersion(reader);
		
		// In the future, backward compatibility can be realized through versioning
		if (version < new Version(1, 0))
			throw new InvalidDataException();

		string name = ReadName(reader);
		int iterations = reader.ReadInt32();
		
		byte[] encryptedJson = ReadData(reader)!;
		
		bool useHardwareBinding = reader.ReadBoolean();
		bool useTrashcan = reader.ReadBoolean();

		byte[]? image = ReadData(reader, true);
		
		if (useHardwareBinding)
			masterPassword = CreateHashWithSecretKey(GetDeviceIdentifiers(), masterPassword);
		
		string fileContent;
		
		{
			byte[] compressedString = Encryptor.DecryptBytes(encryptedJson, masterPassword, iterations);
			fileContent = Encoding.UTF8.GetString(Compressor.Decompress(compressedString));
		}
		
		Storage storage = FromJson(fileContent);

		StorageSettings settings = new()
		{
			Iterations = iterations,
			UseHardwareBinding = useHardwareBinding,
			UseTrashcan = useTrashcan,
			Name = name,
			ImageData = image
		};

		storage.Settings = settings;

		return storage;
	}

	#region Write

	private static void WriteVersion(BinaryWriter writer)
	{
		writer.Write(MagicNumber);
		writer.Write(MajorVersion);
		writer.Write(MinorVersion);
	}

	private static void WriteName(BinaryWriter writer, string name)
	{
		// Since ASCII does not support Cyrillic and other specific characters,
		// we store each character as a number of type short
		
		writer.Write(name.Length);
		foreach (char c in name) 
			writer.Write((short)c);
	}

	private static void WriteData(BinaryWriter writer, byte[]? data, bool compress = false)
	{
		// Maximum size of data - 2 GB
		
		if (data is null)
		{
			writer.Write(-1);
			return;
		}

		if (compress)
			data = Compressor.Compress(data);
		
		writer.Write(data.Length);
		
		for (int i = 0; i < data.Length; i += BufferSize)
		{
			int bytesToWrite = Math.Min(data.Length - i, BufferSize);
			writer.Write(data, i, bytesToWrite);
		}
	}
	
	private static string ToJson(Storage storage)
	{
		return JsonSerializer.Serialize(storage, GenerationContexts.StorageGenerationContext.Default.Storage);
	}

	#endregion

	#region Read

	private static Version ReadVersion(BinaryReader reader)
	{
		// Read major and minor version file
		byte majorVersion = reader.ReadByte();
		byte minorVersion = reader.ReadByte();
		
		return new Version(majorVersion, minorVersion);
	}

	private static string ReadName(BinaryReader reader)
	{
		int nameLength = reader.ReadInt32();
		
		StringBuilder name = new();
		for (int i = 0; i < nameLength; i++) 
			name.Append((char)reader.ReadInt16());

		return name.ToString();
	}

	private static byte[]? ReadData(BinaryReader reader, bool isCompressed = false)
	{
		// Maximum size of data - 2 GB
		
		int count = reader.ReadInt32();

		if (count == -1)
			return null;
		
		byte[] data = new byte[count];

		for (int i = 0; i < count; i += BufferSize)
		{
			int bytesToRead = Math.Min(count - i, BufferSize);
			byte[] buffer = reader.ReadBytes(bytesToRead);
			
			Array.Copy(buffer, 0, data, i, bytesToRead);
		}
		
		if (isCompressed)
			data = Compressor.Decompress(data);

		return data;
	}
	
	private static Storage FromJson(string json)
	{
		return JsonSerializer.Deserialize(json, GenerationContexts.StorageGenerationContext.Default.Storage) ??
		       throw new NullReferenceException();
	}

	#endregion

	private static string CreateHashWithSecretKey(string hash, string masterPassword)
	{
		using HMACSHA3_512 hmac = new(Encoding.UTF8.GetBytes(masterPassword));

		byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
		string masterPasswordHash = BitConverter.ToString(hashValue).Replace("-", "");
		
		return masterPasswordHash;
	}
    
	private static string GetDeviceIdentifiers()
	{
		HardwareInfo hardwareInfo = new();
		
		string?[] processorIds = hardwareInfo.GetProcessorIds();
		string?[] memoryIds = hardwareInfo.GetMemoryIds();
		string?[] videoControllerIds = hardwareInfo.GetVideoControllerIds();
		string?[] motherboardIds = hardwareInfo.GetMotherboardIds();
		
		string result = string.Empty;

		foreach (string? processorId in processorIds)
			result += processorId;

		foreach (string? memoryId in memoryIds)
			result += memoryId;

		foreach (string? videoControllerId in videoControllerIds)
			result += videoControllerId;

		foreach (string? motherboardId in motherboardIds)
			result += motherboardId;

		return result;
	}
}