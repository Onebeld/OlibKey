using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.Core;
using OlibKey.ViewModels.Pages;
using OlibKey.Views.Windows;
using ReactiveUI;
using System;
using System.Globalization;
using System.Reactive.Disposables;

namespace OlibKey.Views.Pages
{
	public class CreateLoginPage : ReactiveUserControl<CreateLoginPageViewModel>
	{
		private StackPanel _passwordSection;
		private StackPanel _reminderSection;
		private StackPanel _bankCartSection;
		private StackPanel _personalDataSection;
		private TextBox _txtSecurityCode;
		private TextBox _txtPassword;
		private TextBox _txtStartTime;
		private ProgressBar _pbHard;
		private ComboBox _cbType;

		public CreateLoginPage() => InitializeComponent();

		private void InitializeComponent()
		{
			_ = this.WhenActivated((CompositeDisposable disposable) => { });
			AvaloniaXamlLoader.Load(this);

			_passwordSection = this.FindControl<StackPanel>("PasswordSection");
			_reminderSection = this.FindControl<StackPanel>("ReminderSection");
			_bankCartSection = this.FindControl<StackPanel>("BankCartSection");
			_personalDataSection = this.FindControl<StackPanel>("PersonalDataSection");
			_txtPassword = this.FindControl<TextBox>("txtPassword");
			_txtSecurityCode = this.FindControl<TextBox>("txtSecurityCode");
			_pbHard = this.FindControl<ProgressBar>("pbHard");
			_txtStartTime = this.FindControl<TextBox>("tbStartTime");
			_cbType = this.FindControl<ComboBox>("cbType");

			_txtPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, _txtPassword));
			_cbType.SelectionChanged += SectionChanged;
			_cbType.SelectedIndex = 0;
		}

		private void SectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch (((ComboBox)sender).SelectedIndex)
			{
				case 0:
					_passwordSection.IsVisible = true;
					_reminderSection.IsVisible = false;
					_bankCartSection.IsVisible = false;
					_personalDataSection.IsVisible = false;
					break;
				case 1:
					_passwordSection.IsVisible = false;
					_reminderSection.IsVisible = false;
					_bankCartSection.IsVisible = true;
					_personalDataSection.IsVisible = false;
					break;
				case 2:
					_passwordSection.IsVisible = false;
					_reminderSection.IsVisible = false;
					_bankCartSection.IsVisible = false;
					_personalDataSection.IsVisible = true;
					break;
				case 3:
					_txtStartTime.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture);
					_passwordSection.IsVisible = false;
					_reminderSection.IsVisible = true;
					_bankCartSection.IsVisible = false;
					_personalDataSection.IsVisible = false;
					break;
				case 4:
					_passwordSection.IsVisible = false;
					_reminderSection.IsVisible = false;
					_bankCartSection.IsVisible = false;
					_personalDataSection.IsVisible = false;
					break;
			}
		}

		private void CheckedPassword(object sender, RoutedEventArgs e) => _txtPassword.PasswordChar = ((CheckBox)sender).IsChecked == true ? '\0' : '•';

		private void CheckedSecurityCode(object sender, RoutedEventArgs e) => _txtSecurityCode.PasswordChar = ((CheckBox)sender).IsChecked == true ? '\0' : '•';

		private void TimeZeroing(object sender, RoutedEventArgs e) => _txtStartTime.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture);

		private async void GeneratePassword(object sender, RoutedEventArgs e)
		{
			PasswordGeneratorWindow a = new PasswordGeneratorWindow { _saveButton = { IsVisible = true } };
			if (await a.ShowDialog<bool>(App.MainWindow)) _txtPassword.Text = a._tbPassword.Text;
		}
	}
}
