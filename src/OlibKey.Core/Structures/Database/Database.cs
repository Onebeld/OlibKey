using Avalonia.Collections;
using PleasantUI;

namespace OlibKey.Core.Structures;

public class Database : ViewModelBase
{
    private DatabaseSettings _settings = null!;

    private AvaloniaList<Data> _data = new();

    private Trashcan _trashcan = new();

    public AvaloniaList<Data> Data
    {
        get => _data;
        set => RaiseAndSet(ref _data, value);
    }
    
    public Trashcan Trashcan
    {
        get => _trashcan;
        set => RaiseAndSet(ref _trashcan, value);
    }

    public DatabaseSettings Settings
    {
        get => _settings;
        set => RaiseAndSet(ref _settings, value);
    }
}