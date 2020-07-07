using OlibKey.Views.Controls;
using OlibKey.Structures;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Windows;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using OlibKey.Core;
using Path = System.IO.Path;
using System;

namespace OlibKey
{
    public class MainWindowViewModel: ReactiveObject, IScreen
    {
        private string _nameDatabase;

        private bool _isUnlockDatabase;
        private bool _isLockDatabase;
        private bool _isOSX;

        private int _selectedIndex;

        private ObservableCollection<AccountListItem> list = new ObservableCollection<AccountListItem>();

        public RoutingState _router = new RoutingState();

        #region ReactiveCommands                               
        public ReactiveCommand<Unit, Unit> ExitProgramCommand { get; }
        public ReactiveCommand<Unit, Unit> AddLoginCommand { get; }
		public ReactiveCommand<Unit, Unit> CreateDatabaseCommand { get; }
		public ReactiveCommand<Unit, Unit> SaveDatabaseCommand { get; }
		public ReactiveCommand<Unit, Unit> UnlockDatabaseCommand { get; }
		public ReactiveCommand<Unit, Unit> LockDatabaseCommand { get; }
		public ReactiveCommand<Unit, Unit> OpenDatabaseCommand { get; }
		public ReactiveCommand<Unit, Unit> ShowSearchWindowCommand { get; }
		public ReactiveCommand<Unit, Unit> ChangeMasterPasswordCommand { get; }
		public ReactiveCommand<Unit, Unit> OpenPasswordGeneratorWindowCommand { get; }
		public ReactiveCommand<Unit, Unit> OpenAboutWindowCommand { get; }
		public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }
		public ReactiveCommand<Unit, Unit> CheckUpdateCommand { get; }
        #endregion

        #region PublicProperties
        public ObservableCollection<AccountListItem> AccountsList
        {
            get => list;
            set => this.RaiseAndSetIfChanged(ref list, value);
        }

        public RoutingState Router
        {
            get => _router;
            set => this.RaiseAndSetIfChanged(ref _router, value);
        }

        public bool IsLockDatabase
        {
            get => _isLockDatabase;
            set => this.RaiseAndSetIfChanged(ref _isLockDatabase, value);
        }
        public bool IsUnlockDatabase
        {
            get => _isUnlockDatabase;
            set => this.RaiseAndSetIfChanged(ref _isUnlockDatabase, value);
        }
        public bool IsOSX
        {
	        get => _isOSX;
	        set => this.RaiseAndSetIfChanged(ref _isOSX, value);
        }

        public string NameStorage
        {
            get => _nameDatabase;
            set => this.RaiseAndSetIfChanged(ref _nameDatabase, value);
        }

        public int SelectedIndex
		{
			get => _selectedIndex;
			set
			{
				this.RaiseAndSetIfChanged(ref _selectedIndex, value);
				InformationAccountVoid(SelectedAccountItem);
			}
		}

		public string MasterPassword { get; set; }

        public AccountListItem SelectedAccountItem { get { try { return AccountsList[SelectedIndex]; } catch { return null; } } }
        #endregion

        public MainWindowViewModel()
        {
	        ExitProgramCommand = ReactiveCommand.Create(ExitApplication);
            AddLoginCommand = ReactiveCommand.Create(AddLoginVoid);
            OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
            CreateDatabaseCommand = ReactiveCommand.Create(CreateDatabase);
            SaveDatabaseCommand = ReactiveCommand.Create(SaveDatabase);
            UnlockDatabaseCommand = ReactiveCommand.Create(UnlockDatabase);
            LockDatabaseCommand = ReactiveCommand.Create(LockDatabase);
            OpenDatabaseCommand = ReactiveCommand.Create(OpenDatabase);
            CheckUpdateCommand = ReactiveCommand.Create(CheckUpdateForCommad);
			ShowSearchWindowCommand = ReactiveCommand.Create(ShowSearchWindow);
			OpenPasswordGeneratorWindowCommand = ReactiveCommand.Create(() => { new PasswordGeneratorWindow().ShowDialog(App.MainWindow); });
			OpenAboutWindowCommand = ReactiveCommand.Create(() => { new AboutWindow().ShowDialog(App.MainWindow); });
			ChangeMasterPasswordCommand = ReactiveCommand.Create(ChangeMasterPassword);


			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) IsOSX = true;
        }

        public async void Loading(MainWindow mainWindow)
        {
	        if (!string.IsNullOrEmpty(App.Settings.PathStorage) && File.Exists(App.Settings.PathStorage))
	        {
		        NameStorage = Path.GetFileNameWithoutExtension(App.Settings.PathStorage);
		        IsLockDatabase = true;
		        await Task.Delay(300);
		        RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
		        {
			        LoadStorageCallback = LoadDatabase
		        };
		        await passwordWindow.ShowDialog(mainWindow);
	        }
		}

        private void CheckUpdateForCommad() => App.CheckUpdate(true);

        public void UnlockDatabase()
        {
	        RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
	        {
		        LoadStorageCallback = LoadDatabase
	        };
	        passwordWindow.ShowDialog(App.MainWindow);
		}

		public void ChangeMasterPassword() => new ChangeMasterPasswordWindow().ShowDialog(App.MainWindow);

		public void LockDatabase()
        {
			App.Autosave.Stop();
			SaveDatabase();
			ClearAccountsList();
			IsUnlockDatabase = false;
			IsLockDatabase = true;
			Router.Navigate.Execute(new StartPageViewModel());
		}

        public async void OpenDatabase()
        {
	        var dialog = new OpenFileDialog();
	        dialog.Filters.Add(new FileDialogFilter {Name = "Olib-Files", Extensions = {"olib"}});
	        string[] res = await dialog.ShowAsync(App.MainWindow);
	        try
	        {
		        if (res?[0] != null)
		        {
			        LockDatabase();

			        App.Settings.PathStorage = res[0];
			        NameStorage = Path.GetFileNameWithoutExtension(App.Settings.PathStorage);

			        var requireMaster = new RequireMasterPasswordWindow {LoadStorageCallback = LoadDatabase};
			        await requireMaster.ShowDialog(App.MainWindow);
		        }
	        }
	        catch { }
        }

		public void ProgramClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			App.Autosave.Stop();
			SaveSettings();
			SaveDatabase();
		}
		private void AddLoginVoid()
        {
			SelectedIndex = -1;
			CreateLoginPageViewModel page = new CreateLoginPageViewModel
            {
                BackPageCallback = StartPageVoid,
                AddAccountCallback = AddAccount
            };
            Router.Navigate.Execute(page);
        }
		private void ExitApplication() => App.MainWindow.Close();

		public void EditAccount(AccountListItem acc)
        {
	        Router.Navigate.Execute(new EditLoginPageViewModel(acc)
	        {
                CancelCallback = BackPageVoid,
                EditCompleteCallback = EditCompleteVoid,
                DeleteAccountCallback = DeleteAccountVoid
	        });
        }
        public void AddAccount(Account accountContent)
        {
			var ali = new AccountListItem(accountContent);
			ali.IDElement = Guid.NewGuid().ToString("N");

			AccountsList.Add(ali);
			ali.GetIconElement();
            Router.Navigate.Execute(new StartPageViewModel());
        }
        public void StartPageVoid() => Router.Navigate.Execute(new StartPageViewModel());
		public void BackPageVoid(AccountListItem a) => Router.Navigate.Execute(new LoginInformationPageViewModel(a) { EditContentCallback = EditAccount });

		public void EditCompleteVoid(AccountListItem a)
		{
			a.EditedAccount();
			a.GetIconElement();
			Router.Navigate.Execute(new LoginInformationPageViewModel(a) { EditContentCallback = EditAccount });
		}

		public void DeleteAccountVoid()
        {
	        var i = SelectedIndex;
	        AccountsList.RemoveAt(SelectedIndex);
	        SelectedIndex = i;
	        SelectedIndex = -1;
	        Router.Navigate.Execute(new StartPageViewModel());
        }

		public void InformationAccountVoid(AccountListItem i)
		{
			if (i == null) return;
			Router.Navigate.Execute(new LoginInformationPageViewModel(i) { EditContentCallback = EditAccount });
		}

		//// Problem with ListBox ////

		//public void MoveUp() => MoveItem(-1);

		//public void MoveDown() => MoveItem(1);

		//private void MoveItem(int direction)
		//{
		//	if (SelectedAccountItem == null)
		//		return;

		//	var newIndex = SelectedIndex + direction;

		//	if (newIndex < 0 || newIndex >= AccountsList.Count)
		//		return;

		//	AccountsList.Move(SelectedIndex, newIndex);
		//	SelectedIndex = newIndex;
		//}

		public void OpenSettingsWindow()
		{
			SettingsWindow window = new SettingsWindow();
			window.ShowDialog(App.MainWindow);
		}

		public void LoadDatabase()
		{
			App.Database = SaveAndLoadAccount.LoadFiles(App.Settings.PathStorage, MasterPassword);
			ClearAccountsList();
			foreach (var accounts in App.Database.Accounts) AddAccount(accounts);
			IsUnlockDatabase = true;
			IsLockDatabase = false;
			App.Autosave.Start();
		}

		public void ClearAccountsList()
		{
			SelectedIndex = -1;
			AccountsList.Clear();
		}

		public async void CreateDatabase()
		{
			CreateDatabaseWindow window = new CreateDatabaseWindow();
			bool res = await window.ShowDialog<bool>(App.MainWindow);
			if (res)
			{
				App.Database = new Database();

				App.Settings.PathStorage = window._tbPathDatabase.Text;
				MasterPassword = window._tbPassword.Text;
				NameStorage = Path.GetFileNameWithoutExtension(App.Settings.PathStorage);
				IsUnlockDatabase = true;
				IsLockDatabase = false;

				ClearAccountsList();
				SaveDatabase();
				Router.Navigate.Execute(new StartPageViewModel());
				App.Autosave.Start();
				App.MainWindow.MessageStatusBar("Not3");
			}
		}

		public void SaveSettings() => File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json", JsonSerializer.Serialize(App.Settings));

		public void SaveDatabase()
		{
			if (IsLockDatabase || !IsUnlockDatabase) return;
			if (App.Settings.PathStorage != null)
			{
				App.Database.Accounts = AccountsList.Select(item => item.AccountItem).ToList();

				SaveAndLoadAccount.SaveFiles(App.Database, App.Settings.PathStorage, MasterPassword);
				App.MainWindow.MessageStatusBar("Not4");
			}
		}
		public void ShowSearchWindow()
		{
			App.SearchWindow = new SearchWindow();
			foreach (var folder in App.Database.CustomFolders) App.SearchWindow.SearchViewModel.AddFolder(folder);
			App.SearchWindow.ShowDialog(App.MainWindow);
		}
		public void SearchSelectAccount(AccountListItem i)
		{
			foreach (var item in AccountsList)
			{
				if (item.IDElement == i.IDElement)
				{
					SelectedIndex = AccountsList.IndexOf(item);
					break;
				}
			}
		}
	}
}