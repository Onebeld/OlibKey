using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow() => InitializeComponent();
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

        private void Full(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                FullMenu.SetResourceReference(HeaderedItemsControl.HeaderProperty, "Expand");
                WindowState = WindowState.Normal;
            }
            else
            {
                FullMenu.SetResourceReference(HeaderedItemsControl.HeaderProperty, "Reestablish");
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                WindowState = WindowState.Maximized;
            }
        }

        private void Timeline_OnCompleted(object? sender, EventArgs e) => Close();
    }
}
