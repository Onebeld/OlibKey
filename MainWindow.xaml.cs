using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private bool _mRestoreForDragMove;

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ResizeMode != ResizeMode.CanResize && ResizeMode != ResizeMode.CanResizeWithGrip) return;

                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else
            {
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                _mRestoreForDragMove = WindowState == WindowState.Maximized;
                DragMove();
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _mRestoreForDragMove = false;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_mRestoreForDragMove) return;
            _mRestoreForDragMove = false;

            Point point = PointToScreen(e.MouseDevice.GetPosition(this));

            Left = point.X * 0.5;
            Top = point.Y - 15;

            WindowState = WindowState.Normal;
            try
            {
                DragMove();
            }
            catch
            {
                // ignored
            }
        }

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
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;
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

        private void Full(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
            {
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                WindowState = WindowState.Maximized;
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation { Duration = TimeSpan.FromSeconds(0.2), From = 1, To = 0, };
            DoubleAnimation anim1 = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.2),
                DecelerationRatio = 1,
                From = 1,
                To = 0.8,
            };
            anim1.Completed += (s,d) => Model.ExitProgramVoid();
            Timeline.SetDesiredFrameRate(anim, 60);
            Timeline.SetDesiredFrameRate(anim1, 60);
            BeginAnimation(OpacityProperty, anim);
            ScaleWindow.BeginAnimation(ScaleTransform.ScaleXProperty, anim1);
            ScaleWindow.BeginAnimation(ScaleTransform.ScaleYProperty, anim1);
        }

        private void Timeline_OnCompleted(object sender, EventArgs e) => Model.ExitProgramVoid();

        private void Collapse(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
