using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace OlibKey.Core.Views.BasicElements;

public partial class StorageManagementButton : UserControl
{
    public StorageManagementButton()
    {
        InitializeComponent();

        IEnumerable<TextBlock> textBlocks = this.GetVisualDescendants().OfType<TextBlock>();

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            foreach (TextBlock textBlock in textBlocks)
            {
                if (textBlock.Classes.Any(x => x == "Hotkey"))
                    textBlock.IsVisible = false;
            }
        }
        else
        {
            foreach (TextBlock textBlock in textBlocks)
            {
                if (textBlock.Classes.Any(x => x == "Hotkey"))
                    textBlock.Text = PlatformKeyGestureConverter.ToPlatformString(KeyGesture.Parse(textBlock.Text));
            }
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
        
        ButtonStorageMenu.Flyout?.Hide();
    }
}