using OlibKey.ModelViews;
using System;
using System.Windows;
using System.Windows.Input;

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

        private  void LoadStorage()
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

        private void LoadStorageButton(object sender, RoutedEventArgs e) => LoadStorage();

        private void TbMasterPassword_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoadStorage();
        }
    }
}
