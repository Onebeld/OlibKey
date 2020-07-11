using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

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
			try
			{
				App.MainWindowViewModel.MasterPassword = _tbPassword.Text;
				LoadStorageCallback?.Invoke();
				App.Autosave.Start();
				Close();
			}
			catch
			{
				App.MainWindowViewModel.MasterPassword = null;
				_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("MB3"), (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
			}
		}
	}
}