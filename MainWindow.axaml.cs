using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace OlibKey
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
	    public ListBox MainWindowListBox;

	    public MainWindow() => InitializeComponent();

	    private void InitializeComponent()
	    {
		    this.WhenActivated(disposables => { });
		    AvaloniaXamlLoader.Load(this);

		    MainWindowListBox = this.FindControl<ListBox>("listBox");
		    Closing += App.MainWindowViewModel.ProgramClosing;
			Opened += MainWindow_Initialized;
	    }

		private void MainWindow_Initialized(object sender, System.EventArgs e)
		{
			App.MainWindowViewModel.Loading(this);
		}
	}
}