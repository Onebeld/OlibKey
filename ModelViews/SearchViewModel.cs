using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using OlibKey.AccountStructures;
using OlibKey.Controls;
using OlibKey.Utilities;
using OlibKey.Views;

namespace OlibKey.ModelViews
{
    public class SearchViewModel : BaseViewModel
    {
        private ObservableCollection<AccountListItem> list = new ObservableCollection<AccountListItem>();
        private ObservableCollection<FolderListItem> folderList = new ObservableCollection<FolderListItem>();
        public string _searchText;
        private int _selectedIndex;

        #region Commands
        public ICommand CreateFolderCommand { get; set; }
        public ICommand DeleteFolderCommand { get; set; }
        public ICommand EditFolderCommand { get; set; }
        #endregion

        #region PublicProperty
        public ObservableCollection<AccountListItem> AccountsList
        {
            get => list; set => RaisePropertyChanged(ref list, value);
        }
        public ObservableCollection<FolderListItem> FolderList
        {
            get => folderList; set => RaisePropertyChanged(ref folderList, value);
        }
        public int SelectedIndex
        {
            get => _selectedIndex; set => RaisePropertyChanged(ref _selectedIndex, value);
        }
        public string SearchText
        {
            get => _searchText; set => RaisePropertyChanged(ref _searchText, value);
        }

        public void AddAccount(AccountListItem account) => AccountsList.Add(account);

        public void RemoveSelectedAccount() => AccountsList.RemoveAt(SelectedIndex);

        public void ClearAccountsList() => AccountsList.Clear();
        public FolderListItem SelectedFolderItem { get { try { return FolderList[SelectedIndex]; } catch { return null; } } }


        #endregion
        public SearchViewModel() => SetupCommandBindings();

        private void SetupCommandBindings()
        {
            CreateFolderCommand = new Command(CreateFolder);
            DeleteFolderCommand = new Command(DeleteFolder);
            EditFolderCommand = new Command(EditFolder);
        }

        private void CreateFolder()
        {
            CreateFolderWindow window = new CreateFolderWindow();
            if ((bool)window.ShowDialog())
            {
                CustomFolder folder = new CustomFolder
                {
                    Name = window.tbNameFolder.Text,
                    ID = Guid.NewGuid().ToString("N")
                };
                FolderListItem ali = new FolderListItem
                {
                    DataContext = folder
                };

                FolderList.Add(ali);
            }
        }

        public void AddFolder(CustomFolder accountContent)
        {
            FolderListItem ali = new FolderListItem
            {
                DataContext = accountContent
            };

            FolderList.Add(ali);
        }

        public void DeleteFolder() => FolderList.Remove(SelectedFolderItem);

        public void EditFolder()
        {
            if (SelectedFolderItem != null)
            {
                CreateFolderWindow window = new CreateFolderWindow((CustomFolder)SelectedFolderItem.DataContext);
                window.tbNameFolder.Text = SelectedFolderItem.FolderContext.Name;
                window.ShowDialog();
            }
        }

        public void CloseSearch()
        {
            App.MainWindow.Model.DatabaseApplication.CustomFolders = FolderList.Select(item => item.DataContext as CustomFolder).ToList();
            App.MainWindow.Model.SearchWindow.Close();
        }
    }
}
