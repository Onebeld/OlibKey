using Avalonia.Collections;
using PleasantUI;

namespace OlibKey.Core.Models.StorageUnits;

public class Section : ViewModelBase
{
	private string _name = string.Empty;
	private AvaloniaList<CustomField> _customFields = [];

	public string Name
	{
		get => _name;
		set => RaiseAndSet(ref _name, value);
	}

	public AvaloniaList<CustomField> CustomFields
	{
		get => _customFields;
		set => RaiseAndSet(ref _customFields, value);
	}
}