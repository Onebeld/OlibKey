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
            string p, c;
            if (TxtPassword.Text.Length < 1) TxtPassword.Text = RandomPassword();

            using (StreamWriter outputFile = new StreamWriter("password.txt"))
            {
                outputFile.WriteLine("Complexity;Password");

                for (int i = 0; i < 5000; i++)
                {
                    p = RandomPassword();
                    c = PasswordUtils.CheckPasswordStrength(p).ToString();

                    outputFile.WriteLine($"{c};{p}");
                }
            }
            

            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("settings.json")) return;
            ChkAllowLowercase.IsChecked = Additional.GlobalSettings.GeneratorAllowLowercase;
            ChkAllowNumber.IsChecked = Additional.GlobalSettings.GeneratorAllowNumber;
            ChkAllowOther.IsChecked = Additional.GlobalSettings.GeneratorAllowOther;
            ChkAllowSpace.IsChecked = Additional.GlobalSettings.GeneratorAllowSpace;
            ChkAllowSpecial.IsChecked = Additional.GlobalSettings.GeneratorAllowSpecial;
            ChkAllowUnderscore.IsChecked = Additional.GlobalSettings.GeneratorAllowUnderscore;
            ChkAllowUppercase.IsChecked = Additional.GlobalSettings.GeneratorAllowUppercase;
            ChkRequireLowercase.IsChecked = Additional.GlobalSettings.GeneratorRequireLowercase;
            ChkRequireNumber.IsChecked = Additional.GlobalSettings.GeneratorRequireNumber;
            ChkRequireOther.IsChecked = Additional.GlobalSettings.GeneratorRequireOther;
            ChkRequireSpace.IsChecked = Additional.GlobalSettings.GeneratorRequireSpace;
            ChkRequireSpecial.IsChecked = Additional.GlobalSettings.GeneratorRequireSpecial;
            ChkRequireUnderscore.IsChecked = Additional.GlobalSettings.GeneratorRequireUnderscore;
            ChkRequireUppercase.IsChecked = Additional.GlobalSettings.GeneratorRequireUppercase;
            TxtMaxLenght.Text = Additional.GlobalSettings.GeneratorMaxCount;
            TxtMinLenght.Text = Additional.GlobalSettings.GeneratorMinCount;
            TxtOther.Text = Additional.GlobalSettings.GeneratorTextOther;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Additional.GlobalSettings.GeneratorAllowLowercase = (bool) ChkAllowLowercase.IsChecked;
            Additional.GlobalSettings.GeneratorAllowNumber = (bool) ChkAllowNumber.IsChecked;
            Additional.GlobalSettings.GeneratorAllowOther = (bool) ChkAllowOther.IsChecked;
            Additional.GlobalSettings.GeneratorAllowSpace = (bool) ChkAllowSpace.IsChecked;
            Additional.GlobalSettings.GeneratorAllowSpecial = (bool) ChkAllowSpecial.IsChecked;
            Additional.GlobalSettings.GeneratorAllowUnderscore = (bool) ChkAllowUnderscore.IsChecked;
            Additional.GlobalSettings.GeneratorAllowUppercase = (bool) ChkAllowUppercase.IsChecked;
            Additional.GlobalSettings.GeneratorRequireLowercase = (bool) ChkRequireLowercase.IsChecked;
            Additional.GlobalSettings.GeneratorRequireNumber = (bool) ChkRequireNumber.IsChecked;
            Additional.GlobalSettings.GeneratorRequireOther = (bool) ChkRequireOther.IsChecked;
            Additional.GlobalSettings.GeneratorRequireSpace = (bool) ChkRequireSpace.IsChecked;
            Additional.GlobalSettings.GeneratorRequireSpecial = (bool) ChkRequireSpecial.IsChecked;
            Additional.GlobalSettings.GeneratorRequireUnderscore = (bool) ChkRequireUnderscore.IsChecked;
            Additional.GlobalSettings.GeneratorRequireUppercase = (bool) ChkRequireUppercase.IsChecked;
            Additional.GlobalSettings.GeneratorTextOther = TxtOther.Text;
            Additional.GlobalSettings.GeneratorMaxCount = TxtMaxLenght.Text;
            Additional.GlobalSettings.GeneratorMinCount = TxtMinLenght.Text;
        }
    }
}
