using OlibKey.AccountStructures;
using OlibKey.Controls;
using OlibKey.Core;
using OlibKey.Utilities;
using OlibKey.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OlibKey.ModelViews
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<AccountListItem> list = new ObservableCollection<AccountListItem>();
        private int _selectedIndex;
        private string _nameStorage;

        #region Pages
        public PasswordInformationPage PasswordInformationPage { get; set; }
        public CreatePasswordPage CreatePasswordPage { get; set; }
        public CreatePasswordStorageWindow CreatePasswordStorageWindow { get; set; }
        public static StartPage StartPage { get; set; }
        #endregion
        #region Commands
        public ICommand NewPasswordStorage { get; set; }
        public ICommand NewCreatePassword { get; set; }
        #endregion

        public MainViewModel()
        {
            SetupCommandBindings();
            StartPage = new StartPage();
        }

        #region PublicProperty
        public ObservableCollection<AccountListItem> AccountsList
        {
            get => list;
            set => RaisePropertyChanged(ref list, value);
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set { RaisePropertyChanged(ref _selectedIndex, value); UpdateSelectedItem(); }
        }
        public string NameStorage
        {
            get => _nameStorage;
            set => RaisePropertyChanged(ref _nameStorage, value);
        }

        public static string PathStorage { get; set; }
        public static string MasterPassword { get; set; }
        #endregion

        public AccountListItem SelectedAccountItem { get { try { return AccountsList[SelectedIndex]; } catch { return null; } } }

        private AccountModel _selectedAccStr = new AccountModel();
        public AccountModel SelectedAccountStructure
        {
            get => _selectedAccStr;
            set => RaisePropertyChanged(ref _selectedAccStr, value);
        }

        private void UpdateSelectedItem()
        {
            if (SelectedAccountItem != null && SelectedAccountItem.DataContext != null)
            {
                SelectedAccountStructure = SelectedAccountItem.DataContext as AccountModel;
            }
        }

        private void SetupCommandBindings()
        {
            NewPasswordStorage = new Command(NewPasswordStorageVoid);
            NewCreatePassword = new Command(NewCreatePasswordVoid);
        }

        public void NewPasswordStorageVoid()
        {
            CreatePasswordStorageWindow = new CreatePasswordStorageWindow();
            if ((bool)CreatePasswordStorageWindow.ShowDialog())
            {

            }
        }

        public void NewCreatePasswordVoid()
        {
            CreatePasswordPage = new CreatePasswordPage
            {
                AddAccountCallback = AddAccount
            };
            App.MainWindow.frame.NavigationService.Navigate(CreatePasswordPage);
        }

        public void AddAccount() { AddAccount(CreatePasswordPage.AccountModel); }
        public void AddAccount(AccountModel accountContent)
        {
            //e
            AccountListItem ali = new AccountListItem
            {
                DataContext = accountContent,
                ShowContentCallback = ShowAccountContent,
                EditContentCallback = ShowEditAccountWindow
            };

            App.MainWindow.frame.Navigate(StartPage);

            AccountsList.Add(ali);
        }

        public void DeleteAccount()
        {
            AccountsList.Remove(SelectedAccountItem);
        }

        public void ShowEditAccountWindow(AccountModel account)
        {
            if (account != null)
            {
                SelectedAccountStructure = account;
            }
        }

        public void ShowEditAccountWindow()
        {
            UpdateSelectedItem();
        }

        public void ShowAccountContent(AccountModel account)
        {
            if (account != null)
            {
                PasswordInformationPage = new PasswordInformationPage(account);
                PasswordInformationPage.DeletedAccount = DeleteAccount;
                PasswordInformationPage.ChangedAccount = ShowEditAccountWindow;
                App.MainWindow.frame.Navigate(PasswordInformationPage);
                SelectedAccountStructure = account;
            }
        }

        public void LoadAccounts()
        {
            ClearAccountsList();
            foreach (AccountModel accounts in SaveAndLoadAccount.LoadFiles(PathStorage))
            {
                AddAccount(accounts);
            }
        }
        public void ClearAccountsList()
        {
            SelectedIndex = 0;
            AccountsList.Clear();
        }
    }
}
