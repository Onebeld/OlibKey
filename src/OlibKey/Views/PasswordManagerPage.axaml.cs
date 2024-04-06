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

        if (OlibKeyApp.ViewModel.Session.Storage is null)
            Content = new CreateDecryptStoragePage();
        else Content = new StoragePage();
        
        OlibKeyApp.ViewModel.StorageCreated += ViewModelOnStorageCreated;
        OlibKeyApp.ViewModel.StorageOpened += ViewModelOnStorageOpened;
        OlibKeyApp.ViewModel.StorageUnlocked += ViewModelOnStorageCreated;
        OlibKeyApp.ViewModel.StorageBlocked += ViewModelOnStorageOpened;
    }

    private void ViewModelOnStorageOpened(object? sender, EventArgs e)
    {
        Content = new CreateDecryptStoragePage();
    }

    private void ViewModelOnStorageCreated(object? sender, EventArgs e)
    {
        Content = new StoragePage();
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        
        OlibKeyApp.ViewModel.StorageCreated -= ViewModelOnStorageCreated;
        OlibKeyApp.ViewModel.StorageOpened -= ViewModelOnStorageOpened;
    }
}