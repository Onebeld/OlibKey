using OlibKey.AccountStructures;
using OlibKey.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OlibKey.Controls
{
    /// <summary>
    /// Interaction logic for AccountListItem.xaml
    /// </summary>
    public partial class AccountListItem : UserControl
    {
        public Action<AccountModel> ShowContentCallback { get; set; }
        public Action<AccountModel> EditContentCallback { get; set; }
        public Action FocusCallback { get; set; }
        public AccountModel AccountContext { get => DataContext as AccountModel; }
        public AccountListItem() => InitializeComponent();

        public System.Windows.Threading.DispatcherTimer timer;

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowContentCallback?.Invoke(AccountContext);
            FocusCallback?.Invoke();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (AccountContext.TypeAccount)
            {
                case 1:
                    imageIcon.Source = (ImageSource)FindResource("debit_cardDrawingImage");
                    break;
                case 2:
                    imageIcon.Source = (ImageSource)FindResource("contactDrawingImage");
                    break;
                case 3:
                    imageIcon.Source = (ImageSource)FindResource("reminderDrawingImage");

                    timer = new System.Windows.Threading.DispatcherTimer
                    {
                        Interval = new TimeSpan(0, 0, 3)
                    };
                    timer.Tick += Timer_Tick;

                    timer.Start();
                    break;
                default:
                    if (string.IsNullOrEmpty(AccountContext.WebSite))
                        imageIcon.Source = (ImageSource) FindResource("globeDrawingImage");
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Parse(AccountContext.Username) <= DateTime.Now && AccountContext.IsReminderActive)
            {
                timer.Stop();

                ReminderWindow reminderWindow = new ReminderWindow
                {
                    accountListItem = this
                };
                reminderWindow.lNameElement.Content = AccountContext.AccountName;
                reminderWindow.lTimeStart.Content = AccountContext.Username;
                reminderWindow.Show();
            }
            else if (!AccountContext.IsReminderActive) timer.Stop();
        }
    }
}
