namespace OlibKey.Core.Structures;

public readonly struct Language(string name, string key, params string[] additionalKeys)
{
	public string Name { get; } = name;

	public string Key { get; } = key;

	public string[] AdditionalKeys { get; } = additionalKeys;
}