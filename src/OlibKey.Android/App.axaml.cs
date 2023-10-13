using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using PleasantUI.Controls;

namespace OlibKey.Android;

public partial class App : OlibKeyApp
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
        
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplicationLifetime)
        {
            Main = new MainView()
            {
                DataContext = ViewModel
            };
            
            TopLevel = TopLevel.GetTopLevel(Main as PleasantView);
            
            singleViewApplicationLifetime.MainView = Main as Control;
        }
    }
}