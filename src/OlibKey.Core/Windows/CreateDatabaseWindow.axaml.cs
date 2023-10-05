using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using OlibKey.Core.ViewModels.Windows;
using PleasantUI.Controls;

namespace OlibKey.Core.Windows;

public partial class CreateDatabaseWindow : ContentDialog
{
    public CreateDatabaseViewModel ViewModel { get; private set; }
    
    public CreateDatabaseWindow()
    {
        InitializeComponent();

        ViewModel = new CreateDatabaseViewModel();
        DataContext = ViewModel;
        
        SetupDragAndDrop();
    }
    
    private void SetupDragAndDrop()
    {
        void DragEnter(object? s, DragEventArgs e)
        {
            e.DragEffects &= DragDropEffects.Copy | DragDropEffects.Link;
        }

        void Drop(object? s, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.Files))
            {
                IEnumerable<IStorageItem>? files = e.Data.GetFiles();

                if (files is not null)
                {
                    List<IStorageItem> filesList = new(files);

                    ViewModel.LoadImage(filesList[0].Path.LocalPath);
                }
            }
        }

        BorderImage.AddHandler(DragDrop.DragEnterEvent, DragEnter);
        BorderImage.AddHandler(DragDrop.DropEvent, Drop);
    }
}