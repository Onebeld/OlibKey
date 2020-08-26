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

namespace OlibKey.ViewModels.Pages
{
    public class CreateLoginPageViewModel : ReactiveObject, IRoutableViewModel
    {
        private int _selectionFolderIndex;
        private ObservableCollection<CustomFieldListItem> _customFields;

        #region Property's

        private int Type { get; set; }
        private Login NewLogin { get; set; }
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

        private ObservableCollection<CustomFieldListItem> CustomFields
        {
            get => _customFields;
            set => this.RaiseAndSetIfChanged(ref _customFields, value);
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

            Folders = new ObservableCollection<Folder>
            {
                new Folder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
            };

            if (db.Folders != null) foreach (Folder i in db.Folders) Folders.Add(i);

            SelectionFolderIndex = Type = 0;
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
                CustomFields.Remove(item);
                break;
            }
        }
        private void Back() => BackPageCallback?.Invoke();
    }
}