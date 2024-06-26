﻿using System.Globalization;
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


	public static readonly IValueConverter ImageDataToImageConverter =
		new FuncValueConverter<string?, Bitmap?>(imageData =>
		{
			if (imageData is null) return null;

			byte[] array = Convert.FromBase64String(imageData);
			using MemoryStream memoryStream = new(array);

			return new Bitmap(memoryStream);
		});

	public static readonly IValueConverter ImageDataToSmallImageConverter =
		new FuncValueConverter<string?, Bitmap?>(imageData =>
		{
			Bitmap? bitmap =
				(Bitmap?)ImageDataToImageConverter.Convert(imageData, typeof(Bitmap), null, CultureInfo.CurrentCulture);

			return bitmap?.CreateScaledBitmap(new PixelSize(26, 26), BitmapInterpolationMode.LowQuality);
		});

	public static readonly StringFormatConverter StringFormat = new();
}