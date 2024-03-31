using Avalonia.Collections;
using PleasantUI;

namespace OlibKey.Core.Models.Database;

public class Trashcan : ViewModelBase
{
    private AvaloniaList<Data> _data = [];

    public AvaloniaList<Data> Data
    {
        get => _data;
        set => RaiseAndSet(ref _data, value);
    }
}