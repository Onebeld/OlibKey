using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using OlibKey.Core;
using OlibKey.Views.Controls;
using OlibKey.Structures;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Windows;
using System;
using System.Collections.Generic;
using Avalonia;

namespace OlibKey
{
	public class MainWindowViewModel : ReactiveObject
	{
		private bool _isUnlockDatabase;
		private bool _isLockDatabase;

		private int _selectedTabIndex;
		private int _countLogins;

		private ChangeMasterPasswordWindow _windowChangeMasterPassword;
		private SettingsWindow _settingsWindow;
		public PasswordGeneratorWindow PasswordGenerator;
		public CheckingWeakPasswordsWindow CheckingWindow;

		private ObservableCollection<TabItem> _tabItems = new ObservableCollection<TabItem>();

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

		#endregion

		#region Propertie's

		public ObservableCollection<TabItem> TabItems
		{
			get => _tabItems;
			set => this.RaiseAndSetIfChanged(ref _tabItems, value);
		}
		private bool IsLockDatabase
		{
			get => _isLockDatabase;
			set => this.RaiseAndSetIfChanged(ref _isLockDatabase, value);
		}
		public bool IsUnlockDatabase
		{
			get => _isUnlockDatabase;
			set => this.RaiseAndSetIfChanged(ref _isUnlockDatabase, value);
		}
		private int SelectedTabIndex
		{
			get => _selectedTabIndex;
			set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
		}
		public int CountLogins
		{
			get => _countLogins;
			set => this.RaiseAndSetIfChanged(ref _countLogins, value);
		}
		public DatabaseControl SelectedTabItem { get { try { return (DatabaseControl)TabItems[SelectedTabIndex].Content; } catch { return null; } } }
		private DatabaseTabHeader SelectedTabItemHeader { get { try { return (DatabaseTabHeader)TabItems[SelectedTabIndex].Header; } catch { return null; } } }
		public List<string> OpenStorages { get; set; }

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

			App.Autoblock.Tick += AutoblockStorage;
		}

		public void Loading()
		{
			foreach (string item in OpenStorages.Where(item => Path.GetExtension(item) == ".olib"))
			{
				Program.Settings.PathDatabase = item;

				CreateTab(Program.Settings.PathDatabase, true, false);
			}

			if (OpenStorages.Count > 0)
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
				TabItems.All(i => ((DatabaseControl)i.Content).ViewModel.PathDatabase != item)))
			{
				Program.Settings.PathDatabase = item;

				CreateTab(Program.Settings.PathDatabase, true, false);
			}

			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not6"));
		}
		public void ProgramClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			App.Autosave.Stop();
			SaveSettings();
			for (int index = 0; index < TabItems.Count; index++) SaveDatabase((DatabaseControl)TabItems[index].Content);
		}
		public void SaveDatabase(DatabaseControl db)
		{
			if (db == null || db.ViewModel.IsLockDatabase || !db.ViewModel.IsUnlockDatabase) return;
			if (db.ViewModel.PathDatabase != null)
			{
				db.ViewModel.Database.Logins = db.ViewModel.LoginList.Select(item => item.LoginItem).ToList();

				SaveAndLoadDatabase.SaveFiles(db);
				App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not4") + $" {Path.GetFileNameWithoutExtension(db.ViewModel.PathDatabase)}");
			}
			for (int i = 0; i < db.ViewModel.Router.NavigationStack.Count - 1; i++)
				db.ViewModel.Router.NavigationStack.Remove(db.ViewModel.Router.NavigationStack[i]);
		}
		public void TabItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SelectedTabItem != null)
			{
				IsUnlockDatabase = SelectedTabItem.ViewModel.IsUnlockDatabase;
				IsLockDatabase = SelectedTabItem.ViewModel.IsLockDatabase;
				CountLogins = SelectedTabItem.ViewModel.LoginList.Count;
			}
			else
			{
				IsUnlockDatabase = false;
				IsLockDatabase = false;
			}

		}
		private async void CreateTab(string path, bool isLockDatabase, bool isUnlockDatabase)
		{
			string id = Guid.NewGuid().ToString("N");

			DatabaseControl db = new DatabaseControl
			{
				ViewModel =
						{
							Database = new Database(),
							IsLockDatabase = isLockDatabase,
							IsUnlockDatabase = isUnlockDatabase,
							PathDatabase = path,
							TabID = id
						}
			};
			DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(Program.Settings.PathDatabase))
			{
				CloseTab = CloseTab,
				iLock = { IsVisible = true },
				iUnlock = { IsVisible = false }
			};

			TabItems.Add(new TabItem { Header = tabHeader, Content = db });

			await new RequireMasterPasswordWindow
			{
				LoadStorageCallback = LoadDatabase,
				databaseControl = db,
				databaseTabHeader = tabHeader,
				tbNameStorage = { Text = Path.GetFileNameWithoutExtension(Program.Settings.PathDatabase) }
			}.ShowDialog(App.MainWindow);
		}
		private void OpenCheckingWeakPasswordsWindow()
		{
			if (SelectedTabItem == null || SelectedTabItem.ViewModel.IsLockDatabase) return;
			CheckingWindow = new CheckingWeakPasswordsWindow();
			CheckingWindow.ShowDialog(App.MainWindow);

			SelectedTabItem.ViewModel.SelectedIndex = -1;
			SelectedTabItem.ViewModel.Router.Navigate.Execute(new StartPageViewModel());
		}
		private void CloseTab(string tabID)
		{
			foreach (TabItem item in TabItems.Where(item => ((DatabaseControl)item.Content).ViewModel.TabID == tabID))
			{
				SaveDatabase((DatabaseControl)item.Content);

				TabItems.Remove(item);
				break;
			}
		}
		private void AutoblockStorage(object sender, EventArgs e)
		{
			App.SearchWindow?.Close();
			_windowChangeMasterPassword?.Close();
			_settingsWindow?.Close();
			PasswordGenerator?.Close();
			CheckingWindow?.Close();
			for (int index = 0; index < TabItems.Count; index++)
			{
				TabItem item = TabItems[index];
				DatabaseControl db = (DatabaseControl)item.Content;
				DatabaseTabHeader dbHeader = (DatabaseTabHeader)item.Header;

				if (db.ViewModel.IsUnlockDatabase && dbHeader != null)
				{
					if (db.ViewModel.LoginList.Any(login => login.LoginItem.IsReminderActive)) continue;

					SaveDatabase(db);
					db.ViewModel.ClearLoginsList();
					IsUnlockDatabase = db.ViewModel.IsUnlockDatabase = dbHeader.iUnlock.IsVisible = false;
					IsLockDatabase = db.ViewModel.IsLockDatabase = dbHeader.iLock.IsVisible = true;
					db.ViewModel.Router.Navigate.Execute(new StartPageViewModel(db.ViewModel));
				}
			}

			App.Autoblock.Stop();
		}
		private async void UnlockDatabase()
		{
			if (SelectedTabItem == null || SelectedTabItem.ViewModel.IsUnlockDatabase) return;

			await new RequireMasterPasswordWindow
			{
				LoadStorageCallback = LoadDatabase,
				databaseControl = SelectedTabItem,
				databaseTabHeader = SelectedTabItemHeader,
				tbNameStorage = { Text = Path.GetFileNameWithoutExtension(SelectedTabItem.ViewModel.PathDatabase) }
			}.ShowDialog(App.MainWindow);
		}
		private async void UnlockAllDatabases()
		{
			foreach (TabItem item in TabItems.Where(item => ((DatabaseControl)item.Content).ViewModel.IsLockDatabase))
				await new RequireMasterPasswordWindow 
				{ 
					LoadStorageCallback = LoadDatabase, 
					databaseControl = (DatabaseControl)item.Content, 
					databaseTabHeader = (DatabaseTabHeader)item.Header, 
					tbNameStorage = { Text = Path.GetFileNameWithoutExtension(((DatabaseControl)item.Content).ViewModel.PathDatabase) }
				}.ShowDialog(App.MainWindow);
			
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not8"));
		}
		private void LockDatabase()
		{
			if (SelectedTabItem == null || SelectedTabItem.ViewModel.IsLockDatabase) return;

			SaveDatabase(SelectedTabItem);
			SelectedTabItem.ViewModel.ClearLoginsList();
			IsUnlockDatabase = SelectedTabItem.ViewModel.IsUnlockDatabase = SelectedTabItemHeader.iUnlock.IsVisible = false;
			IsLockDatabase = SelectedTabItem.ViewModel.IsLockDatabase = SelectedTabItemHeader.iLock.IsVisible = true;
			SelectedTabItem.ViewModel.Router.Navigate.Execute(new StartPageViewModel(SelectedTabItem.ViewModel));
		}
		private void LockAllDatabases()
		{
			for (int index = 0; index < TabItems.Count; index++)
			{
				TabItem item = TabItems[index];
				DatabaseControl db = (DatabaseControl)item.Content;
				DatabaseTabHeader dbHeader = (DatabaseTabHeader)item.Header;

				SaveDatabase(db);
				if (dbHeader != null)
				{
					db.ViewModel.ClearLoginsList();
					IsUnlockDatabase = db.ViewModel.IsUnlockDatabase = dbHeader.iUnlock.IsVisible = false;
					IsLockDatabase = db.ViewModel.IsLockDatabase = dbHeader.iLock.IsVisible = true;
					db.ViewModel.Router.Navigate.Execute(new StartPageViewModel(db.ViewModel));
				}
			}

			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not7"));
		}
		private async void OpenDatabase()
		{
			OpenFileDialog dialog = new OpenFileDialog { AllowMultiple = true };
			dialog.Filters.Add(new FileDialogFilter { Name = (string)Application.Current.FindResource("FileOlib"), Extensions = { "olib" } });
			List<string> files = (await dialog.ShowAsync(App.MainWindow)).ToList();

			if (files == null || files.Count == 0) return;

			foreach (string file in files.Where(file =>
				TabItems.All(i => ((DatabaseControl)i.Content).ViewModel.PathDatabase != file)))
			{
				Program.Settings.PathDatabase = file;

				CreateTab(Program.Settings.PathDatabase, true, false);
			}
		}
		private void CreateLogin()
		{
			if (SelectedTabItem == null || SelectedTabItem.ViewModel.IsLockDatabase) return;

			SelectedTabItem.ViewModel.SelectedIndex = -1;

			SelectedTabItem.ViewModel.Router.Navigate.Execute(new CreateLoginPageViewModel(SelectedTabItem.ViewModel.Database, SelectedTabItem.ViewModel)
			{
				BackPageCallback = SelectedTabItem.ViewModel.StartPage,
				CreateLoginCallback = SelectedTabItem.ViewModel.AddLogin
			});
		}
		private void LoadDatabase(DatabaseControl db, DatabaseTabHeader dbHeader)
		{
			db.ViewModel.Database = SaveAndLoadDatabase.LoadFiles(db);
			for (int index = 0; index < db.ViewModel.Database.Logins.Count; index++)
				db.ViewModel.AddLogin(db.ViewModel.Database.Logins[index]);
			
			db.ViewModel.IsLockDatabase = dbHeader.iLock.IsVisible = false;
			db.ViewModel.IsUnlockDatabase = dbHeader.iUnlock.IsVisible = true;

			if (Equals(SelectedTabItem, db))
			{
				IsLockDatabase = false;
				IsUnlockDatabase = true;
			}

			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not9") + $" {Path.GetFileNameWithoutExtension(db.ViewModel.PathDatabase)}");
		}
		private async void CreateDatabase()
		{
			CreateDatabaseWindow window = new CreateDatabaseWindow();
			if (await window.ShowDialog<bool>(App.MainWindow))
			{
				Program.Settings.PathDatabase = window.TbPathDatabase.Text;

				string id = Guid.NewGuid().ToString("N");

				DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(Program.Settings.PathDatabase))
				{
					CloseTab = CloseTab,
					iLock = { IsVisible = false },
					iUnlock = { IsVisible = true }
				};

				DatabaseControl db = new DatabaseControl
				{
					ViewModel =
					{
						Database = new Database { Folders = new List<Folder>(), Logins = new List<Login>() },
						MasterPassword = window.TbPassword.Text,
						Iterations = int.Parse(window.TbIteration.Text),
						NumberOfEncryptionProcedures = int.Parse(window.TbNumberOfEncryptionProcedures.Text),
						IsLockDatabase = false,
						IsUnlockDatabase = true,
						PathDatabase = window.TbPathDatabase.Text,
						TabID = id,
						UseCompression = (bool)window.CbUseCompression.IsChecked
					}
				};

				TabItems.Add(new TabItem { Header = tabHeader, Content = db });

				SaveDatabase(db);
				App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not3") + $" {Path.GetFileNameWithoutExtension(Program.Settings.PathDatabase)}");
			}
		}
		private void ShowSearchWindow()
		{
			if (SelectedTabItem == null || SelectedTabItem.ViewModel.IsLockDatabase) return;
			App.SearchWindow = new SearchWindow();

			for (int index = 0; index < SelectedTabItem.ViewModel.Database.Folders.Count; index++)
				App.SearchWindow.SearchViewModel.AddFolder(SelectedTabItem.ViewModel.Database.Folders[index]);
			
			App.SearchWindow.ShowDialog(App.MainWindow);
		}
		private void SaveAllDatabases()
		{
			for (int index = 0; index < TabItems.Count; index++)
				SaveDatabase((DatabaseControl)TabItems[index].Content);
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
				DatabaseControl item = (DatabaseControl)TabItems[index].Content;
				foreach (IRoutableViewModel i in item.ViewModel.Router.NavigationStack)
					item.ViewModel.Router.NavigationStack.Remove(i);
			}
			GC.Collect(0, GCCollectionMode.Forced);
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("ClearGC"));
		}

		private void CheckUpdate() => App.CheckUpdate(true);
		private void ExitApplication() => App.MainWindow.Close();
		private void SaveSettings() => SaveAndLoadSettings.SaveSettings();
	}
}