using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.Structures;
using OlibKey.ViewModels.Windows;
using OlibKey.Views.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

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
		private RadioButton _rAll;

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
			_rAll = this.FindControl<RadioButton>("rAll");
			_tbSearchText = this.FindControl<TextBox>("tbSearchText");
			_lbFolders = this.FindControl<ListBox>("lbFolders");

			DataContext = SearchViewModel = new SearchWindowViewModel();
			_rAll.IsChecked = true;
			Closed += SearchWindow_Closed;
			_lbFolders.PointerPressed += (s, e) =>
			{
				SearchViewModel.SelectedFolderIndex = -1;
			};
			await Task.Delay(50);
			_ = _tbSearchText.GetObservable(TextBox.TextProperty).Subscribe(value => SearchLogin());
		}

		private void SearchWindow_Closed(object sender, EventArgs e) => App.MainWindowViewModel.SelectedTabItem.ViewModel.Database.Folders = SearchViewModel.FolderList.Select(item => item.DataContext as Folder).ToList();

		private async void SearchLogin()
		{
			SearchViewModel.ClearLoginsList();
			await Task.Delay(10);

			foreach (LoginListItem i in App.MainWindowViewModel.SelectedTabItem.ViewModel.LoginList)
			{
				Login Login = i.LoginItem;
				if (Login.Name != null)
				{
					if ((bool)_rLogin.IsChecked && Login.Type == 0)
					{
						if (SearchViewModel.SelectedFolderItem == null)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
						else if (Login.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext).ID)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
					}
					else if ((bool)_rBankCard.IsChecked && Login.Type == 1)
					{
						if (SearchViewModel.SelectedFolderItem == null)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
						else if (Login.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext).ID)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
					}
					else if ((bool)_rPassport.IsChecked && Login.Type == 2)
					{
						if (SearchViewModel.SelectedFolderItem == null)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
						else if (Login.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext).ID)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
					}
					else if ((bool)_rReminder.IsChecked && Login.Type == 3)
					{
						if (SearchViewModel.SelectedFolderItem == null)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
						else if (Login.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext).ID)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
					}
					else if ((bool)_rAll.IsChecked)
					{
						if (SearchViewModel.SelectedFolderItem == null)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
						else if (Login.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext).ID)
							if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
							{
								if (Login.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(Login, i.IconLogin, i.LoginID);
							}
							else Add(Login, i.IconLogin, i.LoginID);
					}
				}
			}
		}

		private void Add(Login a, Image i, string id)
		{
			LoginListItem ali = new LoginListItem(a)
			{
				LoginID = id,
				IconLogin = { Source = i.Source }
			};
			AddLogin(ali);
		}

		private void lbFolders_SelectionChanged(object sender, SelectionChangedEventArgs e) => SearchLogin();
		private void rLogin_Click(object sender, RoutedEventArgs e) => SearchLogin();

		private void AddLogin(LoginListItem Login) => SearchViewModel.AddLogin(Login);
	}
}
