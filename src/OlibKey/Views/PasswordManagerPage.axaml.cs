using Avalonia;
using Avalonia.Controls;
using OlibKey.Core;
using OlibKey.Core.Views.MainWindowPages;

namespace OlibKey.Views;

public partial class PasswordManagerPage : UserControl
{
    public PasswordManagerPage()
    {
        InitializeComponent();

        if (OlibKeyApp.ViewModel.Session.Database is null)
            Content = new CreateDecryptDatabasePage();
        else Content = new DatabasePage();
        
        OlibKeyApp.ViewModel.DatabaseCreated += ViewModelOnDatabaseCreated;
        OlibKeyApp.ViewModel.DatabaseOpened += ViewModelOnDatabaseOpened;
        OlibKeyApp.ViewModel.DatabaseUnlocked += ViewModelOnDatabaseCreated;
        OlibKeyApp.ViewModel.DatabaseBlocked += ViewModelOnDatabaseOpened;
    }

    private void ViewModelOnDatabaseOpened(object? sender, EventArgs e)
    {
        Content = new CreateDecryptDatabasePage();
    }

    private void ViewModelOnDatabaseCreated(object? sender, EventArgs e)
    {
        Content = new DatabasePage();
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        
        OlibKeyApp.ViewModel.DatabaseCreated -= ViewModelOnDatabaseCreated;
        OlibKeyApp.ViewModel.DatabaseOpened -= ViewModelOnDatabaseOpened;
    }
}