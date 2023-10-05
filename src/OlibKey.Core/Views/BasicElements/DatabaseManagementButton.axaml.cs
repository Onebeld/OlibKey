using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace OlibKey.Core.Views.BasicElements;

public partial class DatabaseManagementButton : UserControl
{
    public DatabaseManagementButton()
    {
        InitializeComponent();

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            TextBlockHotkeyN.IsVisible = false;
            TextBlockHotkeyS.IsVisible = false;
            TextBlockHotkeyO.IsVisible = false;
        }
        else
        {
            TextBlockHotkeyN.Text = PlatformKeyGestureConverter.ToPlatformString(KeyGesture.Parse("Ctrl+N"));
            TextBlockHotkeyS.Text = PlatformKeyGestureConverter.ToPlatformString(KeyGesture.Parse("Ctrl+S"));
            TextBlockHotkeyO.Text = PlatformKeyGestureConverter.ToPlatformString(KeyGesture.Parse("Ctrl+O"));
        }

        foreach (Control control in StackPanelMenuButtons.Children)
        {
            if (control is Button button)
                button.Click += MenuButtonsOnClick;
        }
    }

    private void MenuButtonsOnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
            button.Command?.Execute(button.CommandParameter);
        
        ButtonDatabaseMenu.Flyout?.Hide();
    }
}