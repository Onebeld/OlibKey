using Avalonia;
using Avalonia.Media.Imaging;
using PleasantUI;

namespace OlibKey.Core.Settings;

public class StorageSettings : ViewModelBase, ICloneable
{
    private string _name = string.Empty;
    private string? _imageData;
    private int _iterations;
    private bool _useTrash;

    public string Name
    {
        get => _name;
        set => RaiseAndSet(ref _name, value);
    }

    public string? ImageData
    {
        get => _imageData;
        set => RaiseAndSet(ref _imageData, value);
    }
    
    public int Iterations
    {
        get => _iterations;
        set => RaiseAndSet(ref _iterations, value);
    }
    
    public bool UseTrashcan
    {
        get => _useTrash;
        set => RaiseAndSet(ref _useTrash, value);
    }

    public void LoadImage(string path)
    {
        Bitmap bitmap;
        
        try
        {
            bitmap = new Bitmap(path);
        }
        catch { return; }

        bitmap = bitmap.CreateScaledBitmap(new PixelSize(80, 80), BitmapInterpolationMode.MediumQuality);

        using MemoryStream memoryStream = new();
        bitmap.Save(memoryStream);

        ImageData = Convert.ToBase64String(memoryStream.ToArray());
    }

    public object Clone()
    {
        return (StorageSettings)MemberwiseClone();
    }
}