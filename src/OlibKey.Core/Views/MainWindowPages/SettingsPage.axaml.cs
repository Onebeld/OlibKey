using Avalonia.Controls;

namespace OlibKey.Core.Views.MainWindowPages;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsLinux() || OperatingSystem.IsIOS())
        {
            OptionsDisplayItemCustomTitleBar.IsVisible = false;
            OptionsDisplayItemEnableBlur.IsVisible = false;
        }
    }
}