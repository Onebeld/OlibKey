using PleasantUI;

namespace OlibKey.Core.Models.Database;

public class Tag(string name) : ViewModelBase
{
    private int _count;

    public string Name
    {
        get => name;
        set => RaiseAndSet(ref name, value);
    }

    public int Count
    {
        get => _count;
        set => RaiseAndSet(ref _count, value);
    }
}