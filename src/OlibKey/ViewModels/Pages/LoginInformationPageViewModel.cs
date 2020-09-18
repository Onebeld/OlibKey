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
		private ObservableCollection<CustomFieldListItem> _customFields;
		private ObservableCollection<ImportedFileListItem> _importedFiles;

		#region Section's

		private bool VisiblePasswordSection { get; set; }
		private bool VisibleBankCardSection { get; set; }
		private bool VisiblePersonalDataSection { get; set; }
		private bool VisibleReminderSection { get; set; }

		#endregion

		#region ReactiveCommand's

		private ReactiveCommand<Unit, Unit> EditContentCommand { get; }
		private ReactiveCommand<Unit, Unit> OpenWebSiteCommand { get; }

		#endregion

		#region Properties
		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
		}
		private ObservableCollection<CustomFieldListItem> CustomFields
		{
			get => _customFields;
			set => this.RaiseAndSetIfChanged(ref _customFields, value);
		}
		private ObservableCollection<ImportedFileListItem> ImportedFiles
        {
			get => _importedFiles;
			set => this.RaiseAndSetIfChanged(ref _importedFiles, value);
        }
		private LoginListItem LoginItem { get; set; }
		private bool VisibleDateChanged { get; set; }
		private bool IsVisibleCustomFields { get; set; } = true;
		private bool IsVisibleImportedFiles { get; set; } = true;
		private ObservableCollection<Folder> Folders { get; set; }
		private Login LoginInformation { get; set; }

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
			ImportedFiles = new ObservableCollection<ImportedFileListItem>();

            switch (LoginInformation.Type)
			{
                case 0:
					VisiblePasswordSection = true;
					break;
				case 1:
					VisibleBankCardSection = true;
					break;
				case 2:
					VisiblePersonalDataSection = true;
					break;
				case 3:
					VisibleReminderSection = true;
					break;
				default: break;
			}

			if (!string.IsNullOrEmpty(LoginInformation.TimeChanged)) VisibleDateChanged = true;

			EditContentCommand = ReactiveCommand.Create(() => EditContentCallback?.Invoke(LoginItem));

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

			for (int index = 0; index < LoginInformation.CustomFields.Count; index++)
				CustomFields.Add(new CustomFieldListItem(new Housing
					{CustomField = LoginInformation.CustomFields[index], IsEnabled = false}));
            for (int i = 0; i < LoginInformation.ImportedFiles.Count; i++)
            {
				ImportedFiles.Add(new ImportedFileListItem
                {
					DataContext = LoginInformation.ImportedFiles[i],
					BDelete = { IsVisible = false }
                });
            }

			if (CustomFields.Count != 0) CustomFields[^1].SLine.IsVisible = false;

			if (CustomFields.Count == 0) IsVisibleCustomFields = false;

			if (ImportedFiles.Count == 0) IsVisibleImportedFiles = false;

			if (ImportedFiles.Count != 0) ImportedFiles[^1].SLine.IsVisible = false;
		}

		private void CopyInformation(string s)
        {
			Application.Current.Clipboard.SetTextAsync(s);
			if (Program.Settings.ClearingTheClipboard)
            {
				App.ClearingClipboard.Stop();
				App.ClearingClipboard.Interval = new TimeSpan(0, 0, Program.Settings.TimeToClearTheClipboard);
				App.ClearingClipboard.Start();
            }
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Copied"));
        }
	}
}
