using System.Globalization;
using Avalonia.Data.Converters;
using OlibKey.Core.Models.Database;

namespace OlibKey.Core.Converters;

public class DataToStringConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not Data data) return null;

		return data.Information;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}