using OlibKey.AccountStructures;
using OlibKey.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace OlibKey.Controls
{
    /// <summary>
    /// Interaction logic for AccountListItem.xaml
    /// </summary>
    public partial class AccountListItem : UserControl
    {
        public Action<Account> ShowContentCallback { get; set; }
        public Action<Account> EditContentCallback { get; set; }
        public Action FocusCallback { get; set; }
        public Account AccountContext => DataContext as Account;
        public AccountListItem() => InitializeComponent();

        public DispatcherTimer timer;

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

                    timer = new DispatcherTimer
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
            try
            {
                if (DateTime.Parse(AccountContext.Username, System.Threading.Thread.CurrentThread.CurrentUICulture) <=
                    DateTime.Now && AccountContext.IsReminderActive)
                {
                    timer.Stop();

                    var reminderWindow = new ReminderWindow
                    {
                        AccountListItem = this,
                        lNameElement = {Content = AccountContext.AccountName},
                        lTimeStart = {Content = AccountContext.Username}
                    };
                    reminderWindow.Show();
                }
                else if (!AccountContext.IsReminderActive) timer.Stop();
            }
            catch
            {
                timer.Stop();
                AccountContext.IsReminderActive = false;
                MessageBox.Show($"{(string)FindResource("ErrorElement1")} {AccountContext.AccountName}\n\n{(string)FindResource("ErrorElement2")}", (string)FindResource("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
