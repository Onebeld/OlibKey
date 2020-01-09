using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("settings.json"))
            {
                chkAllowLowercase.IsChecked = App.Settings.GeneratorAllowLowercase;
                chkAllowNumber.IsChecked = App.Settings.GeneratorAllowNumber;
                chkAllowOther.IsChecked = App.Settings.GeneratorAllowOther;
                chkAllowSpace.IsChecked = App.Settings.GeneratorAllowSpace;
                chkAllowSpecial.IsChecked = App.Settings.GeneratorAllowSpecial;
                chkAllowUnderscore.IsChecked = App.Settings.GeneratorAllowUnderscore;
                chkAllowUppercase.IsChecked = App.Settings.GeneratorAllowUppercase;
                chkRequireLowercase.IsChecked = App.Settings.GeneratorRequireLowercase;
                chkRequireNumber.IsChecked = App.Settings.GeneratorRequireNumber;
                chkRequireOther.IsChecked = App.Settings.GeneratorRequireOther;
                chkRequireSpace.IsChecked = App.Settings.GeneratorRequireSpace;
                chkRequireSpecial.IsChecked = App.Settings.GeneratorRequireSpecial;
                chkRequireUnderscore.IsChecked = App.Settings.GeneratorRequireUnderscore;
                chkRequireUppercase.IsChecked = App.Settings.GeneratorRequireUppercase;
                txtMaxLenght.Text = App.Settings.GeneratorMaxCount;
                txtMinLenght.Text = App.Settings.GeneratorMinCount;
                txtOther.Text = App.Settings.GeneratorTextOther;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Settings.GeneratorAllowLowercase = (bool)chkAllowLowercase.IsChecked;
            App.Settings.GeneratorAllowNumber = (bool)chkAllowNumber.IsChecked;
            App.Settings.GeneratorAllowOther = (bool)chkAllowOther.IsChecked;
            App.Settings.GeneratorAllowSpace = (bool)chkAllowSpace.IsChecked;
            App.Settings.GeneratorAllowSpecial = (bool)chkAllowSpecial.IsChecked;
            App.Settings.GeneratorAllowUnderscore = (bool)chkAllowUnderscore.IsChecked;
            App.Settings.GeneratorAllowUppercase = (bool)chkAllowUppercase.IsChecked;
            App.Settings.GeneratorRequireLowercase = (bool)chkRequireLowercase.IsChecked;
            App.Settings.GeneratorRequireNumber = (bool)chkRequireNumber.IsChecked;
            App.Settings.GeneratorRequireOther = (bool)chkRequireOther.IsChecked;
            App.Settings.GeneratorRequireSpace = (bool)chkRequireSpace.IsChecked;
            App.Settings.GeneratorRequireSpecial = (bool)chkRequireSpecial.IsChecked;
            App.Settings.GeneratorRequireUnderscore = (bool)chkRequireUnderscore.IsChecked;
            App.Settings.GeneratorRequireUppercase = (bool)chkRequireUppercase.IsChecked;
            App.Settings.GeneratorTextOther = txtOther.Text;
            App.Settings.GeneratorMaxCount = txtMaxLenght.Text;
            App.Settings.GeneratorMinCount = txtMinLenght.Text;
        }
    }
}
