using System.IO.Compression;
using System.Text;

namespace OlibKey.Core.Helpers;

/// <summary>
/// Provides methods for compressing and decompressing data.
/// </summary>
public static class Compressor
{
	/// <summary>
	/// Compresses a text
	/// </summary>
	/// <param name="text">Original text</param>
	/// <returns>Compressed text</returns>
	public static string Compress(string text) => Convert.ToBase64String(Compress(Encoding.UTF8.GetBytes(text)));

	/// <summary>
	/// Decompresses a compressed text
	/// </summary>
	/// <param name="text">Compressed text</param>
	/// <returns>Original text</returns>
	public static string Decompress(string text) => Encoding.UTF8.GetString(Decompress(Convert.FromBase64String(text)));

	/// <summary>
	/// Compresses a byte array
	/// </summary>
	/// <param name="buffer">Buffer</param>
	/// <returns>Compressed buffer</returns>
	public static byte[] Compress(byte[] buffer)
	{
		using MemoryStream output = new();

		using (MemoryStream input = new(buffer))
		{
			using (BrotliStream compressionStream = new(output, CompressionLevel.Optimal))
			{
				input.CopyTo(compressionStream);
			}
		}

		return output.ToArray();
	}

	/// <summary>
	/// Decompresses a compressed byte array
	/// </summary>
	/// <returns>Original byte array</returns>
	public static byte[] Decompress(byte[] compressedBuffer)
	{
		using MemoryStream output = new();

		using (MemoryStream input = new(compressedBuffer))
		{
			using (BrotliStream decompressionStream = new(input, CompressionMode.Decompress))
			{
				decompressionStream.CopyTo(output);
			}
		}

		return output.ToArray();
	}
}