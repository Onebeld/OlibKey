using Avalonia.Collections;
using PleasantUI;

namespace OlibKey.Core.Models.StorageModels;

public class Section : ViewModelBase
{
	private string _name = string.Empty;
	private AvaloniaList<SectionField> _sectionFields = [];

	public string Name
	{
		get => _name;
		set => RaiseAndSet(ref _name, value);
	}

	public AvaloniaList<SectionField> SectionFields
	{
		get => _sectionFields;
		set => RaiseAndSet(ref _sectionFields, value);
	}
}