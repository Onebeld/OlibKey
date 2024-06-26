using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using OlibKey.RemoteRequestHandler;
using PleasantUI.Controls;

namespace OlibKey;

public class App : OlibKeyApp
{
	public override void Initialize() => AvaloniaXamlLoader.Load(this);

	public override void OnFrameworkInitializationCompleted()
	{
		base.OnFrameworkInitializationCompleted();

		if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
			return;

		if (Design.IsDesignMode)
		{
			desktop.MainWindow = new Window
			{
				DataContext = ViewModel
			};

			return;
		}

		Task.Run(OlibKeyWeb.RunApp);

		desktop.ShutdownRequested += async (sender, args) =>
		{
			await OlibKeyWeb.StopApp();
		};

		OpenMainWindowAction = () =>
		{
			if (desktop.MainWindow?.PlatformImpl is not null)
			{
				desktop.MainWindow.Topmost = true;
				desktop.MainWindow.Topmost = false;
				
				return;
			}
			
			Main = new MainWindow
			{
				DataContext = ViewModel
			};

			TopLevel = TopLevel.GetTopLevel(Main as PleasantWindow) ??
			           throw new NullReferenceException("TopLevel is null");

			desktop.MainWindow = Main as PleasantWindow;
			
			desktop.MainWindow?.Show();
		};

		OpenMainWindowAction.Invoke();
	}
}