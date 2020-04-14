using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OlibKey.Controls;
using OlibKey.Utilities;

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

        public void AddAccount(AccountListItem account)
        {
            AccountsList.Add(account);
        }

        public void RemoveSelectedAccount()
        {
            AccountsList.RemoveAt(SelectedIndex);
        }

        public void ClearAccountsList()
        {
            SelectedIndex = 0;
            AccountsList.Clear();
        }



        #endregion
        public SearchViewModel()
        {
            SetupCommandBindings();
        }

        private void SetupCommandBindings()
        {
            CreateFolderCommand = new Command(CreateFolder);
        }

        private void CreateFolder()
        {

        }
    }
}
