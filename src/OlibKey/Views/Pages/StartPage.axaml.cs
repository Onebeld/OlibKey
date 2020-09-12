using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.ViewModels.Pages;

namespace OlibKey.Views.Pages
{
	public class StartPage : ReactiveUserControl<StartPageViewModel>
	{
		public StartPage() => InitializeComponent();
		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
	}
}
