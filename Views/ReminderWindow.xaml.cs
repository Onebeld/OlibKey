using OlibKey.Controls;
using OlibKey.Core;
using System;
using System.Media;
using System.Windows;
using System.Windows.Media;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для ReminderWindow.xaml
    /// </summary>
    public partial class ReminderWindow : Window
    {
        public AccountListItem AccountListItem;
        public SoundPlayer SoundPlayer;

        public ReminderWindow()
        {
            InitializeComponent();
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width - 10;
            Top = desktopWorkingArea.Bottom - Height - 10;

            SoundPlayer = new SoundPlayer(Resource.notification);
            SoundPlayer.PlayLooping();
        }

        private async void ButtonPause(object sender, RoutedEventArgs e)
        {
            AccountListItem.timer.Interval = new TimeSpan(0, 5, 0);
            AccountListItem.timer.Start();
            SoundPlayer.Stop();
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private async void ButtonShutdown(object sender, RoutedEventArgs e)
        {
            AccountListItem.AccountContext.IsReminderActive = false;
            SoundPlayer.Stop();
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Setting.EnableFastRendering)
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.LowQuality);
            }
        }
    }
}
