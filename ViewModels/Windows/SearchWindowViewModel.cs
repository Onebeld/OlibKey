using OlibKey.Structures;
using OlibKey.Views.Controls;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

namespace OlibKey.ViewModels.Windows
{
	public class SearchWindowViewModel : ReactiveObject
	{
		private ObservableCollection<AccountListItem> list = new ObservableCollection<AccountListItem>();
		private ObservableCollection<CustomFolderListItem> folderList = new ObservableCollection<CustomFolderListItem>();

		public string _searchText;
		private int _selectedAccountIndex;
		private int _selectedFolderIndex;

		public ReactiveCommand<Unit, Unit> CreateFolderCommand { get; }
		public ReactiveCommand<Unit, Unit> DeleteFolderCommand { get; }
		public ReactiveCommand<Unit, Unit> EditFolderCommand { get; }
		public ReactiveCommand<Unit, Unit> UnselectFolderItemCommand { get; }

		#region PublicProperty
		public ObservableCollection<AccountListItem> AccountsList
		{
			get => list; set => this.RaiseAndSetIfChanged(ref list, value);
		}
		public ObservableCollection<CustomFolderListItem> FolderList
		{
			get => folderList; set => this.RaiseAndSetIfChanged(ref folderList, value);
		}
		public int SelectedFolderIndex
		{
			get => _selectedFolderIndex; set => this.RaiseAndSetIfChanged(ref _selectedFolderIndex, value);
		}
		public int SelectedAccountIndex
		{
			get => _selectedAccountIndex; 
			set
			{
				this.RaiseAndSetIfChanged(ref _selectedAccountIndex, value);
				if (SelectedAccountIndex == -1) return;
				App.MainWindowViewModel.SearchSelectAccount(SelectedAccountItem);
				App.SearchWindow.Close();
			}
		}
		public string SearchText
		{
			get => _searchText; set => this.RaiseAndSetIfChanged(ref _searchText, value);
		}
		public SearchWindowViewModel()
		{
			CreateFolderCommand = ReactiveCommand.Create(CreateFolder);
			DeleteFolderCommand = ReactiveCommand.Create(() => { FolderList.Remove(SelectedFolderItem); });
			EditFolderCommand = ReactiveCommand.Create(() => SelectedFolderItem.Focusing());
			UnselectFolderItemCommand = ReactiveCommand.Create(() => { SelectedFolderIndex = -1; });

			SelectedAccountIndex = -1;
			SelectedFolderIndex = -1;
		}
		public void AddAccount(AccountListItem account) => AccountsList.Add(account);

		public void ClearAccountsList() => AccountsList.Clear();
		public CustomFolderListItem SelectedFolderItem { get { try { return FolderList[SelectedFolderIndex]; } catch { return null; } } }
		public AccountListItem SelectedAccountItem { get { try { return AccountsList[SelectedAccountIndex]; } catch { return null; } } }
		#endregion
		public void CreateFolder()
		{
			var a = new CustomFolderListItem
			{
				DataContext = new CustomFolder()
			};
			a.FolderContext.ID = Guid.NewGuid().ToString("N");
			FolderList.Add(a);
		}
		public void AddFolder(CustomFolder accountContent)
		{
			var ali = new CustomFolderListItem
			{
				DataContext = accountContent,
			};
			ali._tbName.IsVisible = false;

			FolderList.Add(ali);
		}
	}
}
