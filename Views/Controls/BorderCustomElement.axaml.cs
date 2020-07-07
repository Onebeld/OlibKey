using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibKey.ViewModels.Controls;

namespace OlibKey.Views.Controls
{
	public class BorderCustomElement : UserControl
	{
		public BorderCustomElementViewModel viewModel;

		public BorderCustomElement()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = viewModel = new BorderCustomElementViewModel(true);
		}
	}
}
