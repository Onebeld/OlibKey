using OlibKey.SystemHardwareInfoProvider;

namespace OlibKey.Core.UnitTests;

public class HardwareTests
{
	[Test]
	public void Get_Device_Identifiers()
	{
		HardwareInfo hardwareInfo = new();
		
		TestContext.WriteLine(hardwareInfo.GetProcessorIds().Aggregate((current, s) => current + s));
		TestContext.WriteLine(hardwareInfo.GetMotherboardIds().Aggregate((current, s) => current + s));
		TestContext.WriteLine(hardwareInfo.GetMemoryIds().Aggregate((current, s) => current + s));
		TestContext.WriteLine(hardwareInfo.GetVideoControllerIds().Aggregate((current, s) => current + s));
		
		Assert.Pass();
	}

}