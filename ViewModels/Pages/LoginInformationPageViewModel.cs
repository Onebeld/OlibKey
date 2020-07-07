using System;
using System.Diagnostics;
using System.Reactive;
using Avalonia;
using OlibKey.Views.Controls;
using OlibKey.Structures;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace OlibKey.ViewModels.Pages
{
	public class LoginInformationPageViewModel : ReactiveObject, IRoutableViewModel
	{
		public string UrlPathSegment => "/informationLogin";

		public ReactiveCommand<Unit, Unit> EditContentCommand { get; }

		public ReactiveCommand<Unit, Unit> CopyUsernameCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPasswordCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyWebSiteCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyTypeBankCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyDateCardCommand { get; }
		public ReactiveCommand<Unit, Unit> CopySecurityCodeCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPersonalDataNumberCommand { get; }
		public ReactiveCommand<Unit, Unit> CopyPersonalDataPlaceOfIssue { get; }

		public ReactiveCommand<Unit, Unit> OpenWebSiteCommand { get; }

		private int _selectionFolderIndex;
		private bool IsVisible { get; set; } = true;

		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}

		private ObservableCollection<CustomElementListItem> customElements;
		public ObservableCollection<CustomElementListItem> CustomElements
		{
			get => customElements;
			set => this.RaiseAndSetIfChanged(ref customElements, value);
		}

		public IScreen HostScreen { get; }

		public Account InfAccount { get; set; }

		public AccountListItem AccountItem { get; set; }

		public Action<AccountListItem> EditContentCallback;

		private bool VisiblePasswordSection { get; set; }
		private bool VisibleBankCardSection { get; set; }
		private bool VisiblePersonalDataSection { get; set; }
		private bool VisibleReminderSection { get; set; }
		private bool VisibleDateChanged { get; set; }

		private ObservableCollection<CustomFolder> Folders { get; set; }

		public LoginInformationPageViewModel(AccountListItem acc, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			InfAccount = acc.AccountItem;
			AccountItem = acc;
			CustomElements = new ObservableCollection<CustomElementListItem>();

			switch (InfAccount.TypeAccount)
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

			if (!string.IsNullOrEmpty(InfAccount.TimeChanged)) VisibleDateChanged = true;

			EditContentCommand = ReactiveCommand.Create(() => EditContentCallback?.Invoke(AccountItem));

			CopyUsernameCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.Username); });
			CopyPasswordCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.Password); });
			CopyWebSiteCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.WebSite); });
			CopyTypeBankCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.TypeBankCard); });
			CopyDateCardCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.DateCard); });
			CopySecurityCodeCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.SecurityCode); });
			CopyPersonalDataNumberCommand = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.PersonalDataPlaceOfIssue); });
			CopyPersonalDataPlaceOfIssue = ReactiveCommand.Create(() => { Application.Current.Clipboard.SetTextAsync(InfAccount.PersonalDataPlaceOfIssue); });

			OpenWebSiteCommand = ReactiveCommand.Create(() =>
			{
				var psi = new ProcessStartInfo
				{
					FileName = "http://" + InfAccount.WebSite,
					UseShellExecute = true
				};
				Process.Start(psi);
			});

			Folders = new ObservableCollection<CustomFolder>();
			Folders.Add(new CustomFolder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") });
			if (App.Database.CustomFolders != null)
				foreach (var i in App.Database.CustomFolders) Folders.Add(i);

			SelectionFolderIndex = 0;

			foreach (var i in Folders)
			{
				if (i.ID == InfAccount.IDFolder)
				{
					SelectionFolderIndex = Folders.IndexOf(i);
					break;
				}
			}

			foreach (var i in InfAccount.CustomElements)
			{
				CustomElements.Add(new CustomElementListItem(new Housing
				{
					CustomElement = i,
					IsEnabled = false
				}));
			}
			if (CustomElements.Count == 0)
			{
				IsVisible = false;
			}
		}
	}
}
