using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Views.Controls;
using System;

namespace OlibKey.Views.Windows
{
	public class ReminderWindow : Window
	{
		public AccountListItem AccountListItem;
		public TextBlock _tbName;
		public TextBlock _tbTime;

		public ReminderWindow()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbName = this.FindControl<TextBlock>("tbName");
			_tbTime = this.FindControl<TextBlock>("tbTime");
		}

		private async void ButtonPause(object sender, RoutedEventArgs e)
		{
			AccountListItem.timer.Interval = new TimeSpan(0, 5, 0);
			AccountListItem.timer.Start();
			Close();
		}

		private async void ButtonShutdown(object sender, RoutedEventArgs e)
		{
			AccountListItem.AccountItem.IsReminderActive = false;
			Close();
		}
	}
}
