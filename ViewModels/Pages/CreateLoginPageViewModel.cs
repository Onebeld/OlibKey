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
		public ReactiveCommand<Unit, Unit> AddCustomElementCommand { get; }
		#endregion

		private int _selectionFolderIndex;
		private ObservableCollection<CustomElementListItem> _customElements;

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
		public ObservableCollection<CustomElementListItem> CustomElements
		{
			get => _customElements;
			set => this.RaiseAndSetIfChanged(ref _customElements, value);
		}

		#endregion

		public Action BackPageCallback { get; set; }
		public Action<Login> CreateLoginCallback { get; set; }

		// routing
		public string UrlPathSegment => "/addLogin";
		public IScreen HostScreen { get; }


		public CreateLoginPageViewModel(IScreen screen = null)
		{
			HostScreen = screen ?? Locator.Current.GetService<IScreen>();

			NewLogin = new Login
			{
				CustomElements = new System.Collections.Generic.List<CustomElement>()
			};
			CustomElements = new ObservableCollection<CustomElementListItem>();

			BackCommand = ReactiveCommand.Create(BackVoid);
			CreateLoginCommand = ReactiveCommand.Create(CreateLogin);
			AddCustomElementCommand = ReactiveCommand.Create(AddCustomElement);

			Folders = new ObservableCollection<Folder>
			{
				new Folder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
			};
			if (App.Database.Folders != null)
				foreach (Folder i in App.Database.Folders) Folders.Add(i);

			SelectionFolderIndex = 0;
			Type = 0;
		}

		private void CreateLogin()
		{
			NewLogin.CustomElements.AddRange(CustomElements.Select(item => item.HousingElement.CustomElement));
			NewLogin.TimeCreate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			CreateLoginCallback?.Invoke(NewLogin);
			App.MainWindow.MessageStatusBar("Not1");
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
				_ = CustomElements.Remove(item);
				break;
			}
		}
		private void BackVoid() => BackPageCallback?.Invoke();
	}
}