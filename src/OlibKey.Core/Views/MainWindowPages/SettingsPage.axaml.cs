using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using OlibKey.Core.ViewModels;

namespace OlibKey.Core.Views.MainWindowPages;

public partial class SettingsPage : UserControl
{
    private readonly SettingsViewModel _viewModel;
    
    public SettingsPage()
    {
        InitializeComponent();

        _viewModel = new SettingsViewModel();
        DataContext = _viewModel;

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsLinux() || OperatingSystem.IsIOS())
        {
            OptionsDisplayItemCustomTitleBar.IsVisible = false;
            OptionsDisplayItemEnableBlur.IsVisible = false;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _viewModel.IsNotLoaded = false;
    }
}