namespace OlibKey.Core.Models.StorageUnits.CustomFieldTypes;

public class PasswordField : CustomField
{
	private string? _value;

	public string? Value
	{
		get => _value;
		set => RaiseAndSetNullIfEmpty(ref _value, value);
	}
}