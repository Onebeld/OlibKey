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
        public AccountListItem accountListItem;
        public SoundPlayer soundPlayer;

        public ReminderWindow()
        {
            InitializeComponent();
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width - 10;
            Top = desktopWorkingArea.Bottom - Height - 10;

            soundPlayer = new SoundPlayer(Resource.notification);
            soundPlayer.PlayLooping();
        }

        private async void ButtonPause(object sender, RoutedEventArgs e)
        {
            accountListItem.timer.Interval = new TimeSpan(0, 5, 0);
            accountListItem.timer.Start();
            soundPlayer.Stop();
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private async void ButtonShutdown(object sender, RoutedEventArgs e)
        {
            accountListItem.AccountContext.IsReminderActive = false;
            soundPlayer.Stop();
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
