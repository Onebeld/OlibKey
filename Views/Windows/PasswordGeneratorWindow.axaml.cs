using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OlibKey.Views.Windows
{
	public class PasswordGeneratorWindow : Window
	{
		public PasswordGeneratorWindow()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = App.Settings;
		}
	}
}
