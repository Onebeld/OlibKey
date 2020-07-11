using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Views.Controls;

namespace OlibKey.Views.Windows
{
	public class ReminderWindow : Window
	{
		public LoginListItem AccountListItem;
		public TextBlock _tbName;
		public TextBlock _tbTime;

		public ReminderWindow() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbName = this.FindControl<TextBlock>("tbName");
			_tbTime = this.FindControl<TextBlock>("tbTime");
		}

		private void ButtonPause(object sender, RoutedEventArgs e)
		{
			AccountListItem.ReminderTimer.Interval = new TimeSpan(0, 5, 0);
			AccountListItem.ReminderTimer.Start();
			Close();
		}

		private void ButtonShutdown(object sender, RoutedEventArgs e)
		{
			AccountListItem.LoginItem.IsReminderActive = false;
			Close();
		}
	}
}
