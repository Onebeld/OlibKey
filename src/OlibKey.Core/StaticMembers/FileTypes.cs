using Avalonia.Platform.Storage;

namespace OlibKey.Core.StaticMembers;

public static class FileTypes
{
    public static readonly FilePickerFileType[] Images = 
    {
        new(OlibKeyApp.GetLocalizationString("ImageFiles"))
        {
            Patterns = new[] { "*.png", "*.jpg", "*.bmp" }
        }
    };
    
    public static readonly FilePickerFileType[] Olib = 
    {
        new(OlibKeyApp.GetLocalizationString("OlibFiles"))
        {
            Patterns = new [] { "*.olib" }
        }
    };

    public static readonly FilePickerFileType[] Any =
    {
        new("Any files")
        {
            Patterns = new[] { "*.*" }
        }
    };
}