using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OlibKey.Core;

namespace OlibKey;

public class App : OlibKeyApp
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
    }
}