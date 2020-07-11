using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using OlibKey.Structures;
using OlibKey.Views.Controls;

namespace OlibKey.ViewModels.Windows
{
	public class SearchWindowViewModel : ReactiveObject
	{
		private ObservableCollection<LoginListItem> _loginList = new ObservableCollection<LoginListItem>();
		private ObservableCollection<CustomFolderListItem> _folderList = new ObservableCollection<CustomFolderListItem>();

		private string _searchText;

		private int _selectedLoginIndex;
		private int _selectedFolderIndex;

		#region ReactiveCommand's

		public ReactiveCommand<Unit, Unit> CreateFolderCommand { get; }
		public ReactiveCommand<Unit, Unit> DeleteFolderCommand { get; }
		public ReactiveCommand<Unit, Unit> EditFolderCommand { get; }
		public ReactiveCommand<Unit, Unit> UnselectFolderItemCommand { get; }

		#endregion

		#region Property's

		private ObservableCollection<LoginListItem> LoginList
		{
			get => _loginList; set => this.RaiseAndSetIfChanged(ref _loginList, value);
		}
		public ObservableCollection<CustomFolderListItem> FolderList
		{
			get => _folderList; set => this.RaiseAndSetIfChanged(ref _folderList, value);
		}
		public int SelectedFolderIndex
		{
			get => _selectedFolderIndex; set => this.RaiseAndSetIfChanged(ref _selectedFolderIndex, value);
		}
		private int SelectedLoginIndex
		{
			get => _selectedLoginIndex; 
			set
			{
				this.RaiseAndSetIfChanged(ref _selectedLoginIndex, value);
				if (SelectedLoginIndex == -1) return;
				App.MainWindowViewModel.SearchSelectLogin(SelectedLoginItem);
				App.SearchWindow.Close();
			}
		}
		public string SearchText
		{
			get => _searchText; set => this.RaiseAndSetIfChanged(ref _searchText, value);
		}

		#endregion

		public SearchWindowViewModel()
		{
			CreateFolderCommand = ReactiveCommand.Create(CreateFolder);
			DeleteFolderCommand = ReactiveCommand.Create(() => { FolderList.Remove(SelectedFolderItem); });
			EditFolderCommand = ReactiveCommand.Create(() => SelectedFolderItem.Focusing());
			UnselectFolderItemCommand = ReactiveCommand.Create(() => { SelectedFolderIndex = -1; });

			SelectedLoginIndex = -1;
			SelectedFolderIndex = -1;
		}
		public void AddLogin(LoginListItem Login) => LoginList.Add(Login);

		public void ClearLoginsList() => LoginList.Clear();
		public CustomFolderListItem SelectedFolderItem { get { try { return FolderList[SelectedFolderIndex]; } catch { return null; } } }
		public LoginListItem SelectedLoginItem { get { try { return LoginList[SelectedLoginIndex]; } catch { return null; } } }
		private void CreateFolder()
		{
			var a = new CustomFolderListItem
			{
				DataContext = new CustomFolder()
			};
			a.FolderContext.ID = Guid.NewGuid().ToString("N");
			FolderList.Add(a);
		}
		public void AddFolder(CustomFolder LoginContent)
		{
			var ali = new CustomFolderListItem
			{
				DataContext = LoginContent,
			};
			ali._tbName.IsVisible = false;

			FolderList.Add(ali);
		}
	}
}
