using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.Structures;
using OlibKey.ViewModels.Windows;
using OlibKey.Views.Controls;
using System.Threading.Tasks;
using System.Linq;

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
			_tbSearchText.GetObservable(TextBox.TextProperty).Subscribe(value => SearchAccount());
		}

		private void SearchWindow_Closed(object sender, EventArgs e) => App.Database.CustomFolders = SearchViewModel.FolderList.Select(item => item.DataContext as CustomFolder).ToList();

		public async void SearchAccount()
		{
			SearchViewModel.ClearAccountsList();
			await Task.Delay(10);

			foreach (var i in App.MainWindowViewModel.AccountsList)
			{
				var account = i.AccountItem;
				if ((bool)_rLogin.IsChecked && account.TypeAccount == 0)
				{
					if (SearchViewModel.SelectedFolderItem == null)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
					else if (account.IDFolder == ((CustomFolder)SearchViewModel.SelectedFolderItem.DataContext).ID)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
				}
				else if ((bool)_rBankCard.IsChecked && account.TypeAccount == 1)
				{
					if (SearchViewModel.SelectedFolderItem == null)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
					else if (account.IDFolder == ((CustomFolder)SearchViewModel.SelectedFolderItem.DataContext).ID)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
				}
				else if ((bool)_rPassport.IsChecked && account.TypeAccount == 2)
				{
					if (SearchViewModel.SelectedFolderItem == null)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
					else if (account.IDFolder == ((CustomFolder)SearchViewModel.SelectedFolderItem.DataContext).ID)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
				}
				else if ((bool)_rReminder.IsChecked && account.TypeAccount == 3)
				{
					if (SearchViewModel.SelectedFolderItem == null)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
					else if (account.IDFolder == ((CustomFolder)SearchViewModel.SelectedFolderItem.DataContext).ID)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
				}
				else if ((bool)_rAll.IsChecked)
				{
					if (SearchViewModel.SelectedFolderItem == null)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
					else if (account.IDFolder == ((CustomFolder)SearchViewModel.SelectedFolderItem.DataContext).ID)
						if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
						{
							if (account.AccountName.ToLower().Contains(SearchViewModel.SearchText.ToLower())) Add(account, i._image, i.IDElement);
						}
						else Add(account, i._image, i.IDElement);
				}
			}
		}

		private void Add(Account a, Image i, string id)
		{
			var ali = new AccountListItem(a);
			ali.IDElement = id;
			ali._image.Source = i.Source;
			AddAccount(ali);
		}

		private void lbFolders_SelectionChanged(object sender, SelectionChangedEventArgs e) => SearchAccount();
		private void rLogin_Click(object sender, RoutedEventArgs e) => SearchAccount();

		public void AddAccount(AccountListItem account) => SearchViewModel.AddAccount(account);
	}
}
