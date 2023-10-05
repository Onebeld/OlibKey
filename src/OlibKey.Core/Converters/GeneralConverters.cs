using System.Text;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using OlibKey.Core.Helpers;

namespace OlibKey.Core.Converters;

public static class GeneralConverters
{
    public static readonly IValueConverter EmptyToNotDataConverter =
        new FuncValueConverter<string?, string?>(str => string.IsNullOrWhiteSpace(str) ? OlibKeyApp.GetResource<string>("NoName") : str);
    
    public static readonly IValueConverter ComplexityPasswordConverter =
        new FuncValueConverter<string, double>(PasswordChecker.GetPasswordComplexity);
    
    public static readonly IValueConverter ImageDataToImageConverter =
        new FuncValueConverter<string?, Bitmap?>(imageData =>
        {
            if (imageData is null) return null;

            byte[] array = Convert.FromBase64String(imageData);
            using MemoryStream memoryStream = new(array);

            return new Bitmap(memoryStream);
        });
}