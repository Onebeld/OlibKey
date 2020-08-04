using Avalonia;
using Avalonia.Controls;
using OlibKey.Structures;
using OlibKey.Views.Controls;
using OlibKey.Views.Windows;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;

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

		private ReactiveCommand<Unit, Unit> SaveLoginCommand { get; }
		private ReactiveCommand<Unit, Unit> DeleteLoginCommand { get; }
		private ReactiveCommand<Unit, Unit> CancelCommand { get; }
		private ReactiveCommand<Unit, Unit> AddCustomFieldCommand { get; }

		#endregion

		#region Property's

		public int Type { get; set; }
		public ObservableCollection<CustomFieldListItem> CustomFields { get; set; }
		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}

		private Folder SelectionFolderItem { get { try { return Folders[SelectionFolderIndex]; } catch { return null; } } }
		private ObservableCollection<Folder> Folders { get; set; }

		public LoginListItem LoginList;

		#endregion

		public Action<LoginListItem> EditCompleteCallback;
		public Action<LoginListItem> CancelCallback;
		public Action DeleteLoginCallback;

		private Login NewLogin { get; set; }

		// routing
		public string UrlPathSegment => "/editLogin";

		public IScreen HostScreen { get; }

		public EditLoginPageViewModel(LoginListItem acc, Database db, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();


			CustomFields = new ObservableCollection<CustomFieldListItem>();

			LoginList = acc;

			NewLogin = new Login
			{
				Name = acc.LoginItem.Name,
				Username = acc.LoginItem.Username,
				Email = acc.LoginItem.Email,
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

					NewLogin.Number = acc.LoginItem.Number;
					NewLogin.PlaceOfIssue = acc.LoginItem.PlaceOfIssue;
					NewLogin.SocialSecurityNumber = acc.LoginItem.SocialSecurityNumber;
					NewLogin.TIN = acc.LoginItem.TIN;
					NewLogin.Telephone = acc.LoginItem.Telephone;
					NewLogin.Company = acc.LoginItem.Company;
					NewLogin.Postcode = acc.LoginItem.Postcode;
					NewLogin.Country = acc.LoginItem.Country;
					NewLogin.Region = acc.LoginItem.Region;
					NewLogin.City = acc.LoginItem.City;
					NewLogin.Address = acc.LoginItem.Address;
					break;
				case 3:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = true;

					NewLogin.IsReminderActive = acc.LoginItem.IsReminderActive;
					break;
				case 4:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;
					break;
			}

			SaveLoginCommand = ReactiveCommand.Create(SaveLogin);
			CancelCommand = ReactiveCommand.Create(BackVoid);
			DeleteLoginCommand = ReactiveCommand.Create(DeleteLogin);
			AddCustomFieldCommand = ReactiveCommand.Create(AddCustomField);

			Folders = new ObservableCollection<Folder>
			{
				new Folder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
			};

			if (db.Folders != null) foreach (Folder i in db.Folders) Folders.Add(i);

			SelectionFolderIndex = 0;
			foreach (Folder i in Folders.Where(i => i.ID == LoginList.LoginItem.FolderID))
			{
				SelectionFolderIndex = Folders.IndexOf(i);
				break;
			}

			foreach (CustomField i in acc.LoginItem.CustomFields)
			{
				CustomFields.Add(new CustomFieldListItem(new Housing
				{
					CustomField = i,
					IsEnabled = true
				})
				{
					ID = Guid.NewGuid().ToString("N"),
					DeleteCustomField = DeleteCustomField
				});
			}
		}

		private void SaveLogin()
		{
			LoginList.LoginItem = NewLogin;
			LoginList.LoginItem.FolderID = SelectionFolderItem.ID;

			LoginList.LoginItem.CustomFields = CustomFields.Select(item => item.HousingElement.CustomField).ToList();
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
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not2"));
		}
		private async void DeleteLogin()
		{
			MessageBox.MessageBoxResult r = await MessageBox.Show(App.MainWindow, null,
				(string)Application.Current.FindResource("MB2"), (string)Application.Current.FindResource("Message"),
				MessageBox.MessageBoxButtons.YesNo, MessageBox.MessageBoxIcon.Question);
			if (r == MessageBox.MessageBoxResult.Yes)
				DeleteLoginCallback?.Invoke();
		}
		private void AddCustomField()
		{
			CustomFields.Add(new CustomFieldListItem(new Housing
			{
				CustomField = new CustomField { Type = Type },
				IsEnabled = true
			})
			{
				ID = Guid.NewGuid().ToString("N"),
				DeleteCustomField = DeleteCustomField
			});
		}
		private void DeleteCustomField(string id)
		{
			foreach (CustomFieldListItem item in CustomFields.Where(item => item.ID == id))
			{
				CustomFields.Remove(item);
				break;
			}
		}
		private void BackVoid() => CancelCallback?.Invoke(LoginList);
	}
}