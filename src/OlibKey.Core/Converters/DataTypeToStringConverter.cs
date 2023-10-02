using System.Globalization;
using Avalonia.Data.Converters;
using OlibKey.Core.Enums;

namespace OlibKey.Core.Converters;

public class DataTypeToStringConverter : IMultiValueConverter
{
    public static readonly DataTypeToStringConverter Instance = new();
    
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        DataType typeId = values[4] as DataType? ?? DataType.Login;

        switch (typeId)
        {
            case DataType.Login:
                if (!string.IsNullOrEmpty(values[0] as string))
                    return values[0];
                if (!string.IsNullOrEmpty(values[1] as string))
                    return values[1];
                return OlibKeyApp.GetResource<string>("NoData");
            case DataType.BankCard:
                return !string.IsNullOrEmpty(values[2] as string) ? values[2] : OlibKeyApp.GetResource<string>("NoData");
            case DataType.PersonalData:
                return !string.IsNullOrEmpty(values[3] as string) ? values[3] : OlibKeyApp.GetResource<string>("NoData");
                
            default:
                return string.Empty;
        }
    }
}