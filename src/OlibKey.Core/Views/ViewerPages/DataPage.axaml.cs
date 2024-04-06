using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;
using OlibKey.Core.Models.StorageModels;
using OlibKey.Core.ViewModels.ViewerPages;

namespace OlibKey.Core.Views.ViewerPages;

public partial class DataPage : UserControl
{
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
        TagAutoCompleteBox.KeyUp += TagTextBoxOnKeyUp;

        TagAutoCompleteBox.ItemFilter = (search, item) => item is Tag tag && tag.Name.IsDesiredString(search);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        
        WebSiteTextBox.LostFocus -= WebSiteTextBoxOnLostFocus;
        TagAutoCompleteBox.KeyUp -= TagTextBoxOnKeyUp;
    }
    
    private void TagTextBoxOnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            ViewModel.AddTag();
    }

    private void WebSiteTextBoxOnLostFocus(object? sender, RoutedEventArgs e)
    {
        ViewModel.Data.UpdateIcon();
    }
}