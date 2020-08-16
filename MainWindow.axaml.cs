using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace OlibKey
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
	    private TextBlock _tbMessageStatusBar;
		private TextBlock _tbReady;
		private TabControl _tabItems;

		public MainWindow() => InitializeComponent();

	    private void InitializeComponent()
	    {
		    this.WhenActivated(disposables => { });
		    AvaloniaXamlLoader.Load(this);
		    
			_tbMessageStatusBar = this.FindControl<TextBlock>("tbMessageStatusBar");
			_tabItems = this.FindControl<TabControl>("tabItems");
			_tbReady = this.FindControl<TextBlock>("tbReady");

			Closing += App.MainWindowViewModel.ProgramClosing;
			Opened += (s, e) => App.MainWindowViewModel.Loading(this);

			_tabItems.SelectionChanged += App.MainWindowViewModel.TabItemsSelectionChanged;

			Activated += (s, e) => { App.Autoblock.Stop(); };
			Deactivated += (s, e) => { if (App.Settings.AutoblockEnabled) App.Autoblock.Start(); };

			SetupDnd();
		}

		private void SetupDnd()
		{
			static void DragOver(object sender, DragEventArgs e)
			{
				e.DragEffects &= (DragDropEffects.Copy | DragDropEffects.Link);

				if (!e.Data.Contains(DataFormats.FileNames)) e.DragEffects = DragDropEffects.None;
			}
			static void Drop(object sender, DragEventArgs e)
			{
				if (e.Data.Contains(DataFormats.FileNames)) App.MainWindowViewModel.OpenStorageDnD(e.Data.GetFileNames().ToList());
			}

			AddHandler(DragDrop.DropEvent, Drop);
			AddHandler(DragDrop.DragOverEvent, DragOver);
		}

		public async void MessageStatusBar(string message)
		{
			_tbMessageStatusBar.IsVisible = true;
			_tbMessageStatusBar.Text = message;
			_tbReady.IsVisible = false;
			await Task.Delay(App.Settings.MessageDuration * 1000);
			_tbMessageStatusBar.IsVisible = false;
			_tbReady.IsVisible = true;
		}
	}
}