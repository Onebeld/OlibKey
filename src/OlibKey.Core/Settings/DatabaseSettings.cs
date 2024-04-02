using PleasantUI;

namespace OlibKey.Core.Settings;

public class DatabaseSettings : ViewModelBase
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
}