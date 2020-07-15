using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Views.Controls;
using System;

namespace OlibKey.Views.Windows
{
	public class ReminderWindow : Window
	{
		public LoginListItem LoginListItem;
		public TextBlock _tbName;
		public TextBlock _tbTime;

		private bool _dialogResult = false;

		public ReminderWindow() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbName = this.FindControl<TextBlock>("tbName");
			_tbTime = this.FindControl<TextBlock>("tbTime");
			Closing += ReminderWindow_Closing;
		}

		private void ReminderWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_dialogResult)
			{
				LoginListItem.ReminderTimer.Interval = new TimeSpan(0, 5, 0);
				LoginListItem.ReminderTimer.Start();
			}
			else LoginListItem.LoginItem.IsReminderActive = false;
		}

		private void ButtonPause(object sender, RoutedEventArgs e)
		{
			_dialogResult = true;
			Close();
		}

		private void ButtonShutdown(object sender, RoutedEventArgs e)
		{
			_dialogResult = false;
			Close();
		}
	}
}
