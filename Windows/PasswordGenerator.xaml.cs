using System.IO;
using System.Windows;
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
                const string lower = "abcdefghijklmnopqrstuvwxyz";
                const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string number = "0123456789";
                const string special = @"~!@#$%^&*():;[]{}<>,.?/\|";
                string other = TxtOther.Text;
                if (ChkRequireOther.IsChecked != null && ((bool) ChkRequireOther.IsChecked && other.Length < 1))
                {
                    MessageBox.Show((string) Application.Current.Resources["MB6"], (string) Application.Current.Resources["Error"], MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    TxtOther.Focus();
                    return TxtPassword.Text;
                }

                string allowed = "";
                if (ChkAllowLowercase.IsChecked != null && (bool) ChkAllowLowercase.IsChecked) allowed += lower;
                if (ChkAllowUppercase.IsChecked != null && (bool) ChkAllowUppercase.IsChecked) allowed += upper;
                if (ChkAllowNumber.IsChecked != null && (bool) ChkAllowNumber.IsChecked) allowed += number;
                if (ChkAllowSpecial.IsChecked != null && (bool) ChkAllowSpecial.IsChecked) allowed += special;
                if (ChkAllowUnderscore.IsChecked != null && (bool) ChkAllowUnderscore.IsChecked) allowed += "_";
                if (ChkAllowSpace.IsChecked != null && (bool) ChkAllowSpace.IsChecked) allowed += " ";
                if (ChkAllowOther.IsChecked != null && (bool) ChkAllowOther.IsChecked) allowed += other;
                int min_chars = int.Parse(TxtMinLenght.Text);
                int max_chars = int.Parse(TxtMaxLenght.Text);
                int num_chars = Crypto.RandomInteger(min_chars, max_chars);
                string password = "";
                if (ChkRequireLowercase.IsChecked != null && ((bool) ChkRequireLowercase.IsChecked && (password.IndexOfAny(lower.ToCharArray()) == -1)))
                    password += RandomChar(lower);
                if (ChkRequireUppercase.IsChecked != null && ((bool) ChkRequireUppercase.IsChecked && (password.IndexOfAny(upper.ToCharArray()) == -1)))
                    password += RandomChar(upper);
                if (ChkRequireNumber.IsChecked != null && ((bool) ChkRequireNumber.IsChecked && (password.IndexOfAny(number.ToCharArray()) == -1)))
                    password += RandomChar(number);
                if (ChkRequireSpecial.IsChecked != null && ((bool) ChkRequireSpecial.IsChecked && (password.IndexOfAny(special.ToCharArray()) == -1)))
                    password += RandomChar(special);
                if (ChkRequireUnderscore.IsChecked != null && ((bool) ChkRequireUnderscore.IsChecked && (password.IndexOfAny("_".ToCharArray()) == -1)))
                    password += "_";
                if (ChkRequireSpace.IsChecked != null && ((bool) ChkRequireSpace.IsChecked && (password.IndexOfAny(" ".ToCharArray()) == -1))) password += " ";
                if (ChkRequireOther.IsChecked != null && ((bool) ChkRequireOther.IsChecked && (password.IndexOfAny(other.ToCharArray()) == -1)))
                    password += RandomChar(other);
                while (password.Length < num_chars)
                    password += allowed.Substring(Crypto.RandomInteger(0, allowed.Length - 1), 1);
                password = RandomizeString(password);
                return password;
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["MB7"], (string)Application.Current.Resources["Error"],
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return TxtPassword.Text;
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

        private void ClickGeneratePassword(object sender, RoutedEventArgs e) => TxtPassword.Text = RandomPassword();

        private void SavePassword(object sender, RoutedEventArgs e)
        {
            if (TxtPassword.Text.Length < 1) TxtPassword.Text = RandomPassword();
            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("settings.json")) return;
            ChkAllowLowercase.IsChecked = App.Settings.GeneratorAllowLowercase;
            ChkAllowNumber.IsChecked = App.Settings.GeneratorAllowNumber;
            ChkAllowOther.IsChecked = App.Settings.GeneratorAllowOther;
            ChkAllowSpace.IsChecked = App.Settings.GeneratorAllowSpace;
            ChkAllowSpecial.IsChecked = App.Settings.GeneratorAllowSpecial;
            ChkAllowUnderscore.IsChecked = App.Settings.GeneratorAllowUnderscore;
            ChkAllowUppercase.IsChecked = App.Settings.GeneratorAllowUppercase;
            ChkRequireLowercase.IsChecked = App.Settings.GeneratorRequireLowercase;
            ChkRequireNumber.IsChecked = App.Settings.GeneratorRequireNumber;
            ChkRequireOther.IsChecked = App.Settings.GeneratorRequireOther;
            ChkRequireSpace.IsChecked = App.Settings.GeneratorRequireSpace;
            ChkRequireSpecial.IsChecked = App.Settings.GeneratorRequireSpecial;
            ChkRequireUnderscore.IsChecked = App.Settings.GeneratorRequireUnderscore;
            ChkRequireUppercase.IsChecked = App.Settings.GeneratorRequireUppercase;
            TxtMaxLenght.Text = App.Settings.GeneratorMaxCount;
            TxtMinLenght.Text = App.Settings.GeneratorMinCount;
            TxtOther.Text = App.Settings.GeneratorTextOther;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Settings.GeneratorAllowLowercase = (bool) ChkAllowLowercase.IsChecked;
            App.Settings.GeneratorAllowNumber = (bool) ChkAllowNumber.IsChecked;
            App.Settings.GeneratorAllowOther = (bool) ChkAllowOther.IsChecked;
            App.Settings.GeneratorAllowSpace = (bool) ChkAllowSpace.IsChecked;
            App.Settings.GeneratorAllowSpecial = (bool) ChkAllowSpecial.IsChecked;
            App.Settings.GeneratorAllowUnderscore = (bool) ChkAllowUnderscore.IsChecked;
            App.Settings.GeneratorAllowUppercase = (bool) ChkAllowUppercase.IsChecked;
            App.Settings.GeneratorRequireLowercase = (bool) ChkRequireLowercase.IsChecked;
            App.Settings.GeneratorRequireNumber = (bool) ChkRequireNumber.IsChecked;
            App.Settings.GeneratorRequireOther = (bool) ChkRequireOther.IsChecked;
            App.Settings.GeneratorRequireSpace = (bool) ChkRequireSpace.IsChecked;
            App.Settings.GeneratorRequireSpecial = (bool) ChkRequireSpecial.IsChecked;
            App.Settings.GeneratorRequireUnderscore = (bool) ChkRequireUnderscore.IsChecked;
            App.Settings.GeneratorRequireUppercase = (bool) ChkRequireUppercase.IsChecked;
            App.Settings.GeneratorTextOther = TxtOther.Text;
            App.Settings.GeneratorMaxCount = TxtMaxLenght.Text;
            App.Settings.GeneratorMinCount = TxtMinLenght.Text;
        }
    }
}
