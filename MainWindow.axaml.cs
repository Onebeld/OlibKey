using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace OlibKey
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
	    public ListBox MainWindowListBox;
		private TextBlock _tbMessageStatusBar;
		private TextBlock _tbReady;
		private TabControl _tabItems;

		public MainWindow() => InitializeComponent();

	    private void InitializeComponent()
	    {
		    this.WhenActivated(disposables => { });
		    AvaloniaXamlLoader.Load(this);

		    MainWindowListBox = this.FindControl<ListBox>("listBox");
			_tbMessageStatusBar = this.FindControl<TextBlock>("tbMessageStatusBar");
			_tabItems = this.FindControl<TabControl>("tabItems");
			_tbReady = this.FindControl<TextBlock>("tbReady");

			Closing += App.MainWindowViewModel.ProgramClosing;
			Opened += (s, e) => App.MainWindowViewModel.Loading(this);

			_tabItems.SelectionChanged += App.MainWindowViewModel.TabItemsSelectionChanged;
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