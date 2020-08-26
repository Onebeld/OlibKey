using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using OlibKey.Structures;
using System.Threading.Tasks;

namespace OlibKey.Views.Controls
{
	public class FolderListItem : UserControl
	{
		public TextBox _tbName;
		private TextBlock _textName;
		private TextBlock _tbDeleteDate;
		public CheckBox SelectedItem;

		public Folder FolderContext => DataContext as Folder;
		public FolderListItem() => InitializeComponent();
		public FolderListItem(Folder f)
        {
			InitializeComponent();
			DataContext = f;

			if (!string.IsNullOrEmpty(FolderContext.DeleteDate))
            {
				_tbDeleteDate.IsVisible = true;
				_tbDeleteDate.Text = $"{(string)Application.Current.FindResource("Removed")} {FolderContext.DeleteDate}";
            }
        }

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			_tbName = this.FindControl<TextBox>("tbName");
			_textName = this.FindControl<TextBlock>("textName");
			SelectedItem = this.FindControl<CheckBox>("selectedItem");
			_tbDeleteDate = this.FindControl<TextBlock>("tbDeleteDate");

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
		public async void Focusing()
		{
			_tbName.IsVisible = true;
			await Task.Delay(50);
			_tbName.Focus();
		}
	}
}
