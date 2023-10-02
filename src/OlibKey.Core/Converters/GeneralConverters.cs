using Avalonia.Data.Converters;
using OlibKey.Core.Helpers;

namespace OlibKey.Core.Converters;

public static class GeneralConverters
{
    public static readonly IValueConverter EmptyToNotDataConverter =
        new FuncValueConverter<string?, string?>(str => string.IsNullOrWhiteSpace(str) ? OlibKeyApp.GetResource<string>("NoName") : str);
    
    public static readonly IValueConverter ComplexityPasswordConverter =
        new FuncValueConverter<string, double>(PasswordChecker.GetPasswordComplexity);
}