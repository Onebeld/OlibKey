using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace OlibKey
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();
        private string _strResult;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Model.ExitProgramVoid();
            e.Cancel = true;
        }

        public async void Notification(string s)
        {
            bNotification.Visibility = Visibility.Visible;
            NotText.Text = s;
            DoubleAnimation anim = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                DecelerationRatio = 0.5,
                AccelerationRatio = 0.5,
                From = 0,
                To = 1,
            };
            Timeline.SetDesiredFrameRate(anim, 60);
            bNotification.BeginAnimation(OpacityProperty, anim);
            await Task.Delay(3000);
            DoubleAnimation anim1 = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                DecelerationRatio = 0.5,
                AccelerationRatio = 0.5,
                From = bNotification.Opacity,
                To = 0
            };
            anim1.Completed += (s, r) => bNotification.Visibility = Visibility.Collapsed;
            Timeline.SetDesiredFrameRate(anim1, 60);
            bNotification.BeginAnimation(OpacityProperty, anim1);
        }

        public async void IconSave()
        {
            saveIcon.Visibility = Visibility.Visible;
            DoubleAnimation anim = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                DecelerationRatio = 0.5,
                AccelerationRatio = 0.5,
                From = 0,
                To = 1,
            };
            Timeline.SetDesiredFrameRate(anim, 60);
            saveIcon.BeginAnimation(OpacityProperty, anim);
            await Task.Delay(2000);
            DoubleAnimation anim1 = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                DecelerationRatio = 0.5,
                AccelerationRatio = 0.5,
                From = saveIcon.Opacity,
                To = 0
            };
            anim1.Completed += (s, r) => saveIcon.Visibility = Visibility.Collapsed;
            Timeline.SetDesiredFrameRate(anim1, 60);
            saveIcon.BeginAnimation(OpacityProperty, anim1);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.N:
                        if (Model.IsUnlockStorage)
                            Model.NewCreatePasswordVoid();
                        break;
                    case Key.G:
                        Model.PasswordGeneratorVoid();
                        break;
                    case Key.O:
                        Model.OpenStorageVoid();
                        break;
                    case Key.S:
                        if (Model.IsUnlockStorage)
                            Model.SaveAccount();
                        break;
                }
            }
        }

        public async void CheckUpdate(bool b)
        {
            try
            {
                using var wb = new WebClient();
                wb.DownloadStringCompleted += (s, args) => _strResult = args.Result;
                await wb.DownloadStringTaskAsync(new Uri("https://raw.githubusercontent.com/MagnificentEagle/OlibKey/master/forRepository/version.txt"));
                var latest = float.Parse(_strResult.Replace(".", ""));
                var current = float.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                if (!(latest > current) && b)
                {
                    MessageBox.Show((string)Application.Current.FindResource("MB8"),
                        (string)Application.Current.FindResource("Message"), MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                if (!(latest > current)) return;
                if (MessageBox.Show((string)Application.Current.FindResource("MB4"),
                        (string)Application.Current.FindResource("Message"), MessageBoxButton.YesNo,
                        MessageBoxImage.Information) != MessageBoxResult.Yes) return;
                var psi = new ProcessStartInfo
                {
                    FileName = "https://github.com/MagnificentEagle/OlibKey/releases",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                if (b)
                {
                    MessageBox.Show((string)FindResource("MB5"),
                        (string)FindResource("Error"), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Model.SaveAccount();
            Application.Current.Shutdown();
        }

        private void MoveUpButton(object sender, RoutedEventArgs e) => Model.MoveUp();

        private void MoveDownButton(object sender, RoutedEventArgs e) => Model.MoveDown();
    }
}
