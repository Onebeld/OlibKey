using OlibKey.AccountStructures;
using OlibKey.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangedPasswordPage.xaml
    /// </summary>
    public partial class ChangedPasswordPage : Page
    {
        private readonly AccountModel _accountModelChange;
        public Action ChangedAccountCallback { get; set; }
        public Action DeleteAccountCallback { get; set; }
        private void ChangedAccountCallbackFunc() => ChangedAccountCallback?.Invoke();
        private void DeleteAccountCallbackFunc() => DeleteAccountCallback?.Invoke();
        public ChangedPasswordPage(AccountModel accountModel)
        {
            InitializeComponent();

            _accountModelChange = accountModel;

            DataContext = _accountModelChange;


            switch (accountModel.TypeAccount)
            {
                case 0:
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void ChangedAccountClick(object sender, RoutedEventArgs e)
        {
            if (txtWebSite.Text != "" && _accountModelChange.TypeAccount == 0)
                txtIconWebSite.Text =
                    "http://www.google.com/s2/favicons?domain=" + _accountModelChange.WebSite;
            else if (_accountModelChange.TypeAccount == 0)
                App.MainWindow.Model.SelectedAccountItem.imageIcon.Source =
                    (ImageSource) FindResource("globeDrawingImage");

            txtDateChanged.Text = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
            ChangedAccountCallbackFunc();
            NavigationService?.GoBack();
            App.MainWindow.Notification((string)Application.Current.FindResource("Not2"));
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);

        private void DeletePassword(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить элемент?", "Сообщение", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            DeleteAccountCallbackFunc();
            StartPage startPage = new StartPage();
            NavigationService?.Navigate(startPage);
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPasswordCollapsed.Text);
            if (txtPasswordCollapsed.IsSelectionActive)
                txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.IsSelectionActive)
                txtPasswordCollapsed.Text = txtPassword.Password;
        }
        private void txtSecutityCodeCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSecutityCodeCollapsed.IsSelectionActive)
                txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;
        }
        private void txtSecutityCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtSecutityCode.IsSelectionActive)
                txtSecutityCodeCollapsed.Text = txtSecutityCode.Password;
        }

        private void cbHide_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
            }
        }
        private void cbHideSecurityCode_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHideSecurityCode.IsChecked)
            {
                txtSecutityCode.Visibility = Visibility.Collapsed;
                txtSecutityCodeCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                txtSecutityCode.Visibility = Visibility.Visible;
                txtSecutityCodeCollapsed.Visibility = Visibility.Collapsed;
            }
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow generatorWindow = new PasswordGeneratorWindow
            {
                SaveButton = { Visibility = Visibility.Visible }
            };
            if ((bool)generatorWindow.ShowDialog())
            {
                txtPassword.Password = generatorWindow.TxtPassword.Text;
            }
        }

        private void ChangedPasswordPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;
            txtPassword.Password = txtPasswordCollapsed.Text;
        }
    }
}
