using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibKey.Views.Controls;

namespace OlibKey.Views.Windows
{
	public class ReminderWindow : Window
	{
		public AccountListItem AccountListItem;

		public ReminderWindow()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

		}
	}
}
