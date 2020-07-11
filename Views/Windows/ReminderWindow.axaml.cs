using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Views.Controls;

namespace OlibKey.Views.Windows
{
	public class ReminderWindow : Window
	{
		public LoginListItem LoginListItem;
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
			LoginListItem.ReminderTimer.Interval = new TimeSpan(0, 5, 0);
			LoginListItem.ReminderTimer.Start();
			Close();
		}

		private void ButtonShutdown(object sender, RoutedEventArgs e)
		{
			LoginListItem.LoginItem.IsReminderActive = false;
			Close();
		}
	}
}
