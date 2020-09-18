using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using OlibKey.Core;
using OlibKey.Structures;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Windows;
using System;
using System.Collections.Generic;
using Avalonia;
using OlibKey.ViewModels.Controls;

namespace OlibKey
{
    public class MainWindowViewModel : ReactiveObject
    {
        private bool _isUnlockDatabase;
        private bool _isLockDatabase;

        private ChangeMasterPasswordWindow _windowChangeMasterPassword;
        private SettingsWindow _settingsWindow;
        public PasswordGeneratorWindow PasswordGenerator;
        public CheckingWeakPasswordsWindow CheckingWindow;
        public TrashWindow TrashWindow;

        private DatabaseViewModel _selectedTabItem;

        private ObservableCollection<DatabaseViewModel> _tabItems = new ObservableCollection<DatabaseViewModel>();

        #region ReactiveCommand's   

        private ReactiveCommand<Unit, Unit> ExitProgramCommand { get; }
        private ReactiveCommand<Unit, Unit> CreateLoginCommand { get; }
        private ReactiveCommand<Unit, Unit> CreateDatabaseCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveDatabaseCommand { get; }
        private ReactiveCommand<Unit, Unit> UnlockDatabaseCommand { get; }
        private ReactiveCommand<Unit, Unit> LockDatabaseCommand { get; }
        private ReactiveCommand<Unit, Unit> OpenDatabaseCommand { get; }
        private ReactiveCommand<Unit, Unit> ShowSearchWindowCommand { get; }
        private ReactiveCommand<Unit, Unit> ChangeMasterPasswordCommand { get; }
        private ReactiveCommand<Unit, Unit> OpenPasswordGeneratorWindowCommand { get; }
        private ReactiveCommand<Unit, Unit> OpenAboutWindowCommand { get; }
        private ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }
        private ReactiveCommand<Unit, Unit> CheckUpdateCommand { get; }
        private ReactiveCommand<Unit, Unit> LockAllDatabasesCommand { get; }
        private ReactiveCommand<Unit, Unit> SaveAllDatabasesCommand { get; }
        private ReactiveCommand<Unit, Unit> UnlockAllDatabasesCommand { get; }
        private ReactiveCommand<Unit, Unit> OpenCheckingWeakPasswordsWindowCommand { get; }
        private ReactiveCommand<Unit, Unit> ClearGCCommand { get; }
        private ReactiveCommand<Unit, Unit> OpenTrashWindowCommand { get; }

        #endregion

        #region Propertie's

        public ObservableCollection<DatabaseViewModel> TabItems
        {
            get => _tabItems;
            set => this.RaiseAndSetIfChanged(ref _tabItems, value);
        }
        public bool IsUnlockDatabase
        {
            get => _isUnlockDatabase;
            set => this.RaiseAndSetIfChanged(ref _isUnlockDatabase, value);
        }
        private bool IsLockDatabase
        {
            get => _isLockDatabase;
            set => this.RaiseAndSetIfChanged(ref _isLockDatabase, value);
        }
        public DatabaseViewModel SelectedTabItem
        {
            get => _selectedTabItem;
            set => this.RaiseAndSetIfChanged(ref _selectedTabItem, value);
        }
        public string[] OpenStorages { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            ExitProgramCommand = ReactiveCommand.Create(ExitApplication);
            CreateLoginCommand = ReactiveCommand.Create(CreateLogin);
            OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
            CreateDatabaseCommand = ReactiveCommand.Create(CreateDatabase);
            SaveDatabaseCommand = ReactiveCommand.Create(() => SaveDatabase(SelectedTabItem));
            UnlockDatabaseCommand = ReactiveCommand.Create(UnlockDatabase);
            LockDatabaseCommand = ReactiveCommand.Create(LockDatabase);
            OpenDatabaseCommand = ReactiveCommand.Create(OpenDatabase);
            CheckUpdateCommand = ReactiveCommand.Create(CheckUpdate);
            ShowSearchWindowCommand = ReactiveCommand.Create(ShowSearchWindow);
            OpenPasswordGeneratorWindowCommand = ReactiveCommand.Create(() => { new PasswordGeneratorWindow().ShowDialog(App.MainWindow); });
            OpenAboutWindowCommand = ReactiveCommand.Create(() => { new AboutWindow().ShowDialog(App.MainWindow); });
            ChangeMasterPasswordCommand = ReactiveCommand.Create(ChangeMasterPassword);
            LockAllDatabasesCommand = ReactiveCommand.Create(LockAllDatabases);
            SaveAllDatabasesCommand = ReactiveCommand.Create(SaveAllDatabases);
            UnlockAllDatabasesCommand = ReactiveCommand.Create(UnlockAllDatabases);
            OpenCheckingWeakPasswordsWindowCommand = ReactiveCommand.Create(OpenCheckingWeakPasswordsWindow);
            ClearGCCommand = ReactiveCommand.Create(ClearGC);
            OpenTrashWindowCommand = ReactiveCommand.Create(OpenTrashWindow);

            App.Autoblock.Tick += AutoblockStorage;
        }

        public void Loading()
        {
            for (int i = 0; i < OpenStorages.Length; i++)
            {
                string item = OpenStorages[i];
                if (Path.GetExtension(item) == ".olib")
                    CreateTab(Program.Settings.PathDatabase = item, true, false);
            }

            if (OpenStorages.Length > 0)
            {
                OpenStorages = null;
                return;
            }

            if (!string.IsNullOrEmpty(Program.Settings.PathDatabase) && File.Exists(Program.Settings.PathDatabase))
                CreateTab(Program.Settings.PathDatabase, true, false);
        }
        public void OpenStorageDnD(IEnumerable<string> files)
        {
            foreach (string item in files.Where(item => Path.GetExtension(item) == ".olib").Where(item =>
                TabItems.All(i => i.PathDatabase != item)))
                CreateTab(Program.Settings.PathDatabase = item, true, false);

            App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not6"));
        }
        public void ProgramClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            for (int index = 0; index < TabItems.Count; index++) SaveDatabase(TabItems[index]);
        }
        public void SaveDatabase(DatabaseViewModel db)
        {
            if (db == null || db.IsLockDatabase || !db.IsUnlockDatabase) return;
            if (db.PathDatabase != null)
            {
                db.Database.Logins = db.LoginList.Select(item => item.LoginItem).ToList();
                SaveAndLoadDatabase.SaveFiles(db);
                App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not4") + $" {db.Header}");
            }
            for (int i = 0; i < db.Router.NavigationStack.Count - 1; i++)
                db.Router.NavigationStack.Remove(db.Router.NavigationStack[i]);
        }
        public void TabItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedTabItem != null)
            {
                IsUnlockDatabase = SelectedTabItem.IsUnlockDatabase;
                IsLockDatabase = SelectedTabItem.IsLockDatabase;
            }
            else
            {
                IsUnlockDatabase = false;
                IsLockDatabase = false;
            }
        }
        private void CreateTab(string path, bool isLockDatabase, bool isUnlockDatabase)
        {
            DatabaseViewModel db = new DatabaseViewModel
            {
                Database = new Database(),
                IsLockDatabase = isLockDatabase,
                IsUnlockDatabase = isUnlockDatabase,
                PathDatabase = path,
                Header = Path.GetFileNameWithoutExtension(Program.Settings.PathDatabase),
                CloseTabCallback = CloseTab
            };

            TabItems.Add(db);

            if (Equals(SelectedTabItem, db))
            {
                IsLockDatabase = true;
                IsUnlockDatabase = false;
            }

            new RequireMasterPasswordWindow
            {
                LoadStorageCallback = LoadDatabase,
                databaseControl = db,
                tbNameStorage = { Text = db.Header }
            }.ShowDialog(App.MainWindow);
        }
        private void OpenCheckingWeakPasswordsWindow()
        {
            if (SelectedTabItem == null || SelectedTabItem.IsLockDatabase) return;
            CheckingWindow = new CheckingWeakPasswordsWindow();
            CheckingWindow.ShowDialog(App.MainWindow);

            SelectedTabItem.SelectedLoginItem = null;
            SelectedTabItem.Router.Navigate.Execute(new StartPageViewModel());
        }
        private void CloseTab(DatabaseViewModel tab)
        {
            SaveDatabase(tab);
            TabItems.Remove(tab);
        }
        private void AutoblockStorage(object sender, EventArgs e)
        {
            App.Autoblock.Stop();

            App.SearchWindow?.Close();
            _windowChangeMasterPassword?.Close();
            _settingsWindow?.Close();
            PasswordGenerator?.Close();
            CheckingWindow?.Close();
            TrashWindow?.Close();

            for (int index = 0; index < TabItems.Count; index++)
            {
                DatabaseViewModel control = TabItems[index];

                if (control.IsUnlockDatabase)
                {
                    if (control.LoginList.Any(login => login.LoginItem.IsReminderActive)) continue;

                    SaveDatabase(control);
                    control.ClearLoginsList();
                    IsUnlockDatabase = control.IsUnlockDatabase = false;
                    IsLockDatabase = control.IsLockDatabase = true;
                    control.Router.Navigate.Execute(new StartPageViewModel(control));
                }
            }
        }
        private async void UnlockDatabase()
        {
            if (SelectedTabItem == null || SelectedTabItem.IsUnlockDatabase) return;

            await new RequireMasterPasswordWindow
            {
                LoadStorageCallback = LoadDatabase,
                databaseControl = SelectedTabItem,
                tbNameStorage = { Text = SelectedTabItem.Header }
            }.ShowDialog(App.MainWindow);
        }
        private async void UnlockAllDatabases()
        {
            for (int i = 0; i < TabItems.Count; i++)
            {
                DatabaseViewModel item = TabItems[i];
                if (item.IsLockDatabase)
                {
                    await new RequireMasterPasswordWindow
                    {
                        LoadStorageCallback = LoadDatabase,
                        databaseControl = item,
                        tbNameStorage = { Text = item.Header }
                    }.ShowDialog(App.MainWindow);
                }
            }

            App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not8"));
        }
        private void LockDatabase()
        {
            if (SelectedTabItem == null || SelectedTabItem.IsLockDatabase) return;

            SaveDatabase(SelectedTabItem);
            SelectedTabItem.ClearLoginsList();
            IsUnlockDatabase = SelectedTabItem.IsUnlockDatabase = false;
            IsLockDatabase = SelectedTabItem.IsLockDatabase = true;
            SelectedTabItem.Router.Navigate.Execute(new StartPageViewModel(SelectedTabItem));
        }
        private void LockAllDatabases()
        {
            for (int index = 0; index < TabItems.Count; index++)
            {
                DatabaseViewModel control = TabItems[index];

                SaveDatabase(control);

                control.ClearLoginsList();
                IsUnlockDatabase = control.IsUnlockDatabase = false;
                IsLockDatabase = control.IsLockDatabase = true;
                control.Router.Navigate.Execute(new StartPageViewModel(control));
            }

            App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not7"));
        }
        private async void OpenDatabase()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog { AllowMultiple = true };
                dialog.Filters.Add(new FileDialogFilter { Name = (string)Application.Current.FindResource("FileOlib"), Extensions = { "olib" } });
                List<string> files = (await dialog.ShowAsync(App.MainWindow)).ToList();

                if (files.Count == 0) return;

                foreach (string file in files.Where(file =>
                    TabItems.All(i => i.PathDatabase != file)))
                    CreateTab(Program.Settings.PathDatabase = file, true, false);
            }
            catch
            {
                // ignored
            }
        }
        private void CreateLogin()
        {
            if (SelectedTabItem == null || SelectedTabItem.IsLockDatabase) return;

            SelectedTabItem.SelectedLoginItem = null;

            SelectedTabItem.Router.Navigate.Execute(new CreateLoginPageViewModel(SelectedTabItem.Database, SelectedTabItem)
            {
                BackPageCallback = SelectedTabItem.StartPage,
                CreateLoginCallback = SelectedTabItem.AddLogin
            });
        }
        private void LoadDatabase(DatabaseViewModel db)
        {
            db.Database = SaveAndLoadDatabase.LoadFiles(db);
            for (int index = 0; index < db.Database.Logins.Count; index++)
                db.AddLogin(db.Database.Logins[index]);

            db.IsLockDatabase = false;
            db.IsUnlockDatabase = true;

            if (Equals(SelectedTabItem, db))
            {
                IsLockDatabase = false;
                IsUnlockDatabase = true;
            }

            if (Program.Settings.AutoRemoveItemsTrash)
            {
                if (db.Database.Trash != null)
                {
                    for (int i = db.Database.Trash.Logins.Count - 1; i > -1; i--)
                        if (DateTime.Parse(db.Database.Trash.Logins[i].DeleteDate, System.Threading.Thread.CurrentThread.CurrentUICulture).AddDays(Program.Settings.DaysAfterDeletion) <= DateTime.Now)
                            db.Database.Trash.Logins.Remove(db.Database.Trash.Logins[i]);

                    for (int i = db.Database.Trash.Folders.Count - 1; i > -1; i--)
                        if (DateTime.Parse(db.Database.Trash.Folders[i].DeleteDate, System.Threading.Thread.CurrentThread.CurrentUICulture).AddDays(Program.Settings.DaysAfterDeletion) <= DateTime.Now)
                            db.Database.Trash.Folders.Remove(db.Database.Trash.Folders[i]);
                }
            }

            App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not9") + $" {db.Header}");
        }
        private async void CreateDatabase()
        {
            CreateDatabaseWindow window = new CreateDatabaseWindow();
            if (await window.ShowDialog<bool>(App.MainWindow))
            {
                DatabaseViewModel db = new DatabaseViewModel
                {
                    Database = new Database { Folders = new List<Folder>(), Logins = new List<Login>() },
                    MasterPassword = window.TbPassword.Text,
                    Iterations = int.Parse(window.TbIteration.Text),
                    NumberOfEncryptionProcedures = int.Parse(window.TbNumberOfEncryptionProcedures.Text),
                    IsLockDatabase = false,
                    IsUnlockDatabase = true,
                    PathDatabase = window.TbPathDatabase.Text,
                    UseCompression = window.CbUseCompression.IsChecked ?? false,
                    UseTrash = window.CbUseTrash.IsChecked ?? false,
                    Header = Path.GetFileNameWithoutExtension(Program.Settings.PathDatabase = window.TbPathDatabase.Text),
                    CloseTabCallback = CloseTab
                };

                TabItems.Add(db);

                SaveDatabase(db);
                App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not3") + $" {db.Header}");
            }
        }
        private void ShowSearchWindow()
        {
            if (SelectedTabItem == null || SelectedTabItem.IsLockDatabase) return;
            App.SearchWindow = new SearchWindow();

            for (int index = 0; index < SelectedTabItem.Database.Folders.Count; index++)
                App.SearchWindow.SearchViewModel.AddFolder(SelectedTabItem.Database.Folders[index]);

            App.SearchWindow.ShowDialog(App.MainWindow);
        }
        private void SaveAllDatabases()
        {
            for (int index = 0; index < TabItems.Count; index++)
                SaveDatabase(TabItems[index]);
        }
        private void OpenSettingsWindow()
        {
            _settingsWindow = new SettingsWindow();
            _settingsWindow.ShowDialog(App.MainWindow);
        }
        private void ChangeMasterPassword()
        {
            _windowChangeMasterPassword = new ChangeMasterPasswordWindow();
            _windowChangeMasterPassword.ShowDialog(App.MainWindow);
        }
        private void ClearGC()
        {
            for (int index = 0; index < TabItems.Count; index++)
            {
                DatabaseViewModel item = TabItems[index];
                for (int i = 0; i < item.Router.NavigationStack.Count; i++)
                    item.Router.NavigationStack.Remove(item.Router.NavigationStack[i]);
            }
            GC.Collect(0, GCCollectionMode.Forced);
            App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("ClearGC"));
        }
        private void OpenTrashWindow()
        {
            if (IsUnlockDatabase)
            {
                TrashWindow = new TrashWindow();
                TrashWindow.ShowDialog(App.MainWindow);
            }
        }

        private void CheckUpdate() => App.CheckUpdate(true);
        private void ExitApplication() => App.MainWindow.Close();
        private void SaveSettings() => SaveAndLoadSettings.SaveSettings();
    }
}