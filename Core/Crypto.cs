using System;
using System.Security.Cryptography;

namespace OlibKey.Core
{
	public static class Crypto
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
	}
}
