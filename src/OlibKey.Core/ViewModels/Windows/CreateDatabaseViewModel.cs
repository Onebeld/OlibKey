using Avalonia;
using Avalonia.Media.Imaging;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures;
using PleasantUI;
using PleasantUI.Controls;

namespace OlibKey.Core.ViewModels.Windows;

public class CreateDatabaseViewModel : ViewModelBase
{
    private string _masterPassword = string.Empty;
    
    private string _name = string.Empty;
    private string? _imageData;
    private int _iterations = 10000;
    private bool _useTrash = true;
    
    private string _path = string.Empty;

    #region Properties

    public string MasterPassword
    {
        get => _masterPassword;
        set => RaiseAndSet(ref _masterPassword, value);
    }

    public string Name
    {
        get => _name;
        set => RaiseAndSet(ref _name, value);
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

    public string? ImageData
    {
        get => _imageData;
        set => RaiseAndSet(ref _imageData, value);
    }

    public string Path
    {
        get => _path;
        set => RaiseAndSet(ref _path, value);
    }

    #endregion

    public async void SelectPath()
    {
        string? databasePath = await StorageProvider.SaveFile(OlibKeyApp.TopLevel, 
            pickerFileTypes: FileTypes.Olib, 
            defaultExtension: "olib", 
            suggestedFileName: "NewDatabase");
        
        if (databasePath is null) return;

        Path = databasePath;
    }

    public async void LoadImage()
    {
        string? imagePath = await StorageProvider.SelectFile(OlibKeyApp.TopLevel, pickerFileTypes: FileTypes.Images);
        
        if (imagePath is null) return;
        
        LoadImage(imagePath);
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

    public void CloseWithResult(ContentDialog dialog)
    {
        if (string.IsNullOrWhiteSpace(MasterPassword) || string.IsNullOrWhiteSpace(Path) || string.IsNullOrWhiteSpace(Name))
            return;
        
        dialog.Close(true);
    }

    public void Close(ContentDialog dialog)
    {
        dialog.Close(false);
    }
}