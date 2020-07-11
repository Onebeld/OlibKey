using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using OlibKey.Structures;
using OlibKey.Views.Windows;

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
		}

		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

	    public void EditedLogin()
	    {
		    if (LoginItem == null) return;
		    _tbLoginName.Text = LoginItem.Name;
		    _tbUsername.Text = LoginItem.Username;
	    }

		public async void GetIconElement()
		{
			if (LoginItem == null) return;
			if (LoginItem.Type == 0)
			{
				if (!string.IsNullOrEmpty(LoginItem.WebSite))
					IconLogin.Source = await Core.ImageInteraction.DownloadImage(LoginItem.WebSite);
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
					_ => IconLogin.Source
				};
			}
		}

	    private void ReminderTimer_Tick(object sender, EventArgs e)
	    {
		    try
		    {
			    if (DateTime.Parse(LoginItem.Username, System.Threading.Thread.CurrentThread.CurrentUICulture) <=
				    DateTime.Now && LoginItem.IsReminderActive)
			    {
				    ReminderTimer.Stop();

				    var reminderWindow = new ReminderWindow
				    {
					    LoginListItem = this,
						Title = LoginItem.Name,
						_tbName = { Text = LoginItem.Name},
						_tbTime = { Text = LoginItem.Username}
				    };
				    reminderWindow.ShowDialog(App.MainWindow);
			    }
			    else if (!LoginItem.IsReminderActive) ReminderTimer.Stop();
		    }
		    catch
		    {
			    ReminderTimer.Stop();
			    LoginItem.IsReminderActive = false;
		    }
	    }
	}
}
