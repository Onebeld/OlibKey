using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using OlibKey.Core;
using OlibKey.Structures;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Pages;
using ReactiveUI;
using Splat;
using System;
using System.IO;
using System.Linq;

namespace OlibKey
{
	public static class Program
	{
		public static Settings Settings { get; set; }

		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				Settings = File.Exists(AppDomain.CurrentDomain.BaseDirectory + "settings.xml")
				? SaveAndLoadSettings.LoadSettings()
				: new Settings();

				BuildAvaloniaApp().Start(AppMain, args);
			}
			catch (Exception ex)
			{
				Log.WriteFatal(ex);
			}
		}

		private static AppBuilder BuildAvaloniaApp() =>
			AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.LogToDebug()
				.UseReactiveUI()
				.With(new Win32PlatformOptions { AllowEglInitialization = Settings.UsingGPU, UseDeferredRendering = true })
				.With(new MacOSPlatformOptions { ShowInDock = true })
				.With(new AvaloniaNativePlatformOptions { UseGpu = Settings.UsingGPU, UseDeferredRendering = true })
				.With(new X11PlatformOptions { UseGpu = Settings.UsingGPU, UseEGL = true });

		private static void AppMain(Application app, string[] args)
		{
			App.MainWindowViewModel = new MainWindowViewModel { OpenStorages = args.ToList() };

			Locator.CurrentMutable.Register<IViewFor<CreateLoginPageViewModel>>(() => new CreateLoginPage());
			Locator.CurrentMutable.Register<IViewFor<StartPageViewModel>>(() => new StartPage());
			Locator.CurrentMutable.Register<IViewFor<LoginInformationPageViewModel>>(() => new LoginInformationPage());
			Locator.CurrentMutable.Register<IViewFor<EditLoginPageViewModel>>(() => new EditLoginPage());

			App.MainWindow = new MainWindow { DataContext = App.MainWindowViewModel };

			app.Run(App.MainWindow);
		}
	}
}
