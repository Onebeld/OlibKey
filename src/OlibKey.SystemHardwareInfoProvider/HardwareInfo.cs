using OlibKey.SystemHardwareInfoProvider.Platforms;

namespace OlibKey.SystemHardwareInfoProvider;

/// <summary>
/// Responsible for providing information about the system hardware.
/// </summary>
public class HardwareInfo
{
	private readonly IHardwareInfo _hardwareInfo;
																																	
	/// <summary>
	/// Initializes a new instance of the <see cref="HardwareInfo"/> class. 
	/// </summary>
	/// <exception cref="NotSupportedException">If the operating system is not supported</exception>
	public HardwareInfo()
	{
		if (OperatingSystem.IsWindows())
			_hardwareInfo = new WindowsHardwareInfo();
		else throw new NotSupportedException();
	}

	/// <summary>
	/// Gets the processor IDs.
	/// </summary>
	/// <returns>Processor IDs</returns>
	public string?[] GetProcessorIds()
	{
		return _hardwareInfo.GetProcessorIds();
	}
	
	/// <summary>
	/// Gets the mother board IDs.
	/// </summary>
	/// <returns>Mother board IDs</returns>
	public string?[] GetMotherboardIds()
	{
		return _hardwareInfo.GetMotherboardIds();
	}
	
	/// <summary>
	/// Gets the memory IDs.
	/// </summary>
	/// <returns>Memory IDs</returns>
	public string?[] GetMemoryIds()
	{
		return _hardwareInfo.GetMemoryIds();
	}
	
	/// <summary>
	/// Gets the video controller IDs.
	/// </summary>
	/// <returns>Video controller IDs</returns>
	public string?[] GetVideoControllerIds()
	{
		return _hardwareInfo.GetVideoControllerIds();
	}
}