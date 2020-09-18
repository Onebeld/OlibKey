using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using OlibKey.Controls.ColorPicker;
using OlibKey.Core;
using OlibKey.ViewModels.Color;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Windows;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace OlibKey.Views.Pages
{
    public class EditLoginPage : ReactiveUserControl<EditLoginPageViewModel>
    {
        private TextBox _txtPassword;
        private TextBox _txtSecurityCode;
        private ProgressBar _pbHard;
        private TextBox _tbColor;
        private ColorPicker _colorPicker;
        private Popup _pColorPicker;

        private Border _bColor;

        public EditLoginPage() => InitializeComponent();

        private void InitializeComponent()
        {
            this.WhenActivated((CompositeDisposable disposable) => { });

            AvaloniaXamlLoader.Load(this);

            _txtPassword = this.FindControl<TextBox>("txtPassword");
            _txtSecurityCode = this.FindControl<TextBox>("txtSecurityCode");
            _pbHard = this.FindControl<ProgressBar>("pbHard");
            _bColor = this.FindControl<Border>("bColor");
            _tbColor = this.FindControl<TextBox>("tbColor");
            _pColorPicker = this.FindControl<Popup>("pColorPicker");
            _colorPicker = this.FindControl<ColorPicker>("colorPicker");

            _colorPicker.ChangeColor += _colorPicker_ChangeColor;

            _txtPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, _txtPassword));

            _tbColor.GetObservable(TextBox.TextProperty).Subscribe(_ => ChangeColorTextBox());
        }

        private void ChangeColorTextBox()
        {
            if (string.IsNullOrEmpty(_tbColor.Text)) _tbColor.Text = ((Color)Application.Current.FindResource("ThemeSelectedControlColor")).ToString();
            else _bColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(_tbColor.Text));
        }

        private void _colorPicker_ChangeColor(object sender, RoutedEventArgs e)
        {
            _tbColor.Text = ((ArgbColorViewModel)_pColorPicker.DataContext).ToHexString();
        }

        private void OpenColorPicker(object sender, RoutedEventArgs e)
        {
            _pColorPicker.DataContext = new ArgbColorViewModel
            {
                Hex = _tbColor.Text
            };
            _pColorPicker.Open();
        }

        private void CheckedPassword(object sender, RoutedEventArgs e) => _txtPassword.PasswordChar = ((CheckBox)sender).IsChecked ?? false ? '\0' : '•';

        private void CheckedSecurityCode(object sender, RoutedEventArgs e) => _txtSecurityCode.PasswordChar = ((CheckBox)sender).IsChecked ?? false ? '\0' : '•';

        private async void GeneratePassword(object sender, RoutedEventArgs e)
        {
            App.MainWindowViewModel.PasswordGenerator = new PasswordGeneratorWindow { _saveButton = { IsVisible = true } };
            if (await App.MainWindowViewModel.PasswordGenerator.ShowDialog<bool>(App.MainWindow)) _txtPassword.Text = App.MainWindowViewModel.PasswordGenerator._tbPassword.Text;
        }
    }
}
