using OlibKey.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для ReminderWindow.xaml
    /// </summary>
    public partial class ReminderWindow : Window
    {
        public AccountListItem accountListItem;
        public SoundPlayer soundPlayer;

        public ReminderWindow()
        {
            InitializeComponent();
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width - 10;
            Top = desktopWorkingArea.Bottom - Height - 10;

            soundPlayer = new SoundPlayer(Resource.notification);
            soundPlayer.PlayLooping(); // can also use soundPlayer.PlaySync()
        }

        private void ButtonPause(object sender, RoutedEventArgs e)
        {
            accountListItem.timer.Interval = new TimeSpan(0, 0, 10);
            accountListItem.timer.Start();
            soundPlayer.Stop();
            Close();
        }

        private void ButtonShutdown(object sender, RoutedEventArgs e)
        {
            accountListItem.AccountContext.IsReminderActive = false;
            soundPlayer.Stop();
            Close();
        }
    }
}
