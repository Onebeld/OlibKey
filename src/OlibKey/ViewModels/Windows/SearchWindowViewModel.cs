using OlibKey.Structures;
using OlibKey.Views.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace OlibKey.ViewModels.Windows
{
    public class SearchWindowViewModel : ReactiveObject
    {
        private ObservableCollection<LoginListItem> _loginList = new ObservableCollection<LoginListItem>();
        private ObservableCollection<FolderListItem> _folderList = new ObservableCollection<FolderListItem>();

        private FolderListItem _selectedFolderItem;

        private string _searchText;

        public Action ChangeIndexCallback;

        private int _selectedLoginIndex;
        private int _selectedFolderIndex;

        #region ReactiveCommand's

        private ReactiveCommand<Unit, Unit> CreateFolderCommand { get; }
        private ReactiveCommand<Unit, Unit> DeleteFolderCommand { get; }
        private ReactiveCommand<Unit, Unit> EditFolderCommand { get; }
        private ReactiveCommand<Unit, Unit> UnselectFolderItemCommand { get; }

        #endregion

        #region Property's

        public ObservableCollection<LoginListItem> LoginList
        {
            get => _loginList; set => this.RaiseAndSetIfChanged(ref _loginList, value);
        }
        public ObservableCollection<FolderListItem> FolderList
        {
            get => _folderList; set => this.RaiseAndSetIfChanged(ref _folderList, value);
        }
        public int SelectedFolderIndex
        {
            get => _selectedFolderIndex; set
            {
                this.RaiseAndSetIfChanged(ref _selectedFolderIndex, value);
                ChangeIndexCallback?.Invoke();
            }
        }
        private int SelectedLoginIndex
        {
            get => _selectedLoginIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedLoginIndex, value);
                if (SelectedLoginIndex == -1) return;
                ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.SearchSelectLogin(SelectedLoginItem);
                App.SearchWindow.Close();
            }
        }
        public string SearchText
        {
            get => _searchText; 
            set
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                ClearListAndSearchElement();
            }
        }

        private bool Login { get; set; }
        private bool BankCard { get; set; }
        private bool PersonalData { get; set; }
        private bool Reminder { get; set; }
        private bool Notes { get; set; }

        private bool ActiveReminder { get; set; }
        private bool Favorite { get; set; }
        private bool SortAlphabetically { get; set; }
        #endregion

        public SearchWindowViewModel()
        {
            CreateFolderCommand = ReactiveCommand.Create(CreateFolder);
            DeleteFolderCommand = ReactiveCommand.Create(() =>
            {
                if (((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.UseTrash)
                {
                    if (((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.Database.Trash == null)
                    {
                        ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.Database.Trash = new Trash
                        {
                            Logins = new List<Login>(),
                            Folders = new List<Folder>()
                        };
                    }
                    SelectedFolderItem.FolderContext.DeleteDate = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
                    ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.Database.Trash.Folders.Add(SelectedFolderItem.FolderContext);
                }
                FolderList.Remove(SelectedFolderItem);
            });
            EditFolderCommand = ReactiveCommand.Create(() => SelectedFolderItem.Focusing());
            UnselectFolderItemCommand = ReactiveCommand.Create(() => { SelectedFolderIndex = -1; });

            SelectedLoginIndex = SelectedFolderIndex = -1;

            ClearListAndSearchElement();
        }

        public FolderListItem SelectedFolderItem
        {
            get => _selectedFolderItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedFolderItem, value);
                ClearListAndSearchElement();
            }
        }

        private LoginListItem SelectedLoginItem { get { try { return LoginList[SelectedLoginIndex]; } catch { return null; } } }
        private void CreateFolder() =>
            FolderList.Add(new FolderListItem(new Folder())
            {
                FolderContext = { ID = Guid.NewGuid().ToString("N") }
            });
        public void AddFolder(Folder loginContent) => FolderList.Add(new FolderListItem(loginContent) { tbName = { IsVisible = false } });

        public void ClearListAndSearchElement(bool isChecked = true)
        {
            if (!isChecked) return;
            LoginList.Clear();

            List<LoginListItem> selectedItemList = Login
                ? ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 0)
                : BankCard
                ? ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 1)
                : PersonalData
                ? ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 2)
                : Reminder
                ? ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 3)
                : Notes
                ? ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 4)
                : ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.LoginList.ToList();

            if (SelectedFolderItem != null) selectedItemList = selectedItemList.FindAll(x => x.LoginItem.FolderID == ((Folder)SelectedFolderItem.DataContext)?.ID);

            if (ActiveReminder) selectedItemList = selectedItemList.FindAll(x => x.LoginItem.IsReminderActive);

            if (Favorite) selectedItemList = selectedItemList.FindAll(x => x.LoginItem.Favorite);

            if (!string.IsNullOrEmpty(SearchText)) selectedItemList = selectedItemList.FindAll(x => x.LoginItem.Name.ToLower().Contains(SearchText.ToLower()));

            if (SortAlphabetically) selectedItemList = selectedItemList.OrderBy(x => x.LoginItem.Name).ToList();

            for (int i = 0; i < selectedItemList.Count; i++)
                LoginList.Add(new LoginListItem(selectedItemList[i].LoginItem)
                {
                    LoginID = selectedItemList[i].LoginID,
                    IconLogin = { Source = selectedItemList[i].IconLogin.Source },
                    IsFavorite = { IsEnabled = false }
                });
        }
        private void CheckedButton() => ClearListAndSearchElement();
    }
}
