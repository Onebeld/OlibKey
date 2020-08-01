using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Pages;
using ReactiveUI;
using Splat;
using System.Linq;

namespace OlibKey
{
	class Program
	{
		public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

		public static AppBuilder BuildAvaloniaApp() =>
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
			string file = args.FirstOrDefault();
			if (!string.IsNullOrWhiteSpace(file)) App.Settings.PathDatabase = file;

			App.MainWindowViewModel = new MainWindowViewModel();
			Locator.CurrentMutable.Register<IViewFor<CreateLoginPageViewModel>>(() => new CreateLoginPage());
			Locator.CurrentMutable.Register<IViewFor<StartPageViewModel>>(() => new StartPage());
			Locator.CurrentMutable.Register<IViewFor<LoginInformationPageViewModel>>(() => new LoginInformationPage());
			Locator.CurrentMutable.Register<IViewFor<EditLoginPageViewModel>>(() => new EditLoginPage());

			App.MainWindow = new MainWindow { DataContext = App.MainWindowViewModel };

			app.Run(App.MainWindow);
		}
	}
}
