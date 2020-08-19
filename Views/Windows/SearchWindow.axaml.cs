using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.Structures;
using OlibKey.ViewModels.Windows;
using OlibKey.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;

namespace OlibKey.Views.Windows
{
	public class SearchWindow : ReactiveWindow<SearchWindowViewModel>
	{
		public SearchWindowViewModel SearchViewModel { get; set; }

		private TextBox _tbSearchText;

		private RadioButton _rLogin;
		private RadioButton _rBankCard;
		private RadioButton _rPassport;
		private RadioButton _rReminder;
		private RadioButton _rNotes;
		private RadioButton _rAll;

		private ToggleButton _tbSortAlphabetically;
		private ToggleButton _tbFavorite;
		private ToggleButton _tbActiveReminder;

		private ListBox _lbFolders;

		public SearchWindow()
		{
			AvaloniaXamlLoader.Load(this);
			InitializeComponent();
		}

		private async void InitializeComponent()
		{
			_rLogin = this.FindControl<RadioButton>("rLogin");
			_rBankCard = this.FindControl<RadioButton>("rBankCard");
			_rPassport = this.FindControl<RadioButton>("rPassport");
			_rReminder = this.FindControl<RadioButton>("rReminder");
			_rNotes = this.FindControl<RadioButton>("rNotes");
			_rAll = this.FindControl<RadioButton>("rAll");
			_tbSearchText = this.FindControl<TextBox>("tbSearchText");
			_lbFolders = this.FindControl<ListBox>("lbFolders");
			_tbSortAlphabetically = this.FindControl<ToggleButton>("tbSortAlphabetically");
			_tbFavorite = this.FindControl<ToggleButton>("tbFavorite");
			_tbActiveReminder = this.FindControl<ToggleButton>("tbActiveReminder");

			DataContext = SearchViewModel = new SearchWindowViewModel();
			_rAll.IsChecked = true;
			_lbFolders.PointerPressed += (s, e) => ViewModel.SelectedFolderIndex = -1;
			Closed += SearchWindow_Closed;
			await Task.Delay(50);
			_ = _tbSearchText.GetObservable(TextBox.TextProperty).Subscribe(value => DeleteListAndSearchElement());
		}

		private void SearchWindow_Closed(object sender, EventArgs e)
		{
			App.MainWindowViewModel.SelectedTabItem.ViewModel.Database.Folders =
				SearchViewModel.FolderList.Select(item => item.DataContext as Folder).ToList();
		}

		private async void DeleteListAndSearchElement()
		{
			SearchViewModel.ClearLoginsList();
			await Task.Delay(10);

			List<LoginListItem> selectedItemList;

			if (_rLogin.IsChecked != null && (bool)_rLogin.IsChecked)
				selectedItemList = App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList.ToList()
					.FindAll(x => x.LoginItem.Type == 0);
			else if (_rBankCard.IsChecked != null && (bool)_rBankCard.IsChecked)
				selectedItemList = App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList.ToList()
					.FindAll(x => x.LoginItem.Type == 1);
			else if (_rPassport.IsChecked != null && (bool)_rPassport.IsChecked)
				selectedItemList = App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList.ToList()
					.FindAll(x => x.LoginItem.Type == 2);
			else if (_rReminder.IsChecked != null && (bool)_rReminder.IsChecked)
				selectedItemList = App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList.ToList()
					.FindAll(x => x.LoginItem.Type == 3);
			else if (_rNotes.IsChecked != null && (bool)_rNotes.IsChecked)
				selectedItemList = App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList.ToList()
					.FindAll(x => x.LoginItem.Type == 4);
			else selectedItemList = App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList.ToList();

			if (SearchViewModel.SelectedFolderItem != null)
				selectedItemList = selectedItemList.FindAll(x =>
					x.LoginItem.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext)?.ID);

			if (_tbActiveReminder.IsChecked != null && (bool)_tbActiveReminder.IsChecked)
				selectedItemList = selectedItemList.FindAll(x => x.LoginItem.IsReminderActive);

			if (_tbFavorite.IsChecked != null && (bool)_tbFavorite.IsChecked)
				selectedItemList = selectedItemList.FindAll(x => x.LoginItem.Favorite);

			if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
				selectedItemList = selectedItemList.FindAll(x =>
					x.LoginItem.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower()));

			if (_tbSortAlphabetically.IsChecked != null && (bool)_tbSortAlphabetically.IsChecked)
				selectedItemList = selectedItemList.OrderBy(x => x.LoginItem.Name).ToList();

			foreach (LoginListItem item in selectedItemList) Add(item.LoginItem, item.IconLogin, item.LoginID);
		}

		private void Add(Login a, Image i, string id) =>
			AddLogin(new LoginListItem(a)
			{
				LoginID = id,
				IconLogin = { Source = i.Source },
				IsFavorite = { IsEnabled = false}
			});

		private void lbFolders_SelectionChanged(object sender, SelectionChangedEventArgs e) => DeleteListAndSearchElement();
		private void rLogin_Click(object sender, RoutedEventArgs e) => DeleteListAndSearchElement();

		private void AddLogin(LoginListItem login) => SearchViewModel.AddLogin(login);
	}
}
