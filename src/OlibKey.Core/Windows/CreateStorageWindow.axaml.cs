using Avalonia.Input;
using Avalonia.Platform.Storage;
using OlibKey.Core.ViewModels.Windows;
using PleasantUI.Controls;

namespace OlibKey.Core.Windows;

public partial class CreateStorageWindow : ContentDialog
{
	public CreateStorageViewModel ViewModel { get; }

	public CreateStorageWindow()
	{
		InitializeComponent();

		ViewModel = new CreateStorageViewModel();
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

					ViewModel.StorageSettings.LoadImage(filesList[0].Path.LocalPath);
				}
			}
		}

		BorderImage.AddHandler(DragDrop.DragEnterEvent, DragEnter);
		BorderImage.AddHandler(DragDrop.DropEvent, Drop);
	}
}