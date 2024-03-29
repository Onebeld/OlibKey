using PleasantUI;

namespace OlibKey.Core.Structures;

public class Tag(string _name) : ViewModelBase
{
    private int _count;

    public string Name
    {
        get => _name;
        set => RaiseAndSet(ref _name, value);
    }

    public int Count
    {
        get => _count;
        set => RaiseAndSet(ref _count, value);
    }
}