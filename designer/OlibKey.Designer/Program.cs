using Avalonia;
using System;

namespace OlibKey.Designer;

class Program
{
	[STAThread]
	public static void Main(string[] args) => BuildAvaloniaApp()
		.StartWithClassicDesktopLifetime(args);

	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace();
}