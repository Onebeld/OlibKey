using Avalonia.Platform.Storage;

namespace OlibKey.Core.Structures;

public static class FileTypes
{
    public static readonly FilePickerFileType[] Images = 
    {
        new(OlibKeyApp.GetLocalString("ImageFiles"))
        {
            Patterns = new[] { "*.png", "*.jpg", "*.bmp" }
        }
    };
    
    public static readonly FilePickerFileType[] Olib = 
    {
        new(OlibKeyApp.GetLocalString("OlibFiles"))
        {
            Patterns = new []{ "*.olib" }
        }
    };
}