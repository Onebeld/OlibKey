using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using OlibKey.Core;
using OlibKey.Core.Views.MainWindowPages;
using PleasantUI.Controls;
using PleasantUI.Core;

namespace OlibKey;

public partial class MainWindow : PleasantWindow
{
    public MainWindow()
    {
        InitializeComponent();

        SettingsPage.FuncControl += () => new SettingsPage();
        PasswordManagerPage.FuncControl += () => new UserControl();
        AboutPage.FuncControl += () => new UserControl();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        Closed += OnClosed;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        OlibKeySettings.Save();
        PleasantSettings.Instance.Save();
    }
}