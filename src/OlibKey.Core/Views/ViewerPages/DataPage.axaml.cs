using Avalonia.Controls;
using Avalonia.Interactivity;
using OlibKey.Core.Enums;
using OlibKey.Core.Models.Database;
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
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        
        WebSiteTextBox.LostFocus -= WebSiteTextBoxOnLostFocus;
    }

    private void WebSiteTextBoxOnLostFocus(object? sender, RoutedEventArgs e)
    {
        ViewModel.Data.UpdateIcon();
    }
}