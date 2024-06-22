using System.Diagnostics;
using System.Globalization;
using Avalonia.Interactivity;
using OlibKey.Core.Other;
using PleasantUI.Controls;

namespace OlibKey.Core.Windows;

public partial class UpdateWindow : ContentDialog
{
	private readonly Update _update;
	
	public UpdateWindow() => InitializeComponent();

	public UpdateWindow(Update update) : this()
	{
		_update = update;

		InitializeControls();
	}

	private void InitializeControls()
	{
		FollowLinkButton.Click += FollowLinkButtonOnClick;
		CloseButton.Click += CloseButtonOnClick;

		ReleaseTimeRun.Text = _update.PublishedAt.ToString(CultureInfo.CurrentCulture);
		VersionRun.Text = _update.Version.ToString();
		MarkdownScrollViewer.Markdown = _update.Body;
	}

	private void CloseButtonOnClick(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private void FollowLinkButtonOnClick(object? sender, RoutedEventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = _update.Link,
			UseShellExecute = true
		});
		
		Close();
	}
}