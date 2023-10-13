using System;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using OlibKey.Android.Views;
using OlibKey.Core;
using OlibKey.Core.Views.MainWindowPages;
using PleasantUI.Controls;
using PleasantUI.Core;

namespace OlibKey.Android;

public partial class MainView : PleasantView
{
    public MainView()
    {
        InitializeComponent();
        
        SettingsPage.FuncControl += () => new SettingsPage();
        PasswordManagerPage.FuncControl += () => new PasswordManagerPage();
        AboutPage.FuncControl += () => new AboutPage();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        OlibKeyApp.ViewModel.NotificationManager = new PleasantNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 3,
            ZIndex = 1
        };
    }
}