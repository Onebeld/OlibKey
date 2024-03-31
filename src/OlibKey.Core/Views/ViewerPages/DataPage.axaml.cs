using Avalonia.Controls;
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
        ViewModel = new DataPageViewModel(DataViewerMode.Edit, data);
        DataContext = ViewModel;
    }
}