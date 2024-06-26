namespace OlibKey.Core.Models.StorageUnits.CustomFieldTypes;

public class BoolField : CustomField
{
	private bool _value;

	public bool Value
	{
		get => _value;
		set => RaiseAndSet(ref _value, value);
	}
}