﻿using OlibKey.AccountStructures;
using OlibKey.Core;
using System;
using System.Collections.Generic;
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
        private readonly Account _accountModelChange;
        public Action ChangedAccountCallback { get; set; }
        public Action DeleteAccountCallback { get; set; }
        private void ChangedAccountCallbackFunc() => ChangedAccountCallback?.Invoke();
        private void DeleteAccountCallbackFunc() => DeleteAccountCallback?.Invoke();
        public ChangedPasswordPage(Account accountModel)
        {
            InitializeComponent();

            _accountModelChange = accountModel;

            DataContext = _accountModelChange;


            switch (accountModel.TypeAccount)
            {
                case 0:
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
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
            if (_accountModelChange.TypeAccount == 3)
            {
                txtDateChanged.Text = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
                ChangedAccountCallbackFunc();
                App.MainWindow.Model.SelectedAccountItem.timer.Start();
            }
            NavigationService?.GoBack();
            App.MainWindow.Notification((string)Application.Current.FindResource("Not2"));
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);

        private void DeletePassword(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show((string)FindResource("MB2"), (string)FindResource("Message"), MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                App.MainWindow.Model.SelectedAccountItem.timer.Stop();
            }
            catch { }
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
        private void txtSecurityCodeCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSecutityCodeCollapsed.IsSelectionActive)
                txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;
        }
        private void txtSecurityCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtSecutityCode.IsSelectionActive)
                txtSecutityCodeCollapsed.Text = txtSecutityCode.Password;
        }

        private void cbHide_Checked(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
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
            if (cbHideSecurityCode.IsChecked != null && (bool)cbHideSecurityCode.IsChecked)
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
            if (!(bool) generatorWindow.ShowDialog()) return;
            txtPassword.Password = generatorWindow.TxtPassword.Text;
            txtPasswordCollapsed.Text = generatorWindow.TxtPassword.Text;
        }

        private void ChangedPasswordPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;
            txtPassword.Password = txtPasswordCollapsed.Text;

            cbCustomFolder.SelectedValuePath = "Value";
            cbCustomFolder.DisplayMemberPath = "Key";
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (var i in App.MainWindow.Model.DatabaseApplication.CustomFolders) pairs.Add(i.Name, i.ID);
            cbCustomFolder.Items.Add(new KeyValuePair<string, string>((string)FindResource("NotChosen"), null));
            foreach (KeyValuePair<string, string> i in pairs) cbCustomFolder.Items.Add(i);
        }

        private void Button_Click(object sender, RoutedEventArgs e) => tbStartTime.Text = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
    }
}
