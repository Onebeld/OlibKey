using Avalonia;
using Avalonia.Controls;
using OlibKey.Structures;
using OlibKey.Views.Controls;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;

namespace OlibKey.ViewModels.Pages
{
	public class LoginInformationPageViewModel : ReactiveObject, IRoutableViewModel
	{
		private int _selectionFolderIndex;
		private ObservableCollection<CustomFieldListItem> _CustomFields;

		#region Section's

		private bool VisiblePasswordSection { get; set; }
		private bool VisibleBankCardSection { get; set; }
		private bool VisiblePersonalDataSection { get; set; }
		private bool VisibleReminderSection { get; set; }

		#endregion

		#region ReactiveCommand's

		public ReactiveCommand<Unit, Unit> EditContentCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyUsernameCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPasswordCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyWebSiteCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyTypeBankCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyDateCardCommand { get; }
		public ReactiveCommand<Unit, Unit> CopySecurityCodeCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyNumberCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPlaceOfIssueCommand { get; }
		public ReactiveCommand<Unit, Unit> OpenWebSiteCommand { get; }
		public ReactiveCommand<Unit, Unit> CopySocialSecurityNumberCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyTINCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyTelephoneCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyCompanyCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPostcodeCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyCountryCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyRegionCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyCityCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyAddressCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyEmailCommand { get; }

		#endregion

		#region Properties
		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}
		private ObservableCollection<CustomFieldListItem> CustomFields
		{
			get => _CustomFields;
			set => this.RaiseAndSetIfChanged(ref _CustomFields, value);
		}
		private LoginListItem LoginItem { get; set; }
		private bool VisibleDateChanged { get; set; }
		private bool IsVisible { get; set; } = true;
		private ObservableCollection<Folder> Folders { get; set; }
		public Login LoginInformation { get; set; }

		#endregion

		// routing
		public string UrlPathSegment => "/informationLogin";
		public IScreen HostScreen { get; }

		public Action<LoginListItem> EditContentCallback { get; set; }

		public LoginInformationPageViewModel(LoginListItem acc, Database db, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			LoginInformation = acc.LoginItem;
			LoginItem = acc;
			CustomFields = new ObservableCollection<CustomFieldListItem>();

			switch (LoginInformation.Type)
			{
				case 0:
					VisiblePasswordSection = true;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;
					break;
				case 1:
					VisiblePasswordSection = false;
					VisibleBankCardSection = true;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;
					break;
				case 2:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = true;
					VisibleReminderSection = false;
					break;
				case 3:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = true;
					break;
				case 4:
					VisiblePasswordSection = false;
					VisibleBankCardSection = false;
					VisiblePersonalDataSection = false;
					VisibleReminderSection = false;
					break;
			}

			if (!string.IsNullOrEmpty(LoginInformation.TimeChanged)) VisibleDateChanged = true;

			EditContentCommand = ReactiveCommand.Create(() => EditContentCallback?.Invoke(LoginItem));

			CopyUsernameCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Username); });
			CopyPasswordCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Password); });
			CopyWebSiteCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.WebSite); });
			CopyTypeBankCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.TypeBankCard); });
			CopyDateCardCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.DateCard); });
			CopySecurityCodeCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.SecurityCode); });
			CopyNumberCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Number); });
			CopyPlaceOfIssueCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.PlaceOfIssue); });
			CopySocialSecurityNumberCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.SocialSecurityNumber); });
			CopyTINCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.TIN); });
			CopyTelephoneCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Telephone); });
			CopyCompanyCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Company); });
			CopyPostcodeCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Postcode); });
			CopyCountryCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Country); });
			CopyRegionCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Region); });
			CopyCityCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.City); });
			CopyAddressCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Address); });
			CopyEmailCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Email); });

			OpenWebSiteCommand = ReactiveCommand.Create(() =>
			{
				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = "http://" + LoginInformation.WebSite,
					UseShellExecute = true
				};
				Process.Start(psi);
			});

			Folders = new ObservableCollection<Folder>
			{
				new Folder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
			};

			if (db.Folders != null) foreach (Folder i in db.Folders) Folders.Add(i);

			SelectionFolderIndex = 0;
			foreach (Folder i in Folders.Where(i => i.ID == LoginInformation.FolderID))
			{
				SelectionFolderIndex = Folders.IndexOf(i);
				break;
			}

			foreach (CustomField i in LoginInformation.CustomFields)
			{
				CustomFields.Add(new CustomFieldListItem(new Housing
				{
					CustomField = i,
					IsEnabled = false
				}));
			}

			if (CustomFields.Count != 0) CustomFields[^1].SLine.IsVisible = false;

			if (CustomFields.Count == 0) IsVisible = false;
		}
	}
}
