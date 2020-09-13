using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using OlibKey.Controls.ColorPicker;
using OlibKey.Structures;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Controls.Primitives;
using OlibKey.ViewModels.Color;
using Avalonia.Interactivity;

namespace OlibKey.Views.Controls
{
	public class FolderListItem : UserControl
	{
		public TextBox tbName;
		public Border bLabelColor;
		private TextBlock _textName;
		private TextBlock _tbDeleteDate;
		public CheckBox SelectedItem;

		public ColorPicker colorPicker;

		public Popup pColorPicker;

		private ArgbColorViewModel _argbColorViewModel;

		public Folder FolderContext => DataContext as Folder;
		public FolderListItem() => InitializeComponent();
		public FolderListItem(Folder f)
        {
			InitializeComponent();

			DataContext = f;

			if (!string.IsNullOrEmpty(FolderContext.DeleteDate))
            {
				_tbDeleteDate.IsVisible = true;
				bLabelColor.IsEnabled = false;
				_tbDeleteDate.Text = $"{(string)Application.Current.FindResource("Removed")} {FolderContext.DeleteDate}";
            }

			if (string.IsNullOrEmpty(FolderContext.Color))
				FolderContext.Color = "#FF931D60";

			bLabelColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(FolderContext.Color));

			_argbColorViewModel = new ArgbColorViewModel
			{
				Hex = FolderContext.Color
			};

			pColorPicker.DataContext = _argbColorViewModel;
            colorPicker.ChangeColor += _colorPicker_ChangeColor;
            bLabelColor.PointerPressed += BLabelColor_PointerPressed;
        }

        private void BLabelColor_PointerPressed(object sender, PointerPressedEventArgs e) => pColorPicker.Open();

        private void _colorPicker_ChangeColor(object sender, RoutedEventArgs e)
        {
			FolderContext.Color = _argbColorViewModel.ToHexString();
			bLabelColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(_argbColorViewModel.ToHexString()));
        }

        private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			tbName = this.FindControl<TextBox>("tbName");
			_textName = this.FindControl<TextBlock>("textName");
			SelectedItem = this.FindControl<CheckBox>("selectedItem");
			_tbDeleteDate = this.FindControl<TextBlock>("tbDeleteDate");
			bLabelColor = this.FindControl<Border>("bLabelColor");
			pColorPicker = this.FindControl<Popup>("pColorPicker");
			colorPicker = this.FindControl<ColorPicker>("colorPicker");

			tbName.Focus();
			tbName.LostFocus += (s, e) => { tbName.IsVisible = false; FolderContext.Name = tbName.Text; _textName.Text = FolderContext.Name; };
			tbName.KeyDown += (s, e) =>
			{
				if (e.Key == Key.Enter)
				{
					tbName.IsVisible = false;
					FolderContext.Name = tbName.Text;
					_textName.Text = FolderContext.Name;
				}
			};
		}

        public async void Focusing()
		{
			tbName.IsVisible = true;
			await Task.Delay(50);
			tbName.Focus();
		}
	}
}
