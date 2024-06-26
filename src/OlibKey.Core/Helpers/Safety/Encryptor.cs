using System.Security.Cryptography;
using System.Text;

namespace OlibKey.Core.Helpers.Safety;

public static class Encryptor
{
	private const int SaltLength = 128;
	
	/// <summary>
	/// Encrypts text
	/// </summary>
	/// <param name="text">Original text</param>
	/// <param name="masterPassword">Master password</param>
	/// <param name="iterations">Number of encryption iterations</param>
	/// <returns>Encrypted text</returns>
	public static byte[] Encrypt(byte[] baText, string masterPassword, int iterations)
	{
		byte[] baPwd = Encoding.UTF8.GetBytes(masterPassword);
		byte[] baPwdHash = SHA256.HashData(baPwd);

		byte[] baSalt = GetRandomBytes();
		byte[] baEncrypted = new byte[baSalt.Length + baText.Length];
		for (int i = 0; i < baSalt.Length; i++) baEncrypted[i] = baSalt[i];
		for (int i = 0; i < baText.Length; i++) baEncrypted[i + baSalt.Length] = baText[i];
		baEncrypted = AES_Encrypt(baEncrypted, baPwdHash, iterations);

		return baEncrypted;
	}

	public static byte[] EncryptString(string text, string masterPassword, int iterations)
	{
		byte[] baText = Encoding.UTF8.GetBytes(text);

		return Encrypt(baText, masterPassword, iterations);
	}
	
	/// <summary>
	/// Decrypts text
	/// </summary>
	/// <param name="text">Encrypted text</param>
	/// <param name="masterPassword">Master password</param>
	/// <param name="iterations">Number of decryption iterations</param>
	/// <returns>Decrypted text</returns>
	public static byte[] DecryptBytes(byte[] text, string masterPassword, int iterations)
	{
		byte[] baPwdHash = SHA256.HashData(Encoding.UTF8.GetBytes(masterPassword));

		byte[] baDecrypted = AES_Decrypt(text, baPwdHash, iterations);
		byte[] baResult = new byte[baDecrypted.Length - SaltLength];
		for (int i = 0; i < baResult.Length; i++) baResult[i] = baDecrypted[i + SaltLength];

		return baResult;
	}
	
	public static string DecryptToString(byte[] text, string masterPassword, int iterations)
	{
		return Encoding.UTF8.GetString(DecryptBytes(text, masterPassword, iterations));
	}
	
	private static byte[] GetRandomBytes()
	{
		byte[] ba = new byte[SaltLength];
		RandomNumberGenerator.Create().GetBytes(ba);
		return ba;
	}

	private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, int iterations)
	{
		byte[] saltBytes = new byte[SaltLength];
		for (int i = 0; i < SaltLength; i++)
			saltBytes[i] = (byte)(i + 1);

		using MemoryStream memoryStream = new();
		using (Rfc2898DeriveBytes key = new(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
		{
			Aes aes = Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;

			aes.Key = key.GetBytes((int)(aes.KeySize * 0.125));
			aes.IV = key.GetBytes((int)(aes.BlockSize * 0.125));

			using (CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
				cryptoStream.Close();
			}
		}

		return memoryStream.ToArray();
	}

	private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, int iterations)
	{
		byte[] saltBytes = new byte[SaltLength];
		for (int i = 0; i < SaltLength; i++)
			saltBytes[i] = (byte)(i + 1);

		using MemoryStream memoryStream = new();
		using (Rfc2898DeriveBytes key = new(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
		{
			Aes aes = Aes.Create();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;

			aes.Key = key.GetBytes((int)(aes.KeySize * 0.125));
			aes.IV = key.GetBytes((int)(aes.BlockSize * 0.125));

			using (CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
				cryptoStream.Close();
			}
		}

		return memoryStream.ToArray();
	}
}