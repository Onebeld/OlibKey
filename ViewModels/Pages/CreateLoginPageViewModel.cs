using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using OlibKey.Structures;
using OlibKey.Views.Windows;
using ReactiveUI;
using Splat;

namespace OlibKey.ViewModels.Pages
{
    public class CreateLoginPageViewModel : ReactiveObject, IRoutableViewModel
    {
        #region ReactiveCommands
        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateAccountCommand { get; }
		#endregion
		private int _selectionFolderIndex;

		private int SelectionFolderIndex
		{
			get => _selectionFolderIndex;
			set
			{
				this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
				NewAccount.IDFolder = SelectionFolderItem.ID;
			}
		}
		private CustomFolder SelectionFolderItem { get { try { return Folders[SelectionFolderIndex]; } catch { return null; } } }
		private ObservableCollection<CustomFolder> Folders { get; set; }

		public Action BackPageCallback { get; set; }
        public Action<Account> AddAccountCallback { get; set; }


        public string UrlPathSegment => "/addLogin";
        public IScreen HostScreen { get; }

        public Account NewAccount { get; set; }

        public CreateLoginPageViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            NewAccount = new Account();

            BackCommand = ReactiveCommand.Create(BackVoid);
            CreateAccountCommand = ReactiveCommand.Create(CreateAccountVoid);

			Folders = new ObservableCollection<CustomFolder>();
			Folders.Add(new CustomFolder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") });
			if (App.Database.CustomFolders != null)
				foreach (var i in App.Database.CustomFolders) Folders.Add(i);

			SelectionFolderIndex = 0;
        }

        #region Voids
        private void BackVoid() => BackPageCallback?.Invoke();
        private void CreateAccountVoid()
        {
	        NewAccount.TimeCreate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            AddAccountCallback?.Invoke(NewAccount);
        }
        #endregion
    }
}
