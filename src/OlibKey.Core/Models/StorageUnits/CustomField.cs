using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using OlibKey.Core.Models.StorageUnits.CustomFieldTypes;
using PleasantUI;

namespace OlibKey.Core.Models.StorageUnits;

[JsonDerivedType(typeof(BoolField), typeDiscriminator: "bool")]
[JsonDerivedType(typeof(PasswordField), typeDiscriminator: "password")]
[JsonDerivedType(typeof(TextField), typeDiscriminator: "text")]
public class CustomField : ViewModelBase
{
	private string _name = string.Empty;

	public string Name
	{
		get => _name;
		set => RaiseAndSet(ref _name, value);
	}

	public T ConvertField<T>() where T : CustomField, new()
	{
		return new T
		{
			Name = Name
		};
	}

	protected bool RaiseAndSetNullIfEmpty(ref string? field, string? value,
		[CallerMemberName] string? propertyName = null)
	{
		if (string.IsNullOrWhiteSpace(value))
			value = null;

		return RaiseAndSet(ref field, value, propertyName);
	}
}