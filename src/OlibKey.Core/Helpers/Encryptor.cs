using System.Security.Cryptography;
using System.Text;

namespace OlibKey.Core.Helpers;

public static class Encryptor
{
	private static byte[] GetRandomBytes()
	{
		byte[] ba = new byte[SaltLength];
		RandomNumberGenerator.Create().GetBytes(ba);
		return ba;
	}

	private const int SaltLength = 128;

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

	/*public static byte[] EncryptBytes(byte[] data, string password, int iterations)
	{
	    byte[] salt = new byte[SaltLength];
	    RandomNumberGenerator.Create().GetBytes(salt);

	    Rfc2898DeriveBytes keyDerivation = new(password, salt, iterations, HashAlgorithmName.SHA256);
	    byte[] key = keyDerivation.GetBytes(32);

	    byte[] iv = new byte[16];
	    RandomNumberGenerator.Create().GetBytes(iv);

	    using Aes aes = Aes.Create();

	    aes.Key = key;
	    aes.IV = iv;
	    aes.Mode = CipherMode.CBC;
	    aes.Padding = PaddingMode.PKCS7;

	    using MemoryStream memoryStream = new();
	    using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

	    cryptoStream.Write(data, 0, data.Length);

	    byte[] encryptedBytes = memoryStream.ToArray();

	    byte[] outputBytes = new byte[salt.Length + iv.Length + encryptedBytes.Length];
	    Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);
	    Buffer.BlockCopy(iv, 0, outputBytes, salt.Length, iv.Length);
	    Buffer.BlockCopy(encryptedBytes, 0, outputBytes, salt.Length + iv.Length, encryptedBytes.Length);

	    return outputBytes;
	}*/

	/*public static byte[] DecryptBytes(byte[] data, string password, int iterations)
	{
	    byte[] salt = new byte[SaltLength];
	    Buffer.BlockCopy(data, 0, salt, 0, salt.Length);

	    byte[] iv = new byte[16];
	    Buffer.BlockCopy(data, salt.Length, iv, 0, iv.Length);

	    byte[] encryptedData = new byte[data.Length - salt.Length - iv.Length];
	    Buffer.BlockCopy(data, salt.Length + iv.Length, encryptedData, 0, encryptedData.Length);

	    Rfc2898DeriveBytes keyDerivation = new(password, salt, iterations, HashAlgorithmName.SHA256);
	    byte[] key = keyDerivation.GetBytes(32);

	    using Aes aes = Aes.Create();

	    aes.Key = key;
	    aes.IV = iv;
	    aes.Mode = CipherMode.CBC;
	    aes.Padding = PaddingMode.PKCS7;

	    using MemoryStream memoryStream = new();
	    using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

	    cryptoStream.Write(encryptedData, 0, encryptedData.Length);

	    byte[] decryptedBytes = memoryStream.ToArray();

	    return decryptedBytes;
	}*/

	/// <summary>
	/// Encrypts text
	/// </summary>
	/// <param name="text">Original text</param>
	/// <param name="masterPassword">Master password</param>
	/// <param name="iterations">Number of encryption iterations</param>
	/// <returns>Encrypted text</returns>
	public static string EncryptString(string text, string masterPassword, int iterations)
	{
		string encryptString = text;
		byte[] baPwd = Encoding.UTF8.GetBytes(masterPassword);
		byte[] baPwdHash = SHA256.HashData(baPwd);

		byte[] baText = Encoding.UTF8.GetBytes(encryptString);
		byte[] baSalt = GetRandomBytes();
		byte[] baEncrypted = new byte[baSalt.Length + baText.Length];
		for (int i = 0; i < baSalt.Length; i++) baEncrypted[i] = baSalt[i];
		for (int i = 0; i < baText.Length; i++) baEncrypted[i + baSalt.Length] = baText[i];
		baEncrypted = AES_Encrypt(baEncrypted, baPwdHash, iterations);

		encryptString = Convert.ToBase64String(baEncrypted);

		return encryptString;
	}

	/// <summary>
	/// Decrypts text
	/// </summary>
	/// <param name="text">Encrypted text</param>
	/// <param name="masterPassword">Master password</param>
	/// <param name="iterations">Number of decryption iterations</param>
	/// <returns>Decrypted text</returns>
	public static string DecryptString(string text, string masterPassword, int iterations)
	{
		string result = text;

		byte[] baPwdHash = SHA256.HashData(Encoding.UTF8.GetBytes(masterPassword));

		byte[] baText = Convert.FromBase64String(result);
		byte[] baDecrypted = AES_Decrypt(baText, baPwdHash, iterations);
		byte[] baResult = new byte[baDecrypted.Length - SaltLength];
		for (int i = 0; i < baResult.Length; i++) baResult[i] = baDecrypted[i + SaltLength];

		result = Encoding.UTF8.GetString(baResult);


		return result;
	}
}