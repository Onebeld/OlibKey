using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using OlibKey.Structures;

namespace OlibKey.Views.Controls
{
	public class FolderListItem : UserControl
	{
		public TextBox _tbName;
		private TextBlock _textName;

		public Folder FolderContext => DataContext as Folder;
		public FolderListItem() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			_tbName = this.FindControl<TextBox>("tbName");
			_textName = this.FindControl<TextBlock>("textName");

			_tbName.Focus();
			_tbName.LostFocus += (s, e) => { _tbName.IsVisible = false; FolderContext.Name = _tbName.Text; _textName.Text = FolderContext.Name; };
			_tbName.KeyDown += (s, e) =>
			{
				if (e.Key == Key.Enter)
				{
					_tbName.IsVisible = false;
					FolderContext.Name = _tbName.Text;
					_textName.Text = FolderContext.Name;
				}
			};
		}
		public void Focusing()
		{
			_tbName.IsVisible = true;
		}
	}
}
