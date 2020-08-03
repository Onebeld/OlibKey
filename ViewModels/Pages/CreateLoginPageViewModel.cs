using Avalonia;
using Avalonia.Controls;
using OlibKey.Structures;
using OlibKey.Views.Controls;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;

namespace OlibKey.ViewModels.Pages
{
	public class CreateLoginPageViewModel : ReactiveObject, IRoutableViewModel
	{
		#region ReactiveCommands
		public ReactiveCommand<Unit, Unit> BackCommand { get; }
		public ReactiveCommand<Unit, Unit> CreateLoginCommand { get; }
		public ReactiveCommand<Unit, Unit> AddCustomFieldCommand { get; }
		#endregion

		private int _selectionFolderIndex;
		private ObservableCollection<CustomFieldListItem> _CustomFields;

		#region Property's
		public int Type { get; set; }
		public Login NewLogin { get; set; }
		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set
			{
				this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
				NewLogin.FolderID = SelectionFolderItem.ID;
			}
		}
		private Folder SelectionFolderItem { get { try { return Folders[SelectionFolderIndex]; } catch { return null; } } }
		private ObservableCollection<Folder> Folders { get; set; }
		public ObservableCollection<CustomFieldListItem> CustomFields
		{
			get => _CustomFields;
			set => this.RaiseAndSetIfChanged(ref _CustomFields, value);
		}

		#endregion

		public Action BackPageCallback { get; set; }
		public Action<Login> CreateLoginCallback { get; set; }

		// routing
		public string UrlPathSegment => "/addLogin";
		public IScreen HostScreen { get; }


		public CreateLoginPageViewModel(Database db, IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			NewLogin = new Login
			{
				CustomFields = new System.Collections.Generic.List<CustomField>()
			};
			CustomFields = new ObservableCollection<CustomFieldListItem>();

			BackCommand = ReactiveCommand.Create(BackVoid);
			CreateLoginCommand = ReactiveCommand.Create(CreateLogin);
			AddCustomFieldCommand = ReactiveCommand.Create(AddCustomField);

			Folders = new ObservableCollection<Folder>
			{
				new Folder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
			};
			if (db.Folders != null)
				foreach (Folder i in db.Folders) Folders.Add(i);

			SelectionFolderIndex = 0;
			Type = 0;
		}

		private void CreateLogin()
		{
			NewLogin.CustomFields.AddRange(CustomFields.Select(item => item.HousingElement.CustomField));
			NewLogin.TimeCreate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			CreateLoginCallback?.Invoke(NewLogin);
			App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not1"));
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
				_ = CustomFields.Remove(item);
				break;
			}
		}
		private void BackVoid() => BackPageCallback?.Invoke();
	}
}