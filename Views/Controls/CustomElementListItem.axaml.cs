using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Structures;
using System;

namespace OlibKey.Views.Controls
{
	public class CustomFieldListItem : UserControl
	{
		public string ID;

		public Action<string> DeleteCustomField;

		private StackPanel _sectionOne;
		private StackPanel _sectionTwo;
		private StackPanel _sectionThree;
		private TextBox _tbPassword;
		public Separator SLine;
		public Housing HousingElement { get; set; }

		public CustomFieldListItem() => InitializeComponent();
		public CustomFieldListItem(Housing element)
		{
			InitializeComponent();
			DataContext = HousingElement = element;
			_sectionOne = this.FindControl<StackPanel>("SectionOne");
			_sectionTwo = this.FindControl<StackPanel>("SectionTwo");
			_sectionThree = this.FindControl<StackPanel>("SectionThree");
			_tbPassword = this.FindControl<TextBox>("tbPassword");
			SLine = this.FindControl<Separator>("sLine");

			switch (HousingElement.CustomField.Type)
			{
				case 0:
					_sectionTwo.IsVisible = false;
					_sectionThree.IsVisible = false;
					break;
				case 1:
					_sectionOne.IsVisible = false;
					_sectionThree.IsVisible = false;
					break;
				case 2:
					_sectionOne.IsVisible = false;
					_sectionTwo.IsVisible = false;
					break;
			}
		}

		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

		private void Delete(object sender, RoutedEventArgs e) => DeleteCustomField?.Invoke(ID);

		private void CheckedPassword(object sender, RoutedEventArgs e)
		{
			CheckBox cb = (CheckBox)sender;
			_tbPassword.PasswordChar = cb.IsChecked == true ? '\0' : '•';
		}
	}
	public class Housing
	{
		public CustomField CustomField { get; set; }
		public bool IsEnabled { get; set; }
		public bool IsVisibleLine { get; set; } = true;
	}
}
