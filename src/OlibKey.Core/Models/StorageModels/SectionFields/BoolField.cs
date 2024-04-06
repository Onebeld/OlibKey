namespace OlibKey.Core.Models.StorageModels.SectionFields;

public class BoolField : SectionField
{
	private bool _value;

	public bool Value
	{
		get => _value;
		set => RaiseAndSet(ref _value, value);
	}
}