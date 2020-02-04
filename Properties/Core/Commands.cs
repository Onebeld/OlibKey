using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace OlibPasswordManager.Properties.Core
{
    public class ShowProgram : ICommand
    {
        public void Execute(object parameter)
        {
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Topmost = true;
            Application.Current.MainWindow.Topmost = false;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
