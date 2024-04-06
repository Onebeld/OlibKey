namespace OlibKey.Core.Models.StorageModels.SectionFields;

public class TextField : SectionField
{
	private string? _value;

	public string? Value
	{
		get => _value;
		set => RaiseAndSetNullIfEmpty(ref _value, value);
	}
}