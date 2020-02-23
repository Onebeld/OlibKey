using OlibKey.ModelViews;
using System;
using System.Windows;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для RequireMasterPasswordWindow.xaml
    /// </summary>
    public partial class RequireMasterPasswordWindow : Window
    {
        public Action LoadStorageCallback { get; set; }

        public RequireMasterPasswordWindow()
        {
            InitializeComponent();
            tbMasterPassword.Focus();
        }

        private void CancelButton(object sender, RoutedEventArgs e) => Close();

        private void LoadStorageButton(object sender, RoutedEventArgs e)
        {
            try
            {
                MainViewModel.MasterPassword = tbMasterPassword.Text;
                LoadStorageCallback?.Invoke();
                Close();
            }
            catch
            {
                MainViewModel.MasterPassword = null;
                MessageBox.Show("Неверный мастер-пароль.", "Ошибка");
            }
        }
    }
}
