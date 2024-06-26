using System.Diagnostics;

namespace OlibKey.SystemHardwareInfoProvider.Platforms;

internal class UnixHardwareInfo : IHardwareInfo
{
	private static string[]? _processorIds;
	private static string[]? _motherboardIds;
	private static string[]? _memoryIds;
	private static string[]? _videoControllerIds;
	
	public string?[] GetProcessorIds()
	{
		throw new NotImplementedException();
	}

	public string?[] GetMotherboardIds()
	{
		throw new NotImplementedException();
	}

	public string?[] GetMemoryIds()
	{
		throw new NotImplementedException();
	}

	public string?[] GetVideoControllerIds()
	{
		throw new NotImplementedException();
	}

	private string ExecuteCommand(string command)
	{
		ProcessStartInfo processStartInfo = new()
		{
			FileName = "/bin/bash",
			Arguments = "-c \"" + command + "\"",
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		Process process = new Process { StartInfo = processStartInfo };
		process.Start();

		string output = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
		
		return output.Trim();
	}
}