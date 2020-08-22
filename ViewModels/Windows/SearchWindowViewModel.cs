using OlibKey.Structures;
using OlibKey.Views.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace OlibKey.ViewModels.Windows
{
    public class SearchWindowViewModel : ReactiveObject
    {
        private ObservableCollection<LoginListItem> _loginList = new ObservableCollection<LoginListItem>();
        private ObservableCollection<FolderListItem> _folderList = new ObservableCollection<FolderListItem>();

        private string _searchText;

        private int _selectedLoginIndex;
        private int _selectedFolderIndex;

        #region ReactiveCommand's

        private ReactiveCommand<Unit, Unit> CreateFolderCommand { get; }
        private ReactiveCommand<Unit, Unit> DeleteFolderCommand { get; }
        private ReactiveCommand<Unit, Unit> EditFolderCommand { get; }
        private ReactiveCommand<Unit, Unit> UnselectFolderItemCommand { get; }

        #endregion

        #region Property's

        private ObservableCollection<LoginListItem> LoginList
        {
            get => _loginList; set => this.RaiseAndSetIfChanged(ref _loginList, value);
        }
        public ObservableCollection<FolderListItem> FolderList
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
                App.MainWindowViewModel.SelectedTabItem.ViewModel.SearchSelectLogin(SelectedLoginItem);
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
            DeleteFolderCommand = ReactiveCommand.Create(() =>
            {
                if (App.MainWindowViewModel.SelectedTabItem.ViewModel.UseTrash)
                {
                    if (App.MainWindowViewModel.SelectedTabItem.ViewModel.Database.Trash == null)
                    {
                        App.MainWindowViewModel.SelectedTabItem.ViewModel.Database.Trash = new Trash
                        {
                            Logins = new List<Login>(),
                            Folders = new List<Folder>()
                        };
                    }
                    SelectedFolderItem.FolderContext.DeleteDate = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
                    App.MainWindowViewModel.SelectedTabItem.ViewModel.Database.Trash.Folders.Add(SelectedFolderItem.FolderContext);
                }
                FolderList.Remove(SelectedFolderItem);
            });
            EditFolderCommand = ReactiveCommand.Create(() => SelectedFolderItem.Focusing());
            UnselectFolderItemCommand = ReactiveCommand.Create(() => { SelectedFolderIndex = -1; });

            SelectedLoginIndex = SelectedFolderIndex = -1;
        }
        public void AddLogin(LoginListItem login) => LoginList.Add(login);

        public void ClearLoginsList() => LoginList.Clear();
        public FolderListItem SelectedFolderItem { get { try { return FolderList[SelectedFolderIndex]; } catch { return null; } } }
        private LoginListItem SelectedLoginItem { get { try { return LoginList[SelectedLoginIndex]; } catch { return null; } } }
        private void CreateFolder() =>
            FolderList.Add(new FolderListItem
            {
                DataContext = new Folder(),
                FolderContext = { ID = Guid.NewGuid().ToString("N") }
            });
        public void AddFolder(Folder loginContent) => FolderList.Add(new FolderListItem { DataContext = loginContent, _tbName = { IsVisible = false } });
    }
}
