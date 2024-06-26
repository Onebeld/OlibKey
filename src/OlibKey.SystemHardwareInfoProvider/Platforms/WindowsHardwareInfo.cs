using WmiLight;

namespace OlibKey.SystemHardwareInfoProvider.Platforms;

internal class WindowsHardwareInfo : IHardwareInfo
{
	private readonly WmiConnection _wmiConnection = new();
	
	private static string?[]? _processorIds;
	private static string?[]? _motherboardIds;
	private static string?[]? _memoryIds;
	private static string?[]? _videoControllerIds;

	public string?[] GetProcessorIds()
	{
		return _processorIds ??= GetWmiData("SELECT * FROM Win32_Processor", "ProcessorId");
	}

	public string?[] GetMotherboardIds()
	{
		return _motherboardIds ??= GetWmiData("SELECT * FROM Win32_BaseBoard", "SerialNumber");
	}

	public string?[] GetMemoryIds()
	{
		return _memoryIds ??= GetWmiData("SELECT * FROM Win32_PhysicalMemory", "SerialNumber");
	}

	public string?[] GetVideoControllerIds()
	{
		return _videoControllerIds ??= GetWmiData("SELECT * FROM Win32_VideoController", "PNPDeviceID");
	}
	
	private string?[] GetWmiData(string wmiQuery, string wmiProperty)
	{
		WmiObject[] devices = _wmiConnection.CreateQuery(wmiQuery).ToArray();

		int devicesCount = devices.Length;
		
		string?[] result = new string[devicesCount];

		for (int i = 0; i < devicesCount; i++) 
			result[i] = devices[i][wmiProperty].ToString();

		return result;
	}
}