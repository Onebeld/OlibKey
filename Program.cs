using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using OlibKey.Core;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Pages;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OlibKey
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
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
				.With(new Win32PlatformOptions { AllowEglInitialization = false, UseDeferredRendering = true })
				.With(new MacOSPlatformOptions { ShowInDock = true })
				.With(new AvaloniaNativePlatformOptions { UseGpu = true, UseDeferredRendering = true })
				.With(new X11PlatformOptions { UseGpu = true, UseEGL = true });

		private static void AppMain(Application app, string[] args)
		{
			List<string> files = args.ToList();

			App.MainWindowViewModel = new MainWindowViewModel { OpenStorages = files };

			Locator.CurrentMutable.Register<IViewFor<CreateLoginPageViewModel>>(() => new CreateLoginPage());
			Locator.CurrentMutable.Register<IViewFor<StartPageViewModel>>(() => new StartPage());
			Locator.CurrentMutable.Register<IViewFor<LoginInformationPageViewModel>>(() => new LoginInformationPage());
			Locator.CurrentMutable.Register<IViewFor<EditLoginPageViewModel>>(() => new EditLoginPage());

			App.MainWindow = new MainWindow { DataContext = App.MainWindowViewModel };

			app.Run(App.MainWindow);

		}
	}
}
