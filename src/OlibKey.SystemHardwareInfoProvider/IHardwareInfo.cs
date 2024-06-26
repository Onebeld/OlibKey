namespace OlibKey.SystemHardwareInfoProvider;

internal interface IHardwareInfo
{
	string?[] GetProcessorIds();
	
	string?[] GetMotherboardIds();
	
	string?[] GetMemoryIds();

	string?[] GetVideoControllerIds();
}