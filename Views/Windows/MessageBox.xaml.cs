using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace OlibKey.Views.Windows
{
	public class MessageBox : Window
	{
		public enum MessageBoxButtons
		{
			Ok,
			OkCancel,
			YesNo,
			YesNoCancel
		}

		public enum MessageBoxIcon
		{
			Information,
			Error,
			Warning,
			Question
		}

		public enum MessageBoxResult
		{
			Ok,
			Cancel,
			Yes,
			No
		}

		public MessageBox()
		{
			AvaloniaXamlLoader.Load(this);
		}
		public static Task<MessageBoxResult> Show(Window parent, string textException , string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			var msgbox = new MessageBox()
			{
				Title = title
			};
			msgbox.FindControl<TextBlock>("Text").Text = text;
			var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");
			var iconControl = msgbox.FindControl<Image>("Icon");
			var errorText = msgbox.FindControl<TextBox>("ErrorText");

			var res = MessageBoxResult.Ok;

			void AddButton(string caption, MessageBoxResult r, bool def = false)
			{
				var btn = new Button { Content = caption };
				btn.Click += (_, __) => {
					res = r;
					msgbox.Close();
				};
				buttonPanel.Children.Add(btn);
				if (def)
					res = r;
			}

			void ChangeIcon(string icon) => iconControl.Source = (DrawingImage)Application.Current.FindResource($"{icon}Icon");

			switch (buttons)
			{
				case MessageBoxButtons.Ok:
				case MessageBoxButtons.OkCancel:
					AddButton((string)Application.Current.FindResource("Ok"), MessageBoxResult.Ok, true);
					break;
				case MessageBoxButtons.YesNo:
				case MessageBoxButtons.YesNoCancel:
					AddButton((string)Application.Current.FindResource("Yes"), MessageBoxResult.Yes);
					AddButton((string)Application.Current.FindResource("No"), MessageBoxResult.No, true);
					break;
			}

			if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
				AddButton((string)Application.Current.FindResource("Cancel"), MessageBoxResult.Cancel, true);

			switch (icon)
			{
				case MessageBoxIcon.Error:
					ChangeIcon("Error");
					break;
				case MessageBoxIcon.Information:
					ChangeIcon("Information");
					break;
				case MessageBoxIcon.Question:
					ChangeIcon("Question");
					break;
				case MessageBoxIcon.Warning:
					ChangeIcon("Warning");
					break;
			}

			if (textException != null)
			{
				errorText.Text = textException;
			}
			else errorText.IsVisible = false;

			var tcs = new TaskCompletionSource<MessageBoxResult>();
			msgbox.Closed += delegate { tcs.TrySetResult(res); };
			if (parent != null)
				msgbox.ShowDialog(parent);
			else msgbox.Show();
			return tcs.Task;
		}
	}
}
