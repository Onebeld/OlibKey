using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibKey.ViewModels.Controls;

namespace OlibKey.Views.Controls
{
	public class DatabaseControl : UserControl
	{
		public DatabaseControlViewModel ViewModel { get; set; }

		public DatabaseControl() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = ViewModel = new DatabaseControlViewModel();
		}
	}
}
