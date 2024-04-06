using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace OlibKey.Core.Converters;

public class EnumToListConverter : IValueConverter
{
	public static readonly EnumToListConverter Instance = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return targetType.IsEnum ? targetType.GetEnumValues() : AvaloniaProperty.UnsetValue;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}