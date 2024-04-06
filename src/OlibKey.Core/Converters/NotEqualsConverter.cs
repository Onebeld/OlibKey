using System.Globalization;
using Avalonia.Data.Converters;

namespace OlibKey.Core.Converters;

public class NotEqualsConverter : IValueConverter
{
	public static readonly NotEqualsConverter Instance = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		parameter?.ToString() != value?.ToString();

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}