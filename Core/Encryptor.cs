using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OlibKey.Core
{
	public static class Encryptor
	{
		private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

		public static int RandomInteger(int min, int max)
		{
			uint scale = uint.MaxValue;
			while (scale == uint.MaxValue)
			{
				byte[] fourBytes = new byte[4];
				Rand.GetBytes(fourBytes);
				scale = BitConverter.ToUInt32(fourBytes, 0);
			}

			return (int)(min + ((max - min) * (scale / (double)uint.MaxValue)));
		}

		private static byte[] GetRandomBytes()
		{
			byte[] ba = new byte[SaltLength];
			RandomNumberGenerator.Create().GetBytes(ba);
			return ba;
		}

		private static int SaltLength => 24;

		private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
		{
			byte[] saltBytes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

			using MemoryStream ms = new MemoryStream();

			using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, App.MainWindowViewModel.Iterations))
			{
				using RijndaelManaged aes = new RijndaelManaged { KeySize = 256, BlockSize = 128, Mode = CipherMode.CBC };
				aes.Key = key.GetBytes(aes.KeySize / 8);
				aes.IV = key.GetBytes(aes.BlockSize / 8);

				using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

				cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
				cs.Close();
			}

			return ms.ToArray();
		}

		private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, int iteration)
		{
			byte[] saltBytes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

			using MemoryStream ms = new MemoryStream();

			using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iteration))
			{
				using RijndaelManaged aes = new RijndaelManaged { KeySize = 256, BlockSize = 128, Mode = CipherMode.CBC };
				aes.Key = key.GetBytes(aes.KeySize / 8);
				aes.IV = key.GetBytes(aes.BlockSize / 8);

				using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

				cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
				cs.Close();
			}

			return ms.ToArray();
		}

		public static string EncryptString(string text, string password)
		{
			string encryptString = text;
			byte[] baPwd = Encoding.UTF8.GetBytes(password);
			byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

			for (int d = 0; d < App.MainWindowViewModel.NumberOfEncryptionProcedures; d++)
			{
				byte[] baText = Encoding.UTF8.GetBytes(encryptString);
				byte[] baSalt = GetRandomBytes();
				byte[] baEncrypted = new byte[baSalt.Length + baText.Length];
				for (int i = 0; i < baSalt.Length; i++) baEncrypted[i] = baSalt[i];
				for (int i = 0; i < baText.Length; i++) baEncrypted[i + baSalt.Length] = baText[i];
				baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

				encryptString = Convert.ToBase64String(baEncrypted);
			}

			return App.MainWindowViewModel.Iterations + ":" + App.MainWindowViewModel.NumberOfEncryptionProcedures + ":" + encryptString;
		}

		public static string DecryptString(string text, string password)
		{

			string[] split = text.Split(':');
			int iterations = int.Parse(split[0]);
			int numberOfEncryptionProcedures = int.Parse(split[1]);
			string result = split[2];

			byte[] baPwdHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));

			for (int d = 0; d < numberOfEncryptionProcedures; d++)
			{
				byte[] baText = Convert.FromBase64String(result);
				byte[] baDecrypted = AES_Decrypt(baText, baPwdHash, iterations);
				byte[] baResult = new byte[baDecrypted.Length - SaltLength];
				for (int i = 0; i < baResult.Length; i++) baResult[i] = baDecrypted[i + SaltLength];

				result = Encoding.UTF8.GetString(baResult);
			}

			App.MainWindowViewModel.Iterations = iterations;
			App.MainWindowViewModel.NumberOfEncryptionProcedures = numberOfEncryptionProcedures;

			return result;
		}
	}
}
