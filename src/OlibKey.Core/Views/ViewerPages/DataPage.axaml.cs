using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;
using OlibKey.Core.Models.StorageModels;
using OlibKey.Core.Models.StorageModels.StorageTypes;
using OlibKey.Core.ViewModels.ViewerPages;

namespace OlibKey.Core.Views.ViewerPages;

public partial class DataPage : UserControl
{
	private string? _cachedWebSite;
	
	public DataPageViewModel ViewModel { get; }

	public DataPage()
	{
		InitializeComponent();
		ViewModel = new DataPageViewModel(DataViewerMode.Create);
		DataContext = ViewModel;
	}

	public DataPage(Data data)
	{
		InitializeComponent();
		ViewModel = new DataPageViewModel(DataViewerMode.View, data);
		DataContext = ViewModel;
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);

		WebSiteTextBox.LostFocus += WebSiteTextBoxOnLostFocus;
		WebSiteTextBox.GotFocus += WebSiteTextBoxOnGotFocus;
		TagAutoCompleteBox.KeyUp += TagTextBoxOnKeyUp;

		TagAutoCompleteBox.ItemFilter = (search, item) => item is Tag tag && tag.Name.IsDesiredString(search);
		
		ImageBorder.PointerPressed += ImageBorderOnPointerPressed;
	}

	private void WebSiteTextBoxOnGotFocus(object? sender, GotFocusEventArgs e)
	{
		if (ViewModel.Data is not Login login) return;

		_cachedWebSite = login.WebSite;
	}

	protected override void OnUnloaded(RoutedEventArgs e)
	{
		base.OnUnloaded(e);

		WebSiteTextBox.LostFocus -= WebSiteTextBoxOnLostFocus;
		WebSiteTextBox.GotFocus -= WebSiteTextBoxOnGotFocus;
		TagAutoCompleteBox.KeyUp -= TagTextBoxOnKeyUp;
		
		ImageBorder.PointerPressed -= ImageBorderOnPointerPressed;
	}
	
	private void ImageBorderOnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (sender is Border border && !ViewModel.IsView) 
			FlyoutBase.ShowAttachedFlyout(border);
	}

	private void TagTextBoxOnKeyUp(object? sender, KeyEventArgs e)
	{
		if (e.Key == Key.Enter)
			ViewModel.AddTag();
	}

	private async void WebSiteTextBoxOnLostFocus(object? sender, RoutedEventArgs e)
	{
		if (ViewModel.Data is not Login login) return;
		
		if (login.WebSite == _cachedWebSite) return;
		
		await login.ChangeImage();
		login.UpdateIcon();
	}
}