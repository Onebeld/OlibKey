﻿using System.Globalization;
using Avalonia.Data.Converters;

namespace OlibKey.Core.Converters;

public class EqualsConverter : IValueConverter
{
	public static readonly EqualsConverter Instance = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return parameter?.ToString() == value?.ToString();
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}