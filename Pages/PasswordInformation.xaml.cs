using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordInformation.xaml
    /// </summary>
    public partial class PasswordInformation : Page
    {
        public PasswordInformation()
        {
            InitializeComponent();
        }

        private void CopyPassword(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtPassword.Password);
        }

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = txtPassword.Password;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (!(bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = null;
            }
        }
    }
}
