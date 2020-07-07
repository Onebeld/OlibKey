using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Structures;
using System;

namespace OlibKey.Views.Controls
{
	public class CustomElementListItem : UserControl
	{
		public string ID;

		public Action<string> DeleteCustomElement;

		private StackPanel sectionOne;
		private StackPanel sectionTwo;
		private StackPanel sectionThree;
		private TextBox tbPassword;

		public Housing HousingElement { get; set; }

		public CustomElementListItem()
		{
			InitializeComponent();
		}
		public CustomElementListItem(Housing element)
		{
			InitializeComponent();
			DataContext = HousingElement = element;
			sectionOne = this.FindControl<StackPanel>("SectionOne");
			sectionTwo = this.FindControl<StackPanel>("SectionTwo");
			sectionThree = this.FindControl<StackPanel>("SectionThree");
			tbPassword = this.FindControl<TextBox>("tbPassword");

			switch (HousingElement.CustomElement.Type)
			{
				case 0:
					sectionTwo.IsVisible = false;
					sectionThree.IsVisible = false;
					break;
				case 1:
					sectionOne.IsVisible = false;
					sectionThree.IsVisible = false;
					break;
				case 2:
					sectionOne.IsVisible = false;
					sectionTwo.IsVisible = false;
					break;
			}
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void Delete(object sender, RoutedEventArgs e) => DeleteCustomElement?.Invoke(ID);

		private void CheckedPassword(object sender, RoutedEventArgs e)
		{
			CheckBox cb = (CheckBox)sender;
			tbPassword.PasswordChar = cb.IsChecked == true ? '\0' : '•';
		}
	}
	public class Housing
	{
		public CustomElement CustomElement { get; set; }
		public bool IsEnabled { get; set; }
	}
}
