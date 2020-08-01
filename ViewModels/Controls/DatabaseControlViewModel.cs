using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Threading;
using OlibKey.Structures;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Controls;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using OlibKey.Views.Windows;

namespace OlibKey.ViewModels.Controls
{
	public class DatabaseControlViewModel : ReactiveObject, IScreen
	{
		public Database Database { get; set; }

		public string PathDatabase { get; set; }
		public string TabID { get; set; }

		public int Iterations { get; set; }
		public int NumberOfEncryptionProcedures { get; set; }

		private bool _isUnlockDatabase;
		private bool _isLockDatabase;

		private int _selectedIndex;

		private ObservableCollection<LoginListItem> _loginList = new ObservableCollection<LoginListItem>();

		private RoutingState _router = new RoutingState();

		#region ReactiveCommands   

		private ReactiveCommand<Unit, Unit> CreateLoginCommand { get; }
		private ReactiveCommand<Unit, Unit> ShowSearchWindowCommand { get; }

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
		public int SelectedIndex
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

		public DatabaseControlViewModel()
		{
			CreateLoginCommand = ReactiveCommand.Create(CreateLogin);
			ShowSearchWindowCommand = ReactiveCommand.Create(ShowSearchWindow);
			SelectedIndex = -1;
		}
		public void SearchSelectLogin(LoginListItem i)
		{
			foreach (LoginListItem item in LoginList.Where(item => item.LoginID == i.LoginID))
			{
				SelectedIndex = LoginList.IndexOf(item);
				break;
			}
		}
		private void ShowSearchWindow()
		{
			App.SearchWindow = new SearchWindow();
			foreach (Folder folder in Database.Folders) App.SearchWindow.SearchViewModel.AddFolder(folder);
			App.SearchWindow.ShowDialog(App.MainWindow);
		}
		public void AddLogin(Login loginContent)
		{
			LoginListItem ali = new LoginListItem(loginContent)
			{
				LoginID = Guid.NewGuid().ToString("N")
			};

			LoginList.Add(ali);
			ali.GetIconElement();

			if (App.MainWindowViewModel.SelectedTabItem.ViewModel == this)
				App.MainWindowViewModel.CountLogins = LoginList.Count;

			Router.Navigate.Execute(new StartPageViewModel(this));
		}
		public void CreateLogin()
		{
			SelectedIndex = -1;
			CreateLoginPageViewModel page = new CreateLoginPageViewModel(Database, this)
			{
				BackPageCallback = StartPage,
				CreateLoginCallback = AddLogin
			};
			Router.Navigate.Execute(page);
		}
		private void EditLogin(LoginListItem login)
		{
			Router.Navigate.Execute(new EditLoginPageViewModel(login, Database, this)
			{
				CancelCallback = BackPage,
				EditCompleteCallback = EditComplete,
				DeleteLoginCallback = DeleteLogin
			});
		}
		private void DeleteLogin()
		{
			LoginList.RemoveAt(SelectedIndex);
			Router.Navigate.Execute(new StartPageViewModel(this));
		}
		private void EditComplete(LoginListItem a)
		{
			a.EditedLogin();
			a.GetIconElement();
			Router.Navigate.Execute(new LoginInformationPageViewModel(a, Database, this) { EditContentCallback = EditLogin });
		}
		private void InformationLogin(LoginListItem i)
		{
			if (i == null) return;
			Router.Navigate.Execute(new LoginInformationPageViewModel(i, Database, this) { EditContentCallback = EditLogin });
		}
		public void ClearLoginsList()
		{
			SelectedIndex = -1;
			LoginList.Clear();
		}
		public void StartPage() => Router.Navigate.Execute(new StartPageViewModel(this));

		private void BackPage(LoginListItem a) => Router.Navigate.Execute(new LoginInformationPageViewModel(a, Database, this) { EditContentCallback = EditLogin });


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
