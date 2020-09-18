using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using OlibKey.Structures;
using OlibKey.Views.Controls;
using OlibKey.Views.Windows;
using OtpNet;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OlibKey.ViewModels.Pages
{
    public class EditLoginPageViewModel : ReactiveObject, IRoutableViewModel
    {
        private Totp Totp;

        private string _generatedCode;
        private int _timeLeft;

        private DispatcherTimer Timer;

        private int _selectionFolderIndex;

        #region Section's

        private bool VisiblePasswordSection { get; set; }
        private bool VisibleBankCardSection { get; set; }
        private bool VisiblePersonalDataSection { get; set; }
        private bool VisibleReminderSection { get; set; }
        private bool VisibleDateChanged { get; set; }

        #endregion

        #region Property's

        private int Type { get; set; }
        private ObservableCollection<CustomFieldListItem> CustomFields { get; set; }
        private ObservableCollection<ImportedFileListItem> ImportedFiles { get; set; }
        private int SelectionFolderIndex
        {
            get => _selectionFolderIndex;
            set => this.RaiseAndSetIfChanged(ref _selectionFolderIndex, value);
        }
        private string GeneratedCode
        {
            get => _generatedCode;
            set => this.RaiseAndSetIfChanged(ref _generatedCode, value);
        }
        private int TimeLeft
        {
            get => _timeLeft;
            set => this.RaiseAndSetIfChanged(ref _timeLeft, value);
        }

        private Folder SelectionFolderItem { get { try { return Folders[SelectionFolderIndex]; } catch { return null; } } }
        private ObservableCollection<Folder> Folders { get; set; }

        private LoginListItem LoginList;

        #endregion

        public Action<LoginListItem, bool> EditCompleteCallback;
        public Action<LoginListItem> CancelCallback;
        public Action DeleteLoginCallback;

        private Login NewLogin { get; set; }

        // routing
        public string UrlPathSegment => "/editLogin";

        public IScreen HostScreen { get; }

        public EditLoginPageViewModel(LoginListItem acc, Database db, IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            CustomFields = new ObservableCollection<CustomFieldListItem>();
            ImportedFiles = new ObservableCollection<ImportedFileListItem>();

            LoginList = acc;

            NewLogin = new Login
            {
                Name = acc.LoginItem.Name,
                Username = acc.LoginItem.Username,
                Email = acc.LoginItem.Email,
                Note = acc.LoginItem.Note,
                Type = acc.LoginItem.Type,
                TimeCreate = acc.LoginItem.TimeCreate,
                Color = acc.LoginItem.Color,
                UseColor = acc.LoginItem.UseColor
            };

            switch (acc.LoginItem.Type)
            {
                case 0:
                    VisiblePasswordSection = true;

                    NewLogin.Password = acc.LoginItem.Password;
                    NewLogin.WebSite = acc.LoginItem.WebSite;
                    NewLogin.SecretKey = acc.LoginItem.SecretKey;
                    break;
                case 1:
                    VisibleBankCardSection = true;

                    NewLogin.TypeBankCard = acc.LoginItem.TypeBankCard;
                    NewLogin.DateCard = acc.LoginItem.DateCard;
                    NewLogin.SecurityCode = acc.LoginItem.SecurityCode;
                    break;
                case 2:
                    VisiblePersonalDataSection = true;

                    NewLogin.Number = acc.LoginItem.Number;
                    NewLogin.PlaceOfIssue = acc.LoginItem.PlaceOfIssue;
                    NewLogin.SocialSecurityNumber = acc.LoginItem.SocialSecurityNumber;
                    NewLogin.TIN = acc.LoginItem.TIN;
                    NewLogin.Telephone = acc.LoginItem.Telephone;
                    NewLogin.Company = acc.LoginItem.Company;
                    NewLogin.Postcode = acc.LoginItem.Postcode;
                    NewLogin.Country = acc.LoginItem.Country;
                    NewLogin.Region = acc.LoginItem.Region;
                    NewLogin.City = acc.LoginItem.City;
                    NewLogin.Address = acc.LoginItem.Address;
                    break;
                case 3:
                    VisibleReminderSection = true;

                    NewLogin.IsReminderActive = acc.LoginItem.IsReminderActive;
                    break;
               default: break;
            }

            Folders = new ObservableCollection<Folder>
            {
                new Folder { ID = null, Name = (string)Application.Current.FindResource("NotChosen") }
            };

            if (db.Folders != null) foreach (Folder i in db.Folders) Folders.Add(i);

            SelectionFolderIndex = 0;
            foreach (Folder i in Folders.Where(i => i.ID == LoginList.LoginItem.FolderID))
            {
                SelectionFolderIndex = Folders.IndexOf(i);
                break;
            }

            for (int i = 0; i < acc.LoginItem.CustomFields.Count; i++)
            {
                CustomFields.Add(new CustomFieldListItem(new Housing
                {
                    CustomField = acc.LoginItem.CustomFields[i],
                    IsEnabled = true
                })
                {
                    ID = Guid.NewGuid().ToString("N"),
                    DeleteCustomField = DeleteCustomField
                });
            }
            for (int i = 0; i < acc.LoginItem.ImportedFiles.Count; i++)
            {
                ImportedFiles.Add(new ImportedFileListItem
                {
                    DataContext = acc.LoginItem.ImportedFiles[i],
                    ID = Guid.NewGuid().ToString("N"),
                    DeleteFileCallback = DeleteImportedFile
                });
            }

            try
            {
                Totp = new Totp(Base32Encoding.ToBytes(NewLogin.SecretKey.Replace(" ", "")), step: 30, timeCorrection: new TimeCorrection(DateTime.UtcNow));
                GeneratedCode = Totp.ComputeTotp();
                TimeLeft = Totp.RemainingSeconds();
                Timer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 50)
                };
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
            catch {}
        }

        private async void ImportFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog{ AllowMultiple = true };

                List<string> files = (await dialog.ShowAsync(App.MainWindow)).ToList();

                foreach (var file in files)
                {
                    ImportedFiles.Add(new ImportedFileListItem { 
                        DataContext = new ImportedFile { Name = Path.GetFileName(file) , Data = Core.FileInteractions.ImportFile(file) }, 
                        ID = Guid.NewGuid().ToString("N"),
                        DeleteFileCallback = DeleteImportedFile
                    });
                }

            }
            catch { }
        }
        private void SaveLogin()
        {
            bool changeWebSite = false;

            if (LoginList.LoginItem.WebSite != NewLogin.WebSite) changeWebSite = true;

            LoginList.LoginItem = NewLogin;
            LoginList.LoginItem.FolderID = SelectionFolderItem.ID;

            LoginList.LoginItem.CustomFields = CustomFields.Select(item => item.HousingElement.CustomField).ToList();
            LoginList.LoginItem.ImportedFiles = ImportedFiles.Select(item => item.DataContext as ImportedFile).ToList();
            LoginList.LoginItem.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            if (NewLogin.IsReminderActive)
            {
                LoginList.ReminderTimer.Interval = new TimeSpan(0, 0, 1);
                LoginList.ReminderTimer.Start();
            }
            else
                LoginList.ReminderTimer?.Stop();

            EditCompleteCallback?.Invoke(LoginList, changeWebSite);
            App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not2"));
        }
        private async void DeleteLogin()
        {
            if (App.MainWindowViewModel.SelectedTabItem.UseTrash)
            {
                if (App.MainWindowViewModel.SelectedTabItem.Database.Trash == null)
                {
                    App.MainWindowViewModel.SelectedTabItem.Database.Trash = new Trash
                    {
                        Logins = new List<Login>(),
                        Folders = new List<Folder>()
                    };
                }
                LoginList.LoginItem.DeleteDate = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
                App.MainWindowViewModel.SelectedTabItem.Database.Trash.Logins.Add(LoginList.LoginItem);
                DeleteLoginCallback?.Invoke();
            }
            else
            {
                if (await MessageBox.Show(App.MainWindow, null,
                    (string)Application.Current.FindResource("MB2"), (string)Application.Current.FindResource("Message"),
                    MessageBox.MessageBoxButtons.YesNo, MessageBox.MessageBoxIcon.Question) == MessageBox.MessageBoxResult.Yes)
                    DeleteLoginCallback?.Invoke();
            }
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
        private void DeleteImportedFile(string id)
        {
            foreach (ImportedFileListItem item in ImportedFiles.Where(item => item.ID == id))
            {
                ImportedFiles.Remove(item);
                break;
            }
        }
        private void Back() => CancelCallback?.Invoke(LoginList);
        private async void ActivateTOTP()
        {
            try
            {
                if (Timer != null && Timer.IsEnabled) Timer.Stop();

                Totp = new Totp(Base32Encoding.ToBytes(NewLogin.SecretKey.Replace(" ", "")), step: 30, timeCorrection: new TimeCorrection(DateTime.UtcNow));
                GeneratedCode = Totp.ComputeTotp();
                TimeLeft = Totp.RemainingSeconds();
                Timer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 50)
                };
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
            catch
            {
                await MessageBox.Show(App.MainWindow, null,
                    (string)Application.Current.FindResource("MB10"), (string)Application.Current.FindResource("Error"),
                    MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            GeneratedCode = Totp.ComputeTotp();
            TimeLeft = Totp.RemainingSeconds();
        }
    }
}