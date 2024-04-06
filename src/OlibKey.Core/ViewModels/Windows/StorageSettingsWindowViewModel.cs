using OlibKey.Core.Helpers;
using OlibKey.Core.Settings;
using OlibKey.Core.StaticMembers;
using PleasantUI;
using PleasantUI.Controls;

namespace OlibKey.Core.ViewModels.Windows;

public class StorageSettingsWindowViewModel : ViewModelBase
{
	private StorageSettings _storageSettings;
	
	public StorageSettings StorageSettings
	{
		get => _storageSettings;
		set => RaiseAndSet(ref _storageSettings, value);
	}

	public StorageSettingsWindowViewModel(StorageSettings storageSettings)
	{
		StorageSettings = (StorageSettings)storageSettings.Clone();
	}
	
	public async void LoadImage()
	{
		string? imagePath = await StorageProvider.SelectFile(pickerFileTypes: FileTypes.Images);
        
		if (imagePath is null) return;
        
		StorageSettings.LoadImage(imagePath);
	}
	
	public void CloseWithResult(ContentDialog dialog)
	{
		if (string.IsNullOrWhiteSpace(StorageSettings.Name))
			return;
        
		dialog.Close(StorageSettings);
	}

	public void Close(ContentDialog dialog)
	{
		dialog.Close(null);
	}
}