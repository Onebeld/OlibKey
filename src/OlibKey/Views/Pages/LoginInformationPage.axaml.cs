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
		private ProgressBar _pbHard;
		private TextBox _tbColor;

		private Border _bColor;

		public LoginInformationPage() => InitializeComponent();

		private void InitializeComponent()
		{
			this.WhenActivated((CompositeDisposable disposable) => { });
			AvaloniaXamlLoader.Load(this);

			_pbHard = this.FindControl<ProgressBar>("pbHard");
			_bColor = this.FindControl<Border>("bColor");
			_tbColor = this.FindControl<TextBox>("tbColor");

			this.FindControl<TextBox>("txtPassword").GetObservable(TextBox.TextProperty).Subscribe(value =>
				PasswordUtils.DeterminingPasswordComplexity(_pbHard, value));

			_tbColor.GetObservable(TextBox.TextProperty).Subscribe(_ => ChangeColorTextBox());
		}

        private void ChangeColorTextBox()
        {
            if (string.IsNullOrEmpty(_tbColor.Text)) _bColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(((Color)Application.Current.FindResource("ThemeSelectedControlColor")).ToString()));
			else _bColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(_tbColor.Text));
        }
	}
}