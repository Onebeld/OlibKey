using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
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
        
        Main = new MainWindow
        {
            DataContext = ViewModel
        };
            
        TopLevel = TopLevel.GetTopLevel(Main as PleasantWindow) ?? throw new NullReferenceException("TopLevel is null");
            
        desktop.MainWindow = Main as PleasantWindow;
    }
}