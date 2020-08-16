using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using OlibKey.ViewModels.Windows;

namespace OlibKey.Views.Windows
{
	public class CheckingWeakPasswordsWindow : Window
	{
		private CheckingWeakPasswordsWindowViewModel ViewModel;

		private ProgressBar pbHard;

		public CheckingWeakPasswordsWindow()
		{
			InitializeComponent();
			DataContext = ViewModel = new CheckingWeakPasswordsWindowViewModel();
			ItemControls.ColorProgressBar(pbHard);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			pbHard = this.FindControl<ProgressBar>("pbHard");
		}
	}
}
