using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;

namespace OlibKey
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
	    public ListBox MainWindowListBox;
		private TextBlock _tbMessageStatusBar;
		private TextBlock _tbReady;

	    public MainWindow() => InitializeComponent();

	    private void InitializeComponent()
	    {
		    this.WhenActivated(disposables => { });
		    AvaloniaXamlLoader.Load(this);

		    MainWindowListBox = this.FindControl<ListBox>("listBox");
			_tbMessageStatusBar = this.FindControl<TextBlock>("tbMessageStatusBar");
			_tbReady = this.FindControl<TextBlock>("tbReady");
			Closing += App.MainWindowViewModel.ProgramClosing;
			Opened += MainWindow_Initialized;
	    }

		private void MainWindow_Initialized(object sender, System.EventArgs e)
		{
			App.MainWindowViewModel.Loading(this);
		}

		public async void MessageStatusBar(string message)
		{
			_tbMessageStatusBar.IsVisible = true;
			_tbReady.IsVisible = false;
			_tbMessageStatusBar.Text = (string)Application.Current.FindResource(message);
			await Task.Delay(3000);
			_tbMessageStatusBar.IsVisible = false;
			_tbReady.IsVisible = true;
		}
	}
}