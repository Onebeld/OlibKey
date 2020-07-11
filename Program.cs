using System;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Pages;

namespace OlibKey
{
	class Program
    {
	    public static object Sync = new object();

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
		    try
		    {
				string file = args.FirstOrDefault();
				if (!string.IsNullOrWhiteSpace(file)) App.Settings.PathDatabase = file;

			    App.MainWindowViewModel = new MainWindowViewModel();
			    Locator.CurrentMutable.RegisterConstant<IScreen>(App.MainWindowViewModel);
			    Locator.CurrentMutable.Register<IViewFor<CreateLoginPageViewModel>>(() => new CreateLoginPage());
			    Locator.CurrentMutable.Register<IViewFor<StartPageViewModel>>(() => new StartPage());
			    Locator.CurrentMutable.Register<IViewFor<LoginInformationPageViewModel>>(() =>
				    new LoginInformationPage());
			    Locator.CurrentMutable.Register<IViewFor<EditLoginPageViewModel>>(() => new EditLoginPage());

			    App.MainWindow = new MainWindow {DataContext = Locator.Current.GetService<IScreen>()};

			    app.Run(App.MainWindow);
		    }
		    catch (Exception e)
		    {
			    try
			    {
					string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
					    $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log");
					string fullText =
					    $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] [{e.TargetSite.DeclaringType}.{e.TargetSite.Name}()]\n{e}\r\n";
				    lock (Sync) File.AppendAllText(filename, fullText, Encoding.GetEncoding("UTF-8"));
			    }
			    catch
			    {
				    // ignored
			    }
		    }
	    }
    }
}
