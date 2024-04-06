using Avalonia.Input;
using Avalonia.Platform.Storage;
using OlibKey.Core.Settings;
using OlibKey.Core.ViewModels.Windows;
using PleasantUI.Controls;

namespace OlibKey.Core.Windows;

public partial class StorageSettingsWindow : ContentDialog
{
	public StorageSettingsWindowViewModel ViewModel { get; }
	
	public StorageSettingsWindow()
	{
		InitializeComponent();
	}

	public StorageSettingsWindow(StorageSettings storageSettings)
	{
		InitializeComponent();

		ViewModel = new StorageSettingsWindowViewModel(storageSettings);
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