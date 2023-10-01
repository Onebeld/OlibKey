using PleasantUI;

namespace OlibKey.Core;

public class DatabaseSettings : ViewModelBase
{
    private int _iterations;
    private bool _useTrash;
    
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