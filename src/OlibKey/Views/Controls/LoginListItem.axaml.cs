using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using OlibKey.Structures;
using OlibKey.Views.Windows;
using System;
using Avalonia.Controls.Primitives;
using System.IO;
using Avalonia.Media.Imaging;
using OlibKey.Core;
using OlibKey.Controls.ColorPicker;

namespace OlibKey.Views.Controls
{
    public class LoginListItem : UserControl
    {
        public Image IconLogin;
        public CheckBox SelectedItem;
        public ToggleButton IsFavorite;
        private readonly TextBlock _tbLoginName;
        private readonly TextBlock _tbUsername;
        private TextBlock _tbDeleteDate;

        private Border _bLabelColor;

        public string LoginID { get; set; }

        public DispatcherTimer ReminderTimer;

        public Login LoginItem { get; set; }

        public LoginListItem() => InitializeComponent();

        public LoginListItem(Login Login)
        {
            InitializeComponent();

            IconLogin = this.FindControl<Image>("imageIconWebSite");
            _tbLoginName = this.FindControl<TextBlock>("tbLoginName");
            _tbUsername = this.FindControl<TextBlock>("tbUsername");
            SelectedItem = this.FindControl<CheckBox>("selectedItem");
            IsFavorite = this.FindControl<ToggleButton>("isFavorite");
            _tbDeleteDate = this.FindControl<TextBlock>("tbDeleteDate");
            _bLabelColor = this.FindControl<Border>("bLabelColor");

            LoginItem = Login;

            EditedLogin();

            switch (LoginItem.Type)
            {
                case 3:
                    ReminderTimer = new DispatcherTimer
                    {
                        Interval = new TimeSpan(0, 0, 3)
                    };
                    ReminderTimer.Tick += ReminderTimer_Tick;

                    ReminderTimer.Start();
                    break;
            }

            _tbUsername.Text = LoginItem.Type switch
            {
                0 when string.IsNullOrEmpty(Login.Username) => Login.Email,
                4 => LoginItem.TimeCreate,
                _ => _tbUsername.Text
            };

            if (!string.IsNullOrEmpty(LoginItem.DeleteDate))
            {
                _tbDeleteDate.IsVisible = true;
                _tbDeleteDate.Text = $"{(string)Application.Current.FindResource("Removed")} {LoginItem.DeleteDate}";
                Height = 60;
            }

            IsFavorite.IsChecked = LoginItem.Favorite;

            IsFavorite.Checked += IsFavoriteChecking;
            IsFavorite.Unchecked += IsFavoriteChecking;

            if (LoginItem.UseColor && !string.IsNullOrEmpty(LoginItem.Color))
                _bLabelColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(LoginItem.Color));
        }

        private void IsFavoriteChecking(object sender, Avalonia.Interactivity.RoutedEventArgs e) => LoginItem.Favorite = ((ToggleButton)sender).IsChecked ?? false;

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void EditedLogin()
        {
            if (LoginItem == null) return;
            _tbLoginName.Text = LoginItem.Name;
            if (!LoginItem.UseColor)
            {
                LoginItem.Color = null;
                _bLabelColor.Background = null;
            }

            if (LoginItem.UseColor && !string.IsNullOrEmpty(LoginItem.Color))
                _bLabelColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(LoginItem.Color));

            _tbUsername.Text = LoginItem.Type switch
            {
                0 when string.IsNullOrEmpty(LoginItem.Username) => LoginItem.Email,
                4 => LoginItem.TimeCreate,
                _ => LoginItem.Username
            };
        }

        public async void GetIconElement(bool isChangeWebSite = false)
        {
            if (LoginItem == null) return;
            if (LoginItem.Type == 0)
            {
                if (!string.IsNullOrEmpty(LoginItem.WebSite))
                {
                    try
                    {
                        if (isChangeWebSite)
                        {
                            LoginItem.Image = await ImageInteraction.DownloadImage(LoginItem.WebSite);
                            await using MemoryStream ms = new MemoryStream(Convert.FromBase64String(LoginItem.Image));

                            IconLogin.Source = new Bitmap(ms);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(LoginItem.Image))
                                LoginItem.Image = await ImageInteraction.DownloadImage(LoginItem.WebSite);

                            await using MemoryStream ms = new MemoryStream(Convert.FromBase64String(LoginItem.Image));

                            IconLogin.Source = new Bitmap(ms);
                        }
                    }
                    catch
                    {
                        IconLogin.Source = (DrawingImage)Application.Current.FindResource("GlobeIcon");
                    }
                }
                else
                    IconLogin.Source = (DrawingImage)Application.Current.FindResource("GlobeIcon");
            }
            else
            {
                IconLogin.Source = LoginItem.Type switch
                {
                    1 => (DrawingImage)Application.Current.FindResource("CardIcon"),
                    2 => (DrawingImage)Application.Current.FindResource("PersonalDataIcon"),
                    3 => (DrawingImage)Application.Current.FindResource("ReminderIcon"),
                    4 => (DrawingImage)Application.Current.FindResource("NoteIcon"),
                    _ => IconLogin.Source
                };
            }
        }
        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (LoginItem.IsReminderActive && DateTime.Parse(LoginItem.Username, System.Threading.Thread.CurrentThread.CurrentUICulture) <=
                    DateTime.Now)
                {
                    ReminderTimer.Stop();

                    _ = new ReminderWindow
                    {
                        LoginListItem = this,
                        Title = LoginItem.Name,
                        _tbName = { Text = LoginItem.Name },
                        _tbTime = { Text = LoginItem.Username }
                    }.ShowDialog(App.MainWindow);
                }
                else if (!LoginItem.IsReminderActive) ReminderTimer.Stop();
            }
            catch
            {
                ReminderTimer.Stop();
                LoginItem.IsReminderActive = false;

                _ = MessageBox.Show(App.MainWindow, null, $"" +
                    $"{(string)Application.Current.FindResource("ErrorElement1")} {LoginItem.Name}. {(string)Application.Current.FindResource("ErrorElement2")}", (string)Application.Current.FindResource("Error"),
                    MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }
        }
    }
}
