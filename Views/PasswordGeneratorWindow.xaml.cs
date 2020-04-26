using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OlibKey.Core;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для PasswordGeneratorWindow.xaml
    /// </summary>
    public partial class PasswordGeneratorWindow : Window
    {
        public PasswordGeneratorWindow() => InitializeComponent();

        private string RandomPassword()
        {
            try
            {
                const string lower = "abcdefghijklmnopqrstuvwxyz";
                const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string number = "0123456789";
                const string special = @"~!@#$%^&*():;[]{}<>,.?/\|";
                string other = TxtOther.Text;

                string allowed = "";
                string password = "";
                if (ChkAllowLowercase.IsChecked != null && (bool)ChkAllowLowercase.IsChecked) allowed += lower;
                if (ChkAllowUppercase.IsChecked != null && (bool)ChkAllowUppercase.IsChecked) allowed += upper;
                if (ChkAllowNumber.IsChecked != null && (bool)ChkAllowNumber.IsChecked) allowed += number;
                if (ChkAllowSpecial.IsChecked != null && (bool)ChkAllowSpecial.IsChecked) allowed += special;
                if (ChkAllowUnderscore.IsChecked != null && (bool)ChkAllowUnderscore.IsChecked && password.IndexOfAny("_".ToCharArray()) == -1)
                {
                    allowed += "_";
                    password += "_";
                }
                if (ChkAllowOther.IsChecked != null && (bool)ChkAllowOther.IsChecked) allowed += other;
                int min_chars = int.Parse(TxtMinLenght.Text);
                int num_chars = Crypto.RandomInteger(min_chars, min_chars);
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

        private async void SavePassword(object sender, RoutedEventArgs e)
        {
            if (TxtPassword.Text.Length < 1) TxtPassword.Text = RandomPassword();

            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Setting.EnableFastRendering)
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.LowQuality);
            }
            ChkAllowLowercase.IsChecked = App.Setting.GeneratorAllowLowercase;
            ChkAllowNumber.IsChecked = App.Setting.GeneratorAllowNumber;
            ChkAllowOther.IsChecked = App.Setting.GeneratorAllowOther;
            ChkAllowSpecial.IsChecked = App.Setting.GeneratorAllowSpecial;
            ChkAllowUnderscore.IsChecked = App.Setting.GeneratorAllowUnderscore;
            ChkAllowUppercase.IsChecked = App.Setting.GeneratorAllowUppercase;
            TxtOther.Text = App.Setting.GeneratorTextOther;
            TxtMinLenght.Text = App.Setting.GenerationCount;
        }
        private void ClosingWindow()
        {
            App.Setting.GenerationCount = TxtMinLenght.Text;
            App.Setting.GeneratorAllowLowercase = (bool)ChkAllowLowercase.IsChecked;
            App.Setting.GeneratorAllowNumber = (bool)ChkAllowNumber.IsChecked;
            App.Setting.GeneratorAllowOther = (bool)ChkAllowOther.IsChecked;
            App.Setting.GeneratorAllowSpecial = (bool)ChkAllowSpecial.IsChecked;
            App.Setting.GeneratorAllowUnderscore = (bool)ChkAllowUnderscore.IsChecked;
            App.Setting.GeneratorAllowUppercase = (bool)ChkAllowUppercase.IsChecked;
            App.Setting.GeneratorTextOther = TxtOther.Text;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => ClosingWindow();

        private void Timeline_OnCompleted(object sender, EventArgs e) => Close();

        private void CopyGeneratedPassword(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(TxtPassword.Text);
            }
            catch { }
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
