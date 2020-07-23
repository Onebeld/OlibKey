using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using ReactiveUI;
using OlibKey.Core;
using OlibKey.Views.Controls;
using OlibKey.Structures;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Windows;
using Path = System.IO.Path;

namespace OlibKey
{
	public class MainWindowViewModel : ReactiveObject, IScreen
	{
		public int Iterations { get; set; }
		public int NumberOfEncryptionProcedures { get; set; }

		private string _nameDatabase;

		private bool _isUnlockDatabase;
		private bool _isLockDatabase;
		private bool _isOSX;

		private int _selectedIndex;

		private ObservableCollection<LoginListItem> _loginList = new ObservableCollection<LoginListItem>();

		public RoutingState _router = new RoutingState();

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

		#endregion

		#region Propertie's

		public ObservableCollection<LoginListItem> LoginList
		{
			get => _loginList;
			set => this.RaiseAndSetIfChanged(ref _loginList, value);
		}
		public RoutingState Router
		{
			get => _router;
			set => this.RaiseAndSetIfChanged(ref _router, value);
		}
		private bool IsLockDatabase
		{
			get => _isLockDatabase;
			set => this.RaiseAndSetIfChanged(ref _isLockDatabase, value);
		}
		private bool IsUnlockDatabase
		{
			get => _isUnlockDatabase;
			set => this.RaiseAndSetIfChanged(ref _isUnlockDatabase, value);
		}
		private bool IsOSX
		{
			get => _isOSX;
			set => this.RaiseAndSetIfChanged(ref _isOSX, value);
		}
		private string NameStorage
		{
			get => _nameDatabase;
			set => this.RaiseAndSetIfChanged(ref _nameDatabase, value);
		}
		private int SelectedIndex
		{
			get => _selectedIndex;
			set
			{
				this.RaiseAndSetIfChanged(ref _selectedIndex, value);
				InformationLogin(SelectedLoginItem);
			}
		}
		public string MasterPassword { get; set; }
		private LoginListItem SelectedLoginItem { get { try { return LoginList[SelectedIndex]; } catch { return null; } } }

		#endregion

		public MainWindowViewModel()
		{
			ExitProgramCommand = ReactiveCommand.Create(ExitApplication);
			CreateLoginCommand = ReactiveCommand.Create(CreateLogin);
			OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
			CreateDatabaseCommand = ReactiveCommand.Create(CreateDatabase);
			SaveDatabaseCommand = ReactiveCommand.Create(SaveDatabase);
			UnlockDatabaseCommand = ReactiveCommand.Create(UnlockDatabase);
			LockDatabaseCommand = ReactiveCommand.Create(LockDatabase);
			OpenDatabaseCommand = ReactiveCommand.Create(OpenDatabase);
			CheckUpdateCommand = ReactiveCommand.Create(CheckUpdate);
			ShowSearchWindowCommand = ReactiveCommand.Create(ShowSearchWindow);
			OpenPasswordGeneratorWindowCommand = ReactiveCommand.Create(() => { new PasswordGeneratorWindow().ShowDialog(App.MainWindow); });
			OpenAboutWindowCommand = ReactiveCommand.Create(() => { new AboutWindow().ShowDialog(App.MainWindow); });
			ChangeMasterPasswordCommand = ReactiveCommand.Create(ChangeMasterPassword);


			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) IsOSX = true;
		}

		public async void Loading(MainWindow mainWindow)
		{
			if (!string.IsNullOrEmpty(App.Settings.PathDatabase) && File.Exists(App.Settings.PathDatabase))
			{
				NameStorage = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase);
				IsLockDatabase = true;
				RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
				{
					LoadStorageCallback = LoadDatabase
				};
				await passwordWindow.ShowDialog(mainWindow);
			}
		}
		public void ProgramClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			App.Autosave.Stop();
			SaveSettings();
			SaveDatabase();
		}
		public void SaveDatabase()
		{
			if (IsLockDatabase || !IsUnlockDatabase) return;
			if (App.Settings.PathDatabase != null)
			{
				App.Database.Logins = LoginList.Select(item => item.LoginItem).ToList();

				SaveAndLoadDatabase.SaveFiles(App.Database, App.Settings.PathDatabase, MasterPassword);
				App.MainWindow.MessageStatusBar("Not4");
			}
			for (int i = 0; i < Router.NavigationStack.Count - 1; i++)
			{
				IRoutableViewModel item = Router.NavigationStack[i];
				Router.NavigationStack.Remove(item);
			}
		}
		public void SearchSelectLogin(LoginListItem i)
		{
			foreach (var item in LoginList)
			{
				if (item.LoginID == i.LoginID)
				{
					SelectedIndex = LoginList.IndexOf(item);
					break;
				}
			}
		}
		private void UnlockDatabase()
		{
			RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
			{
				LoadStorageCallback = LoadDatabase
			};
			passwordWindow.ShowDialog(App.MainWindow);
		}
		private void LockDatabase()
		{
			App.Autosave.Stop();
			SaveDatabase();
			ClearLoginsList();
			IsUnlockDatabase = false;
			IsLockDatabase = true;
			Router.Navigate.Execute(new StartPageViewModel());
		}
		private async void OpenDatabase()
		{
			var dialog = new OpenFileDialog();
			dialog.Filters.Add(new FileDialogFilter { Name = "Olib-Files", Extensions = { "olib" } });
			string[] res = await dialog.ShowAsync(App.MainWindow);
			try
			{
				if (res?[0] != null)
				{
					LockDatabase();

					App.Settings.PathDatabase = res[0];
					NameStorage = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase);

					var requireMaster = new RequireMasterPasswordWindow { LoadStorageCallback = LoadDatabase };
					await requireMaster.ShowDialog(App.MainWindow);
				}
			}
			catch { }
		}
		private void CreateLogin()
		{
			SelectedIndex = -1;
			CreateLoginPageViewModel page = new CreateLoginPageViewModel
			{
				BackPageCallback = StartPage,
				CreateLoginCallback = AddLogin
			};
			Router.Navigate.Execute(page);
		}
		private void EditLogin(LoginListItem login)
		{
			Router.Navigate.Execute(new EditLoginPageViewModel(login)
			{
				CancelCallback = BackPage,
				EditCompleteCallback = EditComplete,
				DeleteLoginCallback = DeleteLogin
			});
		}
		private void AddLogin(Login loginContent)
		{
			LoginListItem ali = new LoginListItem(loginContent)
			{
				LoginID = Guid.NewGuid().ToString("N")
			};

			LoginList.Add(ali);
			ali.GetIconElement();
			Router.Navigate.Execute(new StartPageViewModel());
		}
		private void DeleteLogin()
		{
			int i = SelectedIndex;
			LoginList.RemoveAt(SelectedIndex);
			SelectedIndex = i;
			SelectedIndex = -1;
			Router.Navigate.Execute(new StartPageViewModel());
		}
		private void EditComplete(LoginListItem a)
		{
			a.EditedLogin();
			a.GetIconElement();
			Router.Navigate.Execute(new LoginInformationPageViewModel(a) { EditContentCallback = EditLogin });
		}
		private void InformationLogin(LoginListItem i)
		{
			if (i == null) return;
			Router.Navigate.Execute(new LoginInformationPageViewModel(i) { EditContentCallback = EditLogin });
		}
		private void LoadDatabase()
		{
			App.Database = SaveAndLoadDatabase.LoadFiles(App.Settings.PathDatabase, MasterPassword);
			ClearLoginsList();
			foreach (Login logins in App.Database.Logins) AddLogin(logins);
			IsUnlockDatabase = true;
			IsLockDatabase = false;

			App.Autosave.Start();
		}
		private async void CreateDatabase()
		{
			CreateDatabaseWindow window = new CreateDatabaseWindow();
			bool res = await window.ShowDialog<bool>(App.MainWindow);
			if (res)
			{
				App.Database = new Database();

				App.Settings.PathDatabase = window.TbPathDatabase.Text;
				MasterPassword = window.TbPassword.Text;
				Iterations = int.Parse(window.TbIteration.Text);
				NumberOfEncryptionProcedures = int.Parse(window.TbNumberOfEncryptionProcedures.Text);
				NameStorage = Path.GetFileNameWithoutExtension(App.Settings.PathDatabase);
				IsUnlockDatabase = true;
				IsLockDatabase = false;

				ClearLoginsList();
				SaveDatabase();
				Router.Navigate.Execute(new StartPageViewModel());
				App.Autosave.Start();
				App.MainWindow.MessageStatusBar("Not3");
			}
		}
		private void OpenSettingsWindow()
		{
			SettingsWindow window = new SettingsWindow();
			window.ShowDialog(App.MainWindow);
		}
		private void ClearLoginsList()
		{
			SelectedIndex = -1;
			LoginList.Clear();
		}
		private void ShowSearchWindow()
		{
			App.SearchWindow = new SearchWindow();
			foreach (CustomFolder folder in App.Database.CustomFolders) App.SearchWindow.SearchViewModel.AddFolder(folder);
			App.SearchWindow.ShowDialog(App.MainWindow);
		}
		private void CheckUpdate() => App.CheckUpdate(true);
		private void ChangeMasterPassword() => new ChangeMasterPasswordWindow().ShowDialog(App.MainWindow);
		private void ExitApplication() => App.MainWindow.Close();
		private void StartPage() => Router.Navigate.Execute(new StartPageViewModel());
		private void BackPage(LoginListItem a) => Router.Navigate.Execute(new LoginInformationPageViewModel(a) { EditContentCallback = EditLogin });
		private void SaveSettings() => SaveAndLoadSettings.SaveSettings();

		//// Problem with ListBox ////

		//public void MoveUp() => MoveItem(-1);

		//public void MoveDown() => MoveItem(1);

		//private void MoveItem(int direction)
		//{
		//	if (SelectedLoginItem == null)
		//		return;

		//	var newIndex = SelectedIndex + direction;

		//	if (newIndex < 0 || newIndex >= LoginsList.Count)
		//		return;

		//	LoginsList.Move(SelectedIndex, newIndex);
		//	SelectedIndex = newIndex;
		//}
	}
}