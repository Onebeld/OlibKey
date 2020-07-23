using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace OlibKey.Views.Windows
{
	public class RequireMasterPasswordWindow : Window
	{
		public Action LoadStorageCallback { get; set; }

		private TextBox _tbPassword;
		private Button _bOpen;
		private Button _bCancel;

		public RequireMasterPasswordWindow()
		{
			InitializeComponent();
			_bCancel.Click += (s, e) => Close();
			_bOpen.Click += ButtonLoadStorage;
			_tbPassword.KeyDown += KeyEnterLoadStorage;
		}

		private void KeyEnterLoadStorage(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) LoadStorage();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbPassword = this.FindControl<TextBox>("tbPassword");
			_bOpen = this.FindControl<Button>("bOpen");
			_bCancel = this.FindControl<Button>("bCancel");
		}

		private void ButtonLoadStorage(object sender, RoutedEventArgs e) => LoadStorage();

		private void LoadStorage()
		{
				App.MainWindowViewModel.MasterPassword = _tbPassword.Text;
				LoadStorageCallback?.Invoke();
				App.Autosave.Start();
				Close();
		}
	}
}