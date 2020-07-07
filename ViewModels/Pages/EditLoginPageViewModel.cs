﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using OlibKey.Views.Controls;
using OlibKey.Structures;
using OlibKey.Views.Windows;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;

namespace OlibKey.ViewModels.Pages
{
	public class EditLoginPageViewModel : ReactiveObject, IRoutableViewModel
	{
		private bool VisiblePasswordSection { get; set; }
		private bool VisibleBankCardSection { get; set; }
		private bool VisiblePersonalDataSection { get; set; }
		private bool VisibleReminderSection { get; set; }
		private bool VisibleDateChanged { get; set; }

		private ReactiveCommand<Unit,Unit> SaveAccountCommand { get; }
		private ReactiveCommand<Unit,Unit> DeleteAccountCommand { get; }
		private ReactiveCommand<Unit, Unit> CancelCommand { get; }
		public ReactiveCommand<Unit, Unit> AddCustomElementCommand { get; }

		private ObservableCollection<CustomElementListItem> customElements;
		public ObservableCollection<CustomElementListItem> CustomElements
		{
			get => customElements;
			set => this.RaiseAndSetIfChanged(ref customElements, value);
		}

		public int Type { get; set; }

		#region AccountInfo
		public string AccountName { get; set; }
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

		private int _selectionFolderIndex;

		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}
		private CustomFolder SelectionFolderItem { get { try { return Folders[SelectionFolderIndex]; } catch { return null; } } }
		private ObservableCollection<CustomFolder> Folders { get; set; }

		public Action<AccountListItem> EditCompleteCallback;
		public Action<AccountListItem> CancelCallback;
		public Action DeleteAccountCallback;

		public AccountListItem AccountListI;

		public string UrlPathSegment => "/editLogin";

		public IScreen HostScreen { get; }

		public EditLoginPageViewModel(AccountListItem acc, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			CustomElements = new ObservableCollection<CustomElementListItem>();

			AccountListI = acc;

			AccountName = acc.AccountItem.AccountName;
			Username = acc.AccountItem.Username;
			Note = acc.AccountItem.Note;

			switch (acc.AccountItem.TypeAccount)
			{
				case 0:
					VisiblePasswordSection = true;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;

					Password = acc.AccountItem.Password;
					WebSite = acc.AccountItem.WebSite;
					break;
				case 1:
					VisiblePasswordSection = false;
					VisibleBankCardSection = true;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;

					TypeBankCard = acc.AccountItem.TypeBankCard;
					DateCard = acc.AccountItem.DateCard;
					SecurityCode = acc.AccountItem.SecurityCode;
					break;
				case 2:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = true;
					VisibleReminderSection = false;

					PersonalDataNumber = acc.AccountItem.PersonalDataNumber;
					PersonalDataPlaceOfIssue = acc.AccountItem.PersonalDataPlaceOfIssue;
					break;
				case 3:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = true;

					IsReminderActive = acc.AccountItem.IsReminderActive;
					break;
			}

			SaveAccountCommand = ReactiveCommand.Create(SaveAccountVoid);
			CancelCommand = ReactiveCommand.Create(BackVoid);
			DeleteAccountCommand = ReactiveCommand.Create(DeleteAccountVoid);
			AddCustomElementCommand = ReactiveCommand.Create(AddCustomElement);

			Folders = new ObservableCollection<CustomFolder>();
			Folders.Add(new CustomFolder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") });
			if (App.Database.CustomFolders != null)
				foreach (var i in App.Database.CustomFolders) Folders.Add(i);

			SelectionFolderIndex = 0;

			foreach (var i in Folders)
			{
				if (i.ID == AccountListI.AccountItem.IDFolder)
				{
					SelectionFolderIndex = Folders.IndexOf(i);
					break;
				}
			}

			foreach (var i in acc.AccountItem.CustomElements)
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

		private void SaveAccountVoid()
		{
			AccountListI.AccountItem.AccountName = AccountName;
			AccountListI.AccountItem.Username = Username;
			AccountListI.AccountItem.DateCard = DateCard;
			AccountListI.AccountItem.IDFolder = SelectionFolderItem.ID;
			AccountListI.AccountItem.IsReminderActive = IsReminderActive;
			AccountListI.AccountItem.Note = Note;
			AccountListI.AccountItem.PersonalDataNumber = PersonalDataNumber;
			AccountListI.AccountItem.PersonalDataPlaceOfIssue = PersonalDataPlaceOfIssue;
			AccountListI.AccountItem.Password = Password;
			AccountListI.AccountItem.SecurityCode = SecurityCode;
			AccountListI.AccountItem.WebSite = WebSite;

			AccountListI.AccountItem.CustomElements = new List<CustomElement>();

			foreach (var item in CustomElements)
			{
				AccountListI.AccountItem.CustomElements.Add(item.HousingElement.CustomElement);
			}
			AccountListI.AccountItem.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			if (IsReminderActive)
			{
				AccountListI.timer.Interval = new TimeSpan(0, 0, 3);
				AccountListI.timer.Start();
			}
			else
			{
				if (AccountListI.timer != null)
					AccountListI.timer.Stop();
			}
			AccountListI.EditedAccount();

			EditCompleteCallback?.Invoke(AccountListI);
			App.MainWindow.MessageStatusBar("Not2");
		}
		private void BackVoid() => CancelCallback?.Invoke(AccountListI);

		private async void DeleteAccountVoid()
		{
			var r = await MessageBox.Show(App.MainWindow, null,
				(string)Application.Current.FindResource("MB2"), (string)Application.Current.FindResource("Message"),
				MessageBox.MessageBoxButtons.YesNo, MessageBox.MessageBoxIcon.Question);
			if (r == MessageBox.MessageBoxResult.Yes)
			{
				DeleteAccountCallback?.Invoke();
			}
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
			foreach (var item in CustomElements)
			{
				if (item.ID == id)
				{
					CustomElements.Remove(item);
					break;
				}
			}
		}
	}
}