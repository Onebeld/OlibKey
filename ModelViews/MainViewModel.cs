using OlibKey.AccountStructures;
using OlibKey.Controls;
using OlibKey.Core;
using OlibKey.Utilities;
using OlibKey.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OlibKey.ModelViews
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<AccountListItem> list = new ObservableCollection<AccountListItem>();
        public Database DatabaseApplication;
        private int _selectedIndex;
        private string _nameStorage;
        private bool _isUnlockStorage;
        private bool _isLockStorage;

        #region Pages
        public PasswordInformationPage PasswordInformationPage { get; set; }
        public CreatePasswordPage CreatePasswordPage { get; set; }
        public CreatePasswordStorageWindow CreatePasswordStorageWindow { get; set; }
        public static StartPage StartPage { get; set; }
        #endregion
        #region Windows
        public SearchWindow SearchWindow { get; set; }
        #endregion
        #region Commands
        public ICommand ChangeMasterPassword { get; set; }
        public ICommand SettingsWindowCommand { get; set; }
        public ICommand ShowWindow { get; set; }
        public ICommand AboutWindowCommand { get; set; }
        public ICommand NewPasswordStorage { get; set; }
        public ICommand PasswordGenerator { get; set; }
        public ICommand NewCreatePassword { get; set; }
        public ICommand ExitProgram { get; set; }
        public ICommand OpenStorage { get; set; }
        public ICommand SaveStorage { get; set; }
        public ICommand BlockingStorage { get; set; }
        public ICommand UnblockingStorage { get; set; }
        public ICommand CheckUpdate { get; set; }
        public ICommand ShowSearchWindow { get; set; }
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

        public bool IsUnlockStorage
        {
            get => _isUnlockStorage;
            set => RaisePropertyChanged(ref _isUnlockStorage, value);
        }

        public bool IsLockStorage
        {
            get => _isLockStorage;
            set => RaisePropertyChanged(ref _isLockStorage, value);
        }

        public static string PathStorage { get; set; }
        public static string MasterPassword { get; set; }
        #endregion

        public AccountListItem SelectedAccountItem { get { try { return AccountsList[SelectedIndex]; } catch { return null; } } }

        private Account _selectedAccStr = new Account();
        public Account SelectedAccountStructure
        {
            get => _selectedAccStr;
            set => RaisePropertyChanged(ref _selectedAccStr, value);
        }

        private void UpdateSelectedItem()
        {
            if (SelectedAccountItem?.DataContext != null)
                SelectedAccountStructure = SelectedAccountItem.DataContext as Account;
        }

        private void AboutWindowVoid()
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void ChangeMasterPasswordVoid()
        {
            ChangeMasterPasswordWindow passwordWindow = new ChangeMasterPasswordWindow();
            passwordWindow.ShowDialog();
        }

        public void MoveUp() => MoveItem(-1);

        public void MoveDown() => MoveItem(1);

        private void MoveItem(int direction)
        {
            if (SelectedAccountItem == null)
                return;

            int newIndex = SelectedIndex + direction;

            if (newIndex < 0 || newIndex >= AccountsList.Count)
                return;

            AccountListItem selected = SelectedAccountItem;

            AccountsList.Remove(selected);
            AccountsList.Insert(newIndex, selected);
            SelectedIndex = newIndex;
        }

        private void SetupCommandBindings()
        {
            NewPasswordStorage = new Command(NewPasswordStorageVoid);
            NewCreatePassword = new Command(NewCreatePasswordVoid);
            ExitProgram = new Command(ExitProgramVoid);
            OpenStorage = new Command(OpenStorageVoid);
            SaveStorage = new Command(SaveAccount);
            BlockingStorage = new Command(BlockingStorageVoid);
            UnblockingStorage = new Command(RequireMasterPasswordVoid);
            CheckUpdate = new Command(CheckUpdateVoid);
            PasswordGenerator = new Command(PasswordGeneratorVoid);
            AboutWindowCommand = new Command(AboutWindowVoid);
            SettingsWindowCommand = new Command(SettingsWindowVoid);
            ShowWindow = new Command(ShowWindowVoid);
            ChangeMasterPassword = new Command(ChangeMasterPasswordVoid);
            ShowSearchWindow = new Command(ShowSearcWindowVoid);
        }

        private void SettingsWindowVoid()
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        public void ExitProgramVoid()
        {
            SaveAccount();
            if (App.Setting.CollapseWhenClosing)
                Application.Current.MainWindow?.Hide();
            else
                Application.Current.Shutdown();
        }

        public void ShowWindowVoid()
        {
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Topmost = true;
            Application.Current.MainWindow.Topmost = false;
            App.MainWindow.mainWindow.Opacity = 1;
            App.MainWindow.ScaleWindow.ScaleX = 1;
            App.MainWindow.ScaleWindow.CenterY = 1;
        }

        public void NewPasswordStorageVoid()
        {
            CreatePasswordStorageWindow = new CreatePasswordStorageWindow();
            if (!(bool) CreatePasswordStorageWindow.ShowDialog()) return;
            DatabaseApplication = new Database();
            NameStorage = Path.GetFileNameWithoutExtension(CreatePasswordStorageWindow.TxtPathSelection.Text);
            IsUnlockStorage = true;
            IsLockStorage = false;
            ClearAccountsList();
            App.MainWindow.Notification((string)Application.Current.FindResource("Not3"));
        }

        public void NewCreatePasswordVoid()
        {
            CreatePasswordPage = new CreatePasswordPage
            {
                AddAccountCallback = AddAccount
            };
            App.MainWindow.frame.NavigationService.Navigate(CreatePasswordPage);
        }
        public void RequireMasterPasswordVoid()
        {
            RequireMasterPasswordWindow requireMaster = new RequireMasterPasswordWindow
            {
                LoadStorageCallback = LoadAccounts
            };
            requireMaster.ShowDialog();
        }

        public void PasswordGeneratorVoid()
        {
            PasswordGeneratorWindow generatorWindow = new PasswordGeneratorWindow();
            generatorWindow.ShowDialog();
        }
        public void OpenStorageVoid()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Olib-files (*.olib)|*.olib"
            };

            if (!(bool) openFileDialog.ShowDialog()) return;

            BlockingStorageVoid();

            PathStorage = openFileDialog.FileName;
            NameStorage = Path.GetFileNameWithoutExtension(openFileDialog.FileName);


            RequireMasterPasswordWindow requireMaster = new RequireMasterPasswordWindow
            {
                LoadStorageCallback = LoadAccounts
            };
            requireMaster.ShowDialog();
        }

        public void BlockingStorageVoid()
        {
            SaveAccount();
            ClearAccountsList();
            StartPage page = new StartPage();
            App.MainWindow.frame.NavigationService.Navigate(page);
            IsLockStorage = true;
            IsUnlockStorage = false;
        }

        public void CheckUpdateVoid() => App.MainWindow.CheckUpdate(true);

        public void AddAccount() { AddAccount(CreatePasswordPage.AccountModel); }
        public void AddAccount(Account accountContent)
        {
            AccountListItem ali = new AccountListItem
            {
                DataContext = accountContent,
                ShowContentCallback = ShowAccountContent,
                EditContentCallback = ShowEditAccountWindow
            };
            StartPage startPage = new StartPage();
            App.MainWindow.frame.Navigate(startPage);

            AccountsList.Add(ali);
        }

        public void DeleteAccount() => AccountsList.Remove(SelectedAccountItem);

        public void ShowEditAccountWindow(Account account)
        {
            if (account != null) SelectedAccountStructure = account;
        }

        public void ShowEditAccountWindow() => UpdateSelectedItem();

        public void ShowAccountContent(Account account)
        {
            if (account == null) return;
            PasswordInformationPage = new PasswordInformationPage(account)
            {
                DeletedAccount = DeleteAccount,
                ChangedAccount = ShowEditAccountWindow
            };
            App.MainWindow.frame.Navigate(PasswordInformationPage);
            SelectedAccountStructure = account;
        }

        public void LoadAccounts()
        {
            DatabaseApplication = SaveAndLoadAccount.LoadFiles(PathStorage, MasterPassword);
            ClearAccountsList();
            foreach (Account accounts in DatabaseApplication.Accounts) AddAccount(accounts);
            IsUnlockStorage = true;
            IsLockStorage = false;
        }

        public void SaveAccount()
        {
            if (!IsUnlockStorage) return;
            if (PathStorage != null)
            {
                DatabaseApplication.Accounts = AccountsList.Select(item => item.DataContext as Account).ToList();

                SaveAndLoadAccount.SaveFiles(DatabaseApplication, PathStorage);

                App.Setting.PathStorage = PathStorage;
            }

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OlibKey"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OlibKey");

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OlibKey\\settings.json", JsonSerializer.Serialize(App.Setting));

            App.MainWindow.IconSave();
        }
        public void ClearAccountsList()
        {
            SelectedIndex = 0;
            AccountsList.Clear();
        }

        public void ShowSearcWindowVoid()
        {
            SearchWindow = new SearchWindow();
            foreach (CustomFolder folder in DatabaseApplication.CustomFolders) SearchWindow.SearchModel.AddFolder(folder);
            SearchWindow.ShowDialog();
        }
    }
}