using System;
using Avalonia;
using Avalonia.Controls;
using OlibKey.Core;
using OlibKey.Core.Views.MainWindowPages;

namespace OlibKey.Android.Views;

public partial class PasswordManagerPage : UserControl
{
    public PasswordManagerPage()
    {
        InitializeComponent();
        
        if (OlibKeyApp.ViewModel.Database is null)
            Content = new CreateAndDecryptDatabasePage();
        else Content = new DatabasePage();
        
        OlibKeyApp.ViewModel.DatabaseCreated += ViewModelOnDatabaseCreated;
    }
    
    private void ViewModelOnDatabaseCreated(object? sender, EventArgs e)
    {
        Content = new DatabasePage();
    }
    
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        OlibKeyApp.ViewModel.DatabaseCreated -= ViewModelOnDatabaseCreated;
    }
}