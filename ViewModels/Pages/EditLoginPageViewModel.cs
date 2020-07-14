using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using Splat;
using OlibKey.Views.Controls;
using OlibKey.Structures;
using OlibKey.Views.Windows;

namespace OlibKey.ViewModels.Pages
{
	public class EditLoginPageViewModel : ReactiveObject, IRoutableViewModel
	{
		private int _selectionFolderIndex;

		#region Section's

		private bool VisiblePasswordSection { get; set; }
		private bool VisibleBankCardSection { get; set; }
		private bool VisiblePersonalDataSection { get; set; }
		private bool VisibleReminderSection { get; set; }
		private bool VisibleDateChanged { get; set; }

		#endregion

		#region ReactiveCommand's

		private ReactiveCommand<Unit,Unit> SaveLoginCommand { get; }
		private ReactiveCommand<Unit,Unit> DeleteLoginCommand { get; }
		private ReactiveCommand<Unit, Unit> CancelCommand { get; }
		private ReactiveCommand<Unit, Unit> AddCustomElementCommand { get; }

		#endregion

		#region Property's

		public int Type { get; set; }
		public ObservableCollection<CustomElementListItem> CustomElements { get; set; }
		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}

		private CustomFolder SelectionFolderItem { get { try { return Folders[SelectionFolderIndex]; } catch { return null; } } }
		private ObservableCollection<CustomFolder> Folders { get; set; }
		
		public LoginListItem LoginList;

		#endregion

		public Action<LoginListItem> EditCompleteCallback;
		public Action<LoginListItem> CancelCallback;
		public Action DeleteLoginCallback;

		private Login NewLogin { get; set; }

		// routing
		public string UrlPathSegment => "/editLogin";

		public IScreen HostScreen { get; }

		public EditLoginPageViewModel(LoginListItem acc, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();


			CustomElements = new ObservableCollection<CustomElementListItem>();

			LoginList = acc;

			NewLogin = new Login
			{
				Name = acc.LoginItem.Name,
				Username = acc.LoginItem.Username,
				Note = acc.LoginItem.Note,
				Type = acc.LoginItem.Type,
				TimeCreate = acc.LoginItem.TimeCreate
			};

			switch (acc.LoginItem.Type)
			{
				case 0:
					VisiblePasswordSection = true;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;

					NewLogin.Password = acc.LoginItem.Password;
					NewLogin.WebSite = acc.LoginItem.WebSite;
					break;
				case 1:
					VisiblePasswordSection = false;
					VisibleBankCardSection = true;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;

					NewLogin.TypeBankCard = acc.LoginItem.TypeBankCard;
					NewLogin.DateCard = acc.LoginItem.DateCard;
					NewLogin.SecurityCode = acc.LoginItem.SecurityCode;
					break;
				case 2:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = true;
					VisibleReminderSection = false;

					NewLogin.PersonalDataNumber = acc.LoginItem.PersonalDataNumber;
					NewLogin.PersonalDataPlaceOfIssue = acc.LoginItem.PersonalDataPlaceOfIssue;
					break;
				case 3:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = true;

					NewLogin.IsReminderActive = acc.LoginItem.IsReminderActive;
					break;
			}

			SaveLoginCommand = ReactiveCommand.Create(SaveLogin);
			CancelCommand = ReactiveCommand.Create(BackVoid);
			DeleteLoginCommand = ReactiveCommand.Create(DeleteLogin);
			AddCustomElementCommand = ReactiveCommand.Create(AddCustomElement);

			Folders = new ObservableCollection<CustomFolder>
			{
				new CustomFolder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
			};
			if (App.Database.CustomFolders != null)
				foreach (CustomFolder i in App.Database.CustomFolders) Folders.Add(i);

			SelectionFolderIndex = 0;
			foreach (CustomFolder i in Folders.Where(i => i.ID == LoginList.LoginItem.FolderID))
			{
				SelectionFolderIndex = Folders.IndexOf(i);
				break;
			}

			foreach (CustomElement i in acc.LoginItem.CustomElements)
			{
				CustomElements.Add(new CustomElementListItem(new Housing
				{
					CustomElement = i,
					IsEnabled = true
				}){
					ID = Guid.NewGuid().ToString("N"),
					DeleteCustomElement = DeleteCustomElement
				});
			}
		}

		private void SaveLogin()
		{
			LoginList.LoginItem = NewLogin;
			LoginList.LoginItem.FolderID = SelectionFolderItem.ID;

			LoginList.LoginItem.CustomElements = CustomElements.Select(item => item.HousingElement.CustomElement).ToList();
			LoginList.LoginItem.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			if (NewLogin.IsReminderActive)
			{
				LoginList.ReminderTimer.Interval = new TimeSpan(0, 0, 3);
				LoginList.ReminderTimer.Start();
			}
			else
			{
				if (LoginList.ReminderTimer != null)
					LoginList.ReminderTimer.Stop();
			}
			LoginList.EditedLogin();

			EditCompleteCallback?.Invoke(LoginList);
			App.MainWindow.MessageStatusBar("Not2");
		}
		private async void DeleteLogin()
		{
			MessageBox.MessageBoxResult r = await MessageBox.Show(App.MainWindow, null,
				(string)Application.Current.FindResource("MB2"), (string)Application.Current.FindResource("Message"),
				MessageBox.MessageBoxButtons.YesNo, MessageBox.MessageBoxIcon.Question);
			if (r == MessageBox.MessageBoxResult.Yes)
				DeleteLoginCallback?.Invoke();
		}
		private void AddCustomElement()
		{
			CustomElements.Add(new CustomElementListItem(new Housing
			{
				CustomElement = new CustomElement { Type = Type },
				IsEnabled = true
			})
			{
				ID = Guid.NewGuid().ToString("N"),
				DeleteCustomElement = DeleteCustomElement
			});
		}
		private void DeleteCustomElement(string id)
		{
			foreach (CustomElementListItem item in CustomElements.Where(item => item.ID == id))
			{
				CustomElements.Remove(item);
				break;
			}
		}
		private void BackVoid() => CancelCallback?.Invoke(LoginList);
	}
}