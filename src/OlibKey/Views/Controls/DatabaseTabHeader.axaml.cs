using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace OlibKey.Views.Controls
{
	public class DatabaseTabHeader : UserControl
	{
		public string TabID { get; set; }

		private Button _bCloseTab;

		public Image iLock;
		public Image iUnlock;

		public TextBlock tbNameDatabase;

		public Action<string> CloseTab;

		public DatabaseTabHeader() => InitializeComponent();

		public DatabaseTabHeader(string tabID, string nameDatabase)
		{
			InitializeComponent();
			TabID = tabID;
			tbNameDatabase.Text = nameDatabase;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			tbNameDatabase = this.FindControl<TextBlock>("tbNameDatabase");
			_bCloseTab = this.FindControl<Button>("bCloseTab");
			iLock = this.FindControl<Image>("iLock");
			iUnlock = this.FindControl<Image>("iUnlock");

			_bCloseTab.Click += (s, e) => { CloseTab?.Invoke(TabID); };
		}
	}
}
