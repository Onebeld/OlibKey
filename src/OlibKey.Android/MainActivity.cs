using Android.App;
using Android.Content.PM;
using Avalonia.Android;
using OlibKey.Core;
using PleasantUI.Core;

namespace OlibKey.Android;

[Activity(
    Label = "OlibKey",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override void OnStop()
    {
        PleasantSettings.Instance.Save();
        OlibKeySettings.Save();
        
        base.OnStop();
    }
}