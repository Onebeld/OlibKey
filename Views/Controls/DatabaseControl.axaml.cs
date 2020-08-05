using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibKey.ViewModels.Controls;
using OlibKey.ViewModels.Pages;

namespace OlibKey.Views.Controls
{
	public class DatabaseControl : UserControl
	{
		public DatabaseControlViewModel ViewModel { get; set; }

		private ListBox _listBox;

		public DatabaseControl() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = ViewModel = new DatabaseControlViewModel();

			_listBox = this.FindControl<ListBox>("listBox");

			_listBox.PointerPressed += _listBox_PointerPressed;
		}

		private void _listBox_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
		{
			if (ViewModel.IsUnlockDatabase || ViewModel.SelectedIndex != -1)
			{
				ViewModel.SelectedIndex = -1;
				ViewModel.Router.Navigate.Execute(new StartPageViewModel());
			}
		}
	}
}
