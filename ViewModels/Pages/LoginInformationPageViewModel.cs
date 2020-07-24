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
		private ObservableCollection<CustomElementListItem> _customElements;

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
		public ReactiveCommand<Unit, Unit> CopyPersonalDataNumberCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPersonalDataPlaceOfIssueCommand { get; }
		public ReactiveCommand<Unit, Unit> OpenWebSiteCommand { get; }

		#endregion

		#region Properties

		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}
		private ObservableCollection<CustomElementListItem> CustomElements
		{
			get => _customElements;
			set => this.RaiseAndSetIfChanged(ref _customElements, value);
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

		public LoginInformationPageViewModel(LoginListItem acc, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			LoginInformation = acc.LoginItem;
			LoginItem = acc;
			CustomElements = new ObservableCollection<CustomElementListItem>();

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
			}

			if (!string.IsNullOrEmpty(LoginInformation.TimeChanged)) VisibleDateChanged = true;

			EditContentCommand = ReactiveCommand.Create(() => EditContentCallback?.Invoke(LoginItem));

			CopyUsernameCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Username); });
			CopyPasswordCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.Password); });
			CopyWebSiteCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.WebSite); });
			CopyTypeBankCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.TypeBankCard); });
			CopyDateCardCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.DateCard); });
			CopySecurityCodeCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.SecurityCode); });
			CopyPersonalDataNumberCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.PersonalDataNumber); });
			CopyPersonalDataPlaceOfIssueCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(LoginInformation.PersonalDataPlaceOfIssue); });

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
			if (App.Database.Folders != null)
				foreach (Folder i in App.Database.Folders) Folders.Add(i);

			SelectionFolderIndex = 0;
			foreach (Folder i in Folders.Where(i => i.ID == LoginInformation.FolderID))
			{
				SelectionFolderIndex = Folders.IndexOf(i);
				break;
			}

			foreach (CustomElement i in LoginInformation.CustomElements)
			{
				CustomElements.Add(new CustomElementListItem(new Housing
				{
					CustomElement = i,
					IsEnabled = false
				}));
			}

			if (CustomElements.Count != 0)
				CustomElements[^1].SLine.IsVisible = false;

			if (CustomElements.Count == 0)
				IsVisible = false;
		}
	}
}
