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
		private bool _isActivateDnD;

		private int _selectedTabIndex;
		private int _countLogins;

		private ObservableCollection<TabItem> _tabItems = new ObservableCollection<TabItem>();

		#region ReactiveCommands   

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

		#endregion

		#region Propertie's

		public ObservableCollection<TabItem> TabItems
		{
			get => _tabItems;
			set => this.RaiseAndSetIfChanged(ref _tabItems, value);
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
		public bool IsActivateDnD
		{
			get => _isActivateDnD;
			set => this.RaiseAndSetIfChanged(ref _isActivateDnD, value);
		}
		public DatabaseControl SelectedTabItem { get { try { return (DatabaseControl)TabItems[SelectedTabIndex].Content; } catch { return null; } } }
		public DatabaseTabHeader SelectedTabItemHeader { get { try { return (DatabaseTabHeader)TabItems[SelectedTabIndex].Header; } catch { return null; } } }
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

			App.Autoblock.Tick += AutoblockStorage;
		}

		public async void Loading(MainWindow mainWindow)
		{
			foreach (string item in OpenStorages)
			{
				if (Path.GetExtension(item) == ".olib")
				{
					App.Settings.PathDatabase = item;

					string id = Guid.NewGuid().ToString("N");

					DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(App.Settings.PathDatabase))
					{
						CloseTab = CloseTab,
						iLock = { IsVisible = true },
						iUnlock = { IsVisible = false }
					};

					DatabaseControl db = new DatabaseControl
					{
						ViewModel =
						{
							IsLockDatabase = true,
							IsUnlockDatabase = false,
							PathDatabase = App.Settings.PathDatabase,
							TabID = id
						}
					};

					TabItems.Add(new TabItem { Header = tabHeader, Content = db });

					RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
					{
						LoadStorageCallback = LoadDatabase,
						databaseControl = db,
						databaseTabHeader = tabHeader,
						tbNameStorage = { Text = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase) }
					};
					IsActivateDnD = false;
					await passwordWindow.ShowDialog(App.MainWindow);
					IsActivateDnD = true;
				}
			}

			if (OpenStorages.Count > 0)
			{
				OpenStorages = null;
				return;
			}

			if (!string.IsNullOrEmpty(App.Settings.PathDatabase) && File.Exists(App.Settings.PathDatabase))
			{
				string id = Guid.NewGuid().ToString("N");

				DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(App.Settings.PathDatabase))
				{
					CloseTab = CloseTab,
					iLock = { IsVisible = true },
					iUnlock = { IsVisible = false }
				};

				DatabaseControl db = new DatabaseControl
				{
					ViewModel =
					{
						IsLockDatabase = true,
						IsUnlockDatabase = false,
						PathDatabase = App.Settings.PathDatabase,
						TabID = id
					}
				};

				TabItems.Add(new TabItem { Header = tabHeader, Content = db });

				RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
				{
					LoadStorageCallback = LoadDatabase,
					databaseControl = db,
					databaseTabHeader = tabHeader,
					tbNameStorage = { Text = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase) }
				};
				IsActivateDnD = false;
				await passwordWindow.ShowDialog(mainWindow);
				IsActivateDnD = true;
			}
		}
		public async void OpenStorageDnD(List<string> files)
		{
			foreach (string item in files)
			{
				if (Path.GetExtension(item) == ".olib")
				{
					bool flag = false;
					foreach (TabItem i in TabItems)
						if (((DatabaseControl)i.Content).ViewModel.PathDatabase == item) { flag = true; break; }
					if (flag) continue;

					App.Settings.PathDatabase = item;

					string id = Guid.NewGuid().ToString("N");

					DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(App.Settings.PathDatabase))
					{
						CloseTab = CloseTab,
						iLock = { IsVisible = true },
						iUnlock = { IsVisible = false }
					};

					DatabaseControl db = new DatabaseControl
					{
						ViewModel =
						{
							IsLockDatabase = true,
							IsUnlockDatabase = false,
							PathDatabase = App.Settings.PathDatabase,
							TabID = id
						}
					};

					TabItems.Add(new TabItem { Header = tabHeader, Content = db });

					RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
					{
						LoadStorageCallback = LoadDatabase,
						databaseControl = db,
						databaseTabHeader = tabHeader,
						tbNameStorage = { Text = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase) }
					};
					IsActivateDnD = false;
					await passwordWindow.ShowDialog(App.MainWindow);
					IsActivateDnD = true;
				}
			}
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not6"));
		}
		public void AutoblockStorage(object sender, EventArgs e)
		{
			foreach (TabItem item in TabItems)
			{
				DatabaseControl db = (DatabaseControl)item.Content;
				DatabaseTabHeader dbHeader = (DatabaseTabHeader)item.Header;

				if (db.ViewModel.IsUnlockDatabase)
				{
					bool flag = false;
					foreach (LoginListItem login in db.ViewModel.LoginList)
					{
						if (login.LoginItem.IsReminderActive == true) 
						{ 
							flag = true;
							break;
						}
					}
					if (flag) continue;

					SaveDatabase(db);
					db.ViewModel.ClearLoginsList();
					IsUnlockDatabase = db.ViewModel.IsUnlockDatabase = dbHeader.iUnlock.IsVisible = false;
					IsLockDatabase = db.ViewModel.IsLockDatabase = dbHeader.iLock.IsVisible = true;
					db.ViewModel.Router.Navigate.Execute(new StartPageViewModel(db.ViewModel));
				}
			}
			App.Autoblock.Stop();
		}
		public void ProgramClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			App.Autosave.Stop();
			SaveSettings();
			foreach (TabItem item in TabItems) SaveDatabase((DatabaseControl)item.Content);
		}
		public void SaveDatabase(DatabaseControl db)
		{
			if (db.ViewModel.IsLockDatabase || !db.ViewModel.IsUnlockDatabase) return;
			if (db.ViewModel.PathDatabase != null)
			{
				db.ViewModel.Database.Logins = db.ViewModel.LoginList.Select(item => item.LoginItem).ToList();

				SaveAndLoadDatabase.SaveFiles(db);
				App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not4") + $" {Path.GetFileNameWithoutExtension(db.ViewModel.PathDatabase)}");
			}
			for (int i = 0; i < db.ViewModel.Router.NavigationStack.Count - 1; i++)
			{
				IRoutableViewModel item = db.ViewModel.Router.NavigationStack[i];
				db.ViewModel.Router.NavigationStack.Remove(item);
			}
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
		public void CloseTab(string tabID)
		{
			foreach (TabItem item in TabItems.Where(item => ((DatabaseControl)item.Content).ViewModel.TabID == tabID))
			{
				SaveDatabase((DatabaseControl)item.Content);

				TabItems.Remove(item);
				break;
			}
		}
		private async void UnlockDatabase()
		{
			RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
			{
				LoadStorageCallback = LoadDatabase,
				databaseControl = SelectedTabItem,
				databaseTabHeader = SelectedTabItemHeader,
				tbNameStorage = { Text = Path.GetFileNameWithoutExtension(SelectedTabItem.ViewModel.PathDatabase) }
			};
			IsActivateDnD = false;
			await passwordWindow.ShowDialog(App.MainWindow);
			IsActivateDnD = true;
		}
		private async void UnlockAllDatabases()
		{
			foreach (var item in TabItems.Where(item => ((DatabaseControl)item.Content).ViewModel.IsLockDatabase == true))
			{
				DatabaseControl db = (DatabaseControl)item.Content;
				DatabaseTabHeader dbHeader = (DatabaseTabHeader)item.Header;
				RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
				{
					LoadStorageCallback = LoadDatabase,
					databaseControl = db,
					databaseTabHeader = dbHeader,
					tbNameStorage = { Text = Path.GetFileNameWithoutExtension(db.ViewModel.PathDatabase) }
				};
				IsActivateDnD = false;
				await passwordWindow.ShowDialog(App.MainWindow);
				IsActivateDnD = true;
			}
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not8"));
		}
		private void LockDatabase()
		{
			SaveDatabase(SelectedTabItem);
			SelectedTabItem.ViewModel.ClearLoginsList();
			IsUnlockDatabase = SelectedTabItem.ViewModel.IsUnlockDatabase = false;
			IsLockDatabase = SelectedTabItem.ViewModel.IsLockDatabase = true;
			SelectedTabItemHeader.iLock.IsVisible = true;
			SelectedTabItemHeader.iUnlock.IsVisible = false;
			SelectedTabItem.ViewModel.Router.Navigate.Execute(new StartPageViewModel(SelectedTabItem.ViewModel));
		}
		private void LockAllDatabases()
		{
			foreach (TabItem item in TabItems)
			{
				DatabaseControl db = (DatabaseControl)item.Content;
				DatabaseTabHeader dbHeader = (DatabaseTabHeader)item.Header;

				SaveDatabase(db);
				db.ViewModel.ClearLoginsList();
				IsUnlockDatabase = db.ViewModel.IsUnlockDatabase = dbHeader.iUnlock.IsVisible = false;
				IsLockDatabase = db.ViewModel.IsLockDatabase = dbHeader.iLock.IsVisible = true;
				db.ViewModel.Router.Navigate.Execute(new StartPageViewModel(db.ViewModel));
			}
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not7"));
		}
		private async void OpenDatabase()
		{
			OpenFileDialog dialog = new OpenFileDialog { AllowMultiple = true };
			dialog.Filters.Add(new FileDialogFilter { Name = "Olib-Files", Extensions = { "olib" } });
			List<string> files = (await dialog.ShowAsync(App.MainWindow)).ToList();
			try
			{
				if (files.Count == 0) return;

				foreach (string file in files)
				{
					bool flag = false;
					foreach (TabItem i in TabItems)
						if (((DatabaseControl)i.Content).ViewModel.PathDatabase == file) { flag = true; break; }
					if (flag) continue;

					App.Settings.PathDatabase = file;

					string id = Guid.NewGuid().ToString("N");

					DatabaseControl db = new DatabaseControl
					{
						ViewModel =
						{
							Database = new Database(),
							IsLockDatabase = true,
							IsUnlockDatabase = false,
							PathDatabase = file,
							TabID = id
						}
					};
					DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(App.Settings.PathDatabase))
					{
						CloseTab = CloseTab,
						iLock = { IsVisible = true },
						iUnlock = { IsVisible = false }
					};
					TabItems.Add(new TabItem { Header = tabHeader, Content = db });

					RequireMasterPasswordWindow requireMaster = new RequireMasterPasswordWindow { LoadStorageCallback = LoadDatabase, databaseControl = db, databaseTabHeader = tabHeader, tbNameStorage = { Text = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase) } };
					IsActivateDnD = false;
					await requireMaster.ShowDialog(App.MainWindow);
					IsActivateDnD = true;
				}
			}
			catch { }
		}
		private void CreateLogin()
		{
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
			foreach (Login logins in db.ViewModel.Database.Logins) db.ViewModel.AddLogin(logins);
			db.ViewModel.IsLockDatabase = false;
			db.ViewModel.IsUnlockDatabase = true;
			dbHeader.iLock.IsVisible = false;
			dbHeader.iUnlock.IsVisible = true;

			if (SelectedTabItem == db)
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
				App.Settings.PathDatabase = window.TbPathDatabase.Text;

				string id = Guid.NewGuid().ToString("N");

				DatabaseTabHeader tabHeader = new DatabaseTabHeader(id, Path.GetFileNameWithoutExtension(App.Settings.PathDatabase))
				{
					CloseTab = CloseTab,
					iLock = { IsVisible = false },
					iUnlock = { IsVisible = true }
				};

				DatabaseControl db = new DatabaseControl
				{
					ViewModel =
					{
						Database = new Database(),
						MasterPassword = window.TbPassword.Text,
						Iterations = int.Parse(window.TbIteration.Text),
						NumberOfEncryptionProcedures = int.Parse(window.TbNumberOfEncryptionProcedures.Text),
						IsLockDatabase = false,
						IsUnlockDatabase = true,
						PathDatabase = window.TbPathDatabase.Text,
						TabID = id
					}
				};

				TabItems.Add(new TabItem { Header = tabHeader, Content = db });

				SaveDatabase(db);
				App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not3"));
			}
		}
		private void ShowSearchWindow()
		{
			App.SearchWindow = new SearchWindow();
			foreach (Folder folder in SelectedTabItem.ViewModel.Database.Folders) App.SearchWindow.SearchViewModel.AddFolder(folder);
			App.SearchWindow.ShowDialog(App.MainWindow);
		}
		private void SaveAllDatabases()
		{
			foreach (TabItem item in TabItems) SaveDatabase((DatabaseControl)item.Content);
		}
		private void OpenSettingsWindow() => new SettingsWindow().ShowDialog(App.MainWindow);
		private void CheckUpdate() => App.CheckUpdate(true);
		private void ChangeMasterPassword() => new ChangeMasterPasswordWindow().ShowDialog(App.MainWindow);
		private void ExitApplication() => App.MainWindow.Close();
		private void SaveSettings() => SaveAndLoadSettings.SaveSettings();
	}
}