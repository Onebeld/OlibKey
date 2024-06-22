using System.Globalization;
using Avalonia;
using Avalonia.Controls.Converters;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using OlibKey.Core.Helpers;

namespace OlibKey.Core.Converters;

public static class GeneralConverters
{
	public static readonly IValueConverter EmptyToNotDataConverter =
		new FuncValueConverter<string?, string?>(str =>
			string.IsNullOrWhiteSpace(str) ? OlibKeyApp.GetResource<string>("NoName") : str);

	public static readonly IValueConverter ComplexityPasswordConverter =
		new FuncValueConverter<string, double>(PasswordChecker.GetPasswordComplexity);

	public static readonly IValueConverter ObjectToString =
		new FuncValueConverter<object, string?>(value => value?.ToString());

	public static readonly IValueConverter IntToDoubleConverter =
		new FuncValueConverter<int, double>(value => value);
	

	public static readonly IValueConverter BytesToImageConverter =
		new FuncValueConverter<byte[]?, Bitmap?>(bytes =>
		{
			if (bytes is null) return null;
			
			using MemoryStream memoryStream = new(bytes);
			return new Bitmap(memoryStream);
		});

	public static readonly IValueConverter BytesToSmallImageConverter =
		new FuncValueConverter<byte[]?, Bitmap?>(imageData =>
		{
			Bitmap? bitmap =
				(Bitmap?)BytesToImageConverter.Convert(imageData, typeof(Bitmap), null, CultureInfo.CurrentCulture);

			return bitmap?.CreateScaledBitmap(new PixelSize(26, 26), BitmapInterpolationMode.LowQuality);
		});

	public static readonly StringFormatConverter StringFormat = new();
}