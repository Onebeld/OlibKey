using System;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.ViewModels.Pages;
using ReactiveUI;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using OlibKey.Core;

namespace OlibKey.Views.Pages
{
	public class EditLoginPage : ReactiveUserControl<EditLoginPageViewModel>
	{
		private TextBox _txtPassword;
		private TextBox _txtSecurityCode;
		private ProgressBar _pbHard;

		public EditLoginPage() => InitializeComponent();

		private void InitializeComponent()
		{
			this.WhenActivated((CompositeDisposable disposable) => { });
			AvaloniaXamlLoader.Load(this);
			_txtPassword = this.FindControl<TextBox>("txtPassword");
			_txtSecurityCode = this.FindControl<TextBox>("txtSecurityCode");
			_pbHard = this.FindControl<ProgressBar>("pbHard");

			_txtPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, _txtPassword));
		}

		private void CheckedPassword(object sender, RoutedEventArgs e)
		{
			CheckBox cb = (CheckBox) sender;
			_txtPassword.PasswordChar = cb.IsChecked == true ? '\0' : '•';
		}

		private void CheckedSecurityCode(object sender, RoutedEventArgs e)
		{
			CheckBox cb = (CheckBox) sender;
			_txtSecurityCode.PasswordChar = cb.IsChecked == true ? '\0' : '•';
		}
	}
}
