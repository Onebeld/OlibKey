using Avalonia;
using Avalonia.Media.Imaging;

namespace OlibKey.Core.Helpers;

public static class BitmapHelpers
{
	public static Bitmap ChangeSize(this Bitmap bitmap, int width, int height)
	{
		return bitmap.CreateScaledBitmap(new PixelSize(width, height), BitmapInterpolationMode.LowQuality);
	}

	public static string ToBase64String(this Bitmap bitmap)
	{
		using MemoryStream memoryStream = new();
		
		bitmap.Save(memoryStream);

		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static Bitmap Base64StringToBitmap(string base64StringImage)
	{
		using MemoryStream memoryStream = new(Convert.FromBase64String(base64StringImage));

		Bitmap bitmap = new(memoryStream);

		return bitmap;
	}
}