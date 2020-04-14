using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OlibKey.Controls;
using OlibKey.Utilities;

namespace OlibKey.ModelViews
{
    public class SearchViewModel : BaseViewModel
    {
        private ObservableCollection<AccountListItem> list = new ObservableCollection<AccountListItem>();
        private int _selectedIndex;

        #region Commands

        #endregion

        #region PublicProperty
        public ObservableCollection<AccountListItem> AccountsList
        {
            get => list; set => RaisePropertyChanged(ref list, value);
        }
        public int SelectedIndex
        {
            get => _selectedIndex; set => RaisePropertyChanged(ref _selectedIndex, value);
        }

        public void AddAccount(AccountListItem account)
        {
            AccountsList.Add(account);
        }

        public void RemoveSelectedAccount()
        {
            AccountsList.RemoveAt(SelectedIndex);
        }



        #endregion
        public SearchViewModel()
        {
            SetupCommandBindings();
        }

        private void SetupCommandBindings()
        {

        }
    }
}
