using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using OlibKey.Controls.ColorPicker;
using OlibKey.Core;
using OlibKey.ViewModels.Pages;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace OlibKey.Views.Pages
{
	public class LoginInformationPage : ReactiveUserControl<LoginInformationPageViewModel>
	{
		private TextBox _txtPassword;
		private TextBox _txtSecurityCode;
		private ProgressBar _pbHard;
		private TextBox _tbColor;

		private Border _bColor;


		public LoginInformationPage() => InitializeComponent();

		private void InitializeComponent()
		{
			this.WhenActivated((CompositeDisposable disposable) => { });
			AvaloniaXamlLoader.Load(this);

			_txtPassword = this.FindControl<TextBox>("txtPassword");
			_txtSecurityCode = this.FindControl<TextBox>("txtSecurityCode");
			_pbHard = this.FindControl<ProgressBar>("pbHard");
			_bColor = this.FindControl<Border>("bColor");
			_tbColor = this.FindControl<TextBox>("tbColor");

			_txtPassword.GetObservable(TextBox.TextProperty).Subscribe(val =>
				PasswordUtils.DeterminingPasswordComplexity(_pbHard, _txtPassword));

			_tbColor.GetObservable(TextBox.TextProperty).Subscribe(_ => ChangeColorTextBox());
		}

        private void ChangeColorTextBox()
        {
            if (string.IsNullOrEmpty(_tbColor.Text)) _bColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(((Color)Application.Current.FindResource("ThemeSelectedControlColor")).ToString()));
			else _bColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(_tbColor.Text));
        }

        private void CheckedPassword(object sender, RoutedEventArgs e) => _txtPassword.PasswordChar = ((CheckBox)sender).IsChecked ?? false ? '\0' : '•';

		private void CheckedSecurityCode(object sender, RoutedEventArgs e) => _txtSecurityCode.PasswordChar = ((CheckBox)sender).IsChecked ?? false ? '\0' : '•';
	}
}