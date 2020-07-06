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
    public class AccountListItem : UserControl
    {
	    public Image _image;
	    private readonly TextBlock _tbAccountName;
	    private readonly TextBlock _tbUsername;

		public string IDElement { get; set; }

		public DispatcherTimer timer;

	    public Account AccountItem { get; set; }

	    public AccountListItem() => InitializeComponent();

	    public AccountListItem(Account account)
	    {
		    InitializeComponent();

		    _image = this.FindControl<Image>("imageIconWebSite");
		    _tbAccountName = this.FindControl<TextBlock>("tbAccountName");
		    _tbUsername = this.FindControl<TextBlock>("tbUsername");

		    AccountItem = account;

			EditedAccount();

		    switch (AccountItem.TypeAccount)
		    {
			    case 3:

				    timer = new DispatcherTimer
				    {
					    Interval = new TimeSpan(0, 0, 3)
				    };
				    timer.Tick += Timer_Tick;

				    timer.Start();
				    break;
		    }
		}

		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

	    public void EditedAccount()
	    {
		    if (AccountItem == null) return;
		    _tbAccountName.Text = AccountItem.AccountName;
		    _tbUsername.Text = AccountItem.Username;
	    }

		public async void GetIconElement()
		{
			if (AccountItem == null) return;
			if (AccountItem.TypeAccount == 0)
			{
				if (!string.IsNullOrEmpty(AccountItem.WebSite))
					_image.Source = await Core.ImageInteraction.DownloadImage(AccountItem.WebSite);
				else
					_image.Source = (DrawingImage)Application.Current.FindResource("GlobeIcon");
			}
			else
			{
				_image.Source = AccountItem.TypeAccount switch
				{
					1 => (DrawingImage)Application.Current.FindResource("CardIcon"),
					2 => (DrawingImage)Application.Current.FindResource("PersonalDataIcon"),
					3 => (DrawingImage)Application.Current.FindResource("ReminderIcon"),
					_ => _image.Source
				};
			}
		}

	    private void Timer_Tick(object sender, EventArgs e)
	    {
		    try
		    {
			    if (DateTime.Parse(AccountItem.Username, System.Threading.Thread.CurrentThread.CurrentUICulture) <=
				    DateTime.Now && AccountItem.IsReminderActive)
			    {
				    timer.Stop();

				    var reminderWindow = new ReminderWindow
				    {
					    AccountListItem = this,
				    };
				    reminderWindow.ShowDialog(App.MainWindow);
			    }
			    else if (!AccountItem.IsReminderActive) timer.Stop();
		    }
		    catch
		    {
			    timer.Stop();
			    AccountItem.IsReminderActive = false;
		    }
	    }
	}
}
