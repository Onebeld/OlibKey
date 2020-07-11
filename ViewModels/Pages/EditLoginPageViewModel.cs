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
		private ObservableCollection<CustomElementListItem> _customElements;

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

		#region LoginInfo

		public string Name { get; set; }
		public string Username { get; set; }
		public string TimeCreate { get; set; }
		public string TimeChanged { get; set; }

		#region Login
		public string Password { get; set; }
		public string WebSite { get; set; }
		#endregion

		#region BankCart
		public string TypeBankCard { get; set; }
		public string DateCard { get; set; }
		public string SecurityCode { get; set; }
		#endregion

		#region Pasport
		public string PersonalDataNumber { get; set; }
		public string PersonalDataPlaceOfIssue { get; set; }
		#endregion

		#region Reminder
		public bool IsReminderActive { get; set; }
		#endregion
		public string Note { get; set; }

		#endregion

		#region Property's

		public int Type { get; set; }
		public ObservableCollection<CustomElementListItem> CustomElements
		{
			get => _customElements;
			set => this.RaiseAndSetIfChanged(ref _customElements, value);
		}
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

		// routing
		public string UrlPathSegment => "/editLogin";

		public IScreen HostScreen { get; }

		public EditLoginPageViewModel(LoginListItem acc, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			CustomElements = new ObservableCollection<CustomElementListItem>();

			LoginList = acc;

			Name = acc.LoginItem.Name;
			Username = acc.LoginItem.Username;
			Note = acc.LoginItem.Note;

			switch (acc.LoginItem.Type)
			{
				case 0:
					VisiblePasswordSection = true;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;

					Password = acc.LoginItem.Password;
					WebSite = acc.LoginItem.WebSite;
					break;
				case 1:
					VisiblePasswordSection = false;
					VisibleBankCardSection = true;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;

					TypeBankCard = acc.LoginItem.TypeBankCard;
					DateCard = acc.LoginItem.DateCard;
					SecurityCode = acc.LoginItem.SecurityCode;
					break;
				case 2:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = true;
					VisibleReminderSection = false;

					PersonalDataNumber = acc.LoginItem.PersonalDataNumber;
					PersonalDataPlaceOfIssue = acc.LoginItem.PersonalDataPlaceOfIssue;
					break;
				case 3:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = true;

					IsReminderActive = acc.LoginItem.IsReminderActive;
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
			LoginList.LoginItem.Name = Name;
			LoginList.LoginItem.Username = Username;
			LoginList.LoginItem.DateCard = DateCard;
			LoginList.LoginItem.FolderID = SelectionFolderItem.ID;
			LoginList.LoginItem.IsReminderActive = IsReminderActive;
			LoginList.LoginItem.Note = Note;
			LoginList.LoginItem.PersonalDataNumber = PersonalDataNumber;
			LoginList.LoginItem.PersonalDataPlaceOfIssue = PersonalDataPlaceOfIssue;
			LoginList.LoginItem.Password = Password;
			LoginList.LoginItem.SecurityCode = SecurityCode;
			LoginList.LoginItem.WebSite = WebSite;

			LoginList.LoginItem.CustomElements = CustomElements.Select(item => item.HousingElement.CustomElement).ToList();
			LoginList.LoginItem.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			if (IsReminderActive)
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