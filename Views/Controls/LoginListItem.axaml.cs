using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using OlibKey.Structures;
using OlibKey.Views.Windows;
using System;

namespace OlibKey.Views.Controls
{
	public class LoginListItem : UserControl
	{
		public Image IconLogin;
		private readonly TextBlock _tbLoginName;
		private readonly TextBlock _tbUsername;

		public string LoginID { get; set; }

		public DispatcherTimer ReminderTimer;

		public Login LoginItem { get; set; }

		public LoginListItem() => InitializeComponent();

		public LoginListItem(Login Login)
		{
			InitializeComponent();

			IconLogin = this.FindControl<Image>("imageIconWebSite");
			_tbLoginName = this.FindControl<TextBlock>("tbLoginName");
			_tbUsername = this.FindControl<TextBlock>("tbUsername");

			LoginItem = Login;

			EditedLogin();

			switch (LoginItem.Type)
			{
				case 3:

					ReminderTimer = new DispatcherTimer
					{
						Interval = new TimeSpan(0, 0, 3)
					};
					ReminderTimer.Tick += ReminderTimer_Tick;

					ReminderTimer.Start();
					break;
			}

			if (LoginItem.Type == 0 && string.IsNullOrEmpty(Login.Username)) _tbUsername.Text = Login.Email;

			if (LoginItem.Type == 4) _tbUsername.Text = LoginItem.TimeCreate;
		}

		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

		public void EditedLogin()
		{
			if (LoginItem == null) return;
			_tbLoginName.Text = LoginItem.Name;
			_tbUsername.Text = LoginItem.Username;

			if (LoginItem.Type == 0 && string.IsNullOrEmpty(LoginItem.Username)) _tbUsername.Text = LoginItem.Email;

			if (LoginItem.Type == 4) _tbUsername.Text = LoginItem.TimeCreate;
		}

		public async void GetIconElement()
		{
			if (LoginItem == null) return;
			if (LoginItem.Type == 0)
			{
				if (!string.IsNullOrEmpty(LoginItem.WebSite))
				{
					try
					{
						IconLogin.Source = await Core.ImageInteraction.DownloadImage(LoginItem.WebSite);
					}
					catch
					{
						IconLogin.Source = (DrawingImage)Application.Current.FindResource("GlobeIcon");
					}
				}
				else
					IconLogin.Source = (DrawingImage)Application.Current.FindResource("GlobeIcon");
			}
			else
			{
				IconLogin.Source = LoginItem.Type switch
				{
					1 => (DrawingImage)Application.Current.FindResource("CardIcon"),
					2 => (DrawingImage)Application.Current.FindResource("PersonalDataIcon"),
					3 => (DrawingImage)Application.Current.FindResource("ReminderIcon"),
					4 => (DrawingImage)Application.Current.FindResource("NoteIcon"),
					_ => IconLogin.Source
				};
			}
		}
		private void ReminderTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (LoginItem.IsReminderActive && DateTime.Parse(LoginItem.Username, System.Threading.Thread.CurrentThread.CurrentUICulture) <=
					DateTime.Now)
				{
					ReminderTimer.Stop();

					_ = new ReminderWindow
					{
						LoginListItem = this,
						Title = LoginItem.Name,
						_tbName = { Text = LoginItem.Name },
						_tbTime = { Text = LoginItem.Username }
					}.ShowDialog(App.MainWindow);
				}
				else if (!LoginItem.IsReminderActive) ReminderTimer.Stop();
			}
			catch
			{
				ReminderTimer.Stop();
				LoginItem.IsReminderActive = false;

				_ = MessageBox.Show(App.MainWindow, null, $"" +
					$"{(string)Application.Current.FindResource("ErrorElement1")} {LoginItem.Name}. {(string)Application.Current.FindResource("ErrorElement2")}", (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
			}
		}
	}
}
