using Avalonia.Controls;
using OlibKey.Core;
using OlibKey.Core.Views.MainWindowPages;

namespace OlibKey.Views;

public partial class PasswordManagerPage : UserControl
{
    public PasswordManagerPage()
    {
        InitializeComponent();

        if (OlibKeyApp.ViewModel.Database is null)
            Content = new CreateAndDecryptDatabasePage();
        else Content = new DatabasePage();
    }
}