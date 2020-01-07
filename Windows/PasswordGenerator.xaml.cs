using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OlibPasswordManager.Properties.Core;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для PasswordGenerator.xaml
    /// </summary>
    public partial class PasswordGenerator : Window
    {
        public PasswordGenerator() => InitializeComponent();

        private string RandomPassword()
        {
            try
            {
                const string LOWER = "abcdefghijklmnopqrstuvwxyz";
                const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string NUMBER = "0123456789";
                const string SPECIAL = @"~!@#$%^&*():;[]{}<>,.?/\|";

                string other = txtOther.Text;

                if ((bool)chkRequireOther.IsChecked && other.Length < 1)
                {
                    MessageBox.Show("Вы не можете требовать символы из пустой строки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtOther.Focus();
                    return txtPassword.Text;
                }

                string allowed = "";

                if ((bool)chkAllowLowercase.IsChecked) allowed += LOWER;
                if ((bool)chkAllowUppercase.IsChecked) allowed += UPPER;
                if ((bool)chkAllowNumber.IsChecked) allowed += NUMBER;
                if ((bool)chkAllowSpecial.IsChecked) allowed += SPECIAL;
                if ((bool)chkAllowUnderscore.IsChecked) allowed += "_";
                if ((bool)chkAllowSpace.IsChecked) allowed += " ";
                if ((bool)chkAllowOther.IsChecked) allowed += other;

                int min_chars = int.Parse(txtMinLenght.Text);
                int max_chars = int.Parse(txtMaxLenght.Text);
                int num_chars = Crypto.RandomInteger(min_chars, max_chars);

                string password = "";

                if ((bool)chkRequireLowercase.IsChecked &&
                    (password.IndexOfAny(LOWER.ToCharArray()) == -1))
                    password += RandomChar(LOWER);
                if ((bool)chkRequireUppercase.IsChecked &&
                    (password.IndexOfAny(UPPER.ToCharArray()) == -1))
                    password += RandomChar(UPPER);
                if ((bool)chkRequireNumber.IsChecked &&
                    (password.IndexOfAny(NUMBER.ToCharArray()) == -1))
                    password += RandomChar(NUMBER);
                if ((bool)chkRequireSpecial.IsChecked &&
                    (password.IndexOfAny(SPECIAL.ToCharArray()) == -1))
                    password += RandomChar(SPECIAL);
                if ((bool)chkRequireUnderscore.IsChecked &&
                    (password.IndexOfAny("_".ToCharArray()) == -1))
                    password += "_";
                if ((bool)chkRequireSpace.IsChecked &&
                    (password.IndexOfAny(" ".ToCharArray()) == -1))
                    password += " ";
                if ((bool)chkRequireOther.IsChecked &&
                    (password.IndexOfAny(other.ToCharArray()) == -1))
                    password += RandomChar(other);

                while (password.Length < num_chars)
                    password += allowed.Substring(
                        Crypto.RandomInteger(0, allowed.Length - 1), 1);

                password = RandomizeString(password);

                return password;
            }
            catch
            {
                MessageBox.Show("Не выбрано ни одной галочки или не указана длина пароля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return txtPassword.Text;
            }
        }
        private string RandomChar(string str) => str.Substring(Crypto.RandomInteger(0, str.Length - 1), 1);
        private string RandomizeString(string str)
        {
            string result = "";
            while (str.Length > 0)
            {
                int i = Crypto.RandomInteger(0, str.Length - 1);
                result += str.Substring(i, 1);
                str = str.Remove(i, 1);
            }
            return result;
        }

        private void ClickGeneratePassword(object sender, RoutedEventArgs e) => txtPassword.Text = RandomPassword();

        private void SavePassword(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text.Length < 1) txtPassword.Text = RandomPassword();
            DialogResult = true;
            Close();
        }
    }
}
