using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using OlibKey.Core;
using OlibKey.Core.Settings;
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

		OlibKeyApp.ViewModel.NotificationManager = new WindowNotificationManager(this)
		{
			Position = NotificationPosition.TopRight,
			MaxItems = 3,
			ZIndex = 1
		};

		Closed += OnClosed;
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		
		OlibKeyApp.ViewModel.CheckUpdate();
	}

	private void OnClosed(object? sender, EventArgs e)
	{
		OlibKeyApp.ViewModel.Save();
		
		OlibKeySettings.Save();
		PleasantSettings.Save();
	}
}