using Avalonia.Controls.Primitives;
using OlibKey.Core;
using OlibKey.Core.Views.MainWindowPages;
using OlibKey.Views;
using PleasantUI.Controls;
using PleasantUI.Core;

namespace OlibKey;

public partial class MainWindow : PleasantWindow
{
    public MainWindow()
    {
        InitializeComponent();

        SettingsPage.FuncControl += () => new SettingsPage();
        PasswordManagerPage.FuncControl += () => new PasswordManagerPage();
        AboutPage.FuncControl += () => new AboutPage();
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