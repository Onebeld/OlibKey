using PleasantUI;

namespace OlibKey.Core.Models.StorageUnits;

public class Tag(string name, int count) : ViewModelBase
{
	public string Name
	{
		get => name;
		set => RaiseAndSet(ref name, value);
	}

	public int Count
	{
		get => count;
		set => RaiseAndSet(ref count, value);
	}
}