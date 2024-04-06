using OlibKey.Core.Helpers;
using OlibKey.Core.Settings;
using OlibKey.Core.StaticMembers;
using OlibKey.Core.Structures;
using PleasantUI;
using PleasantUI.Controls;

namespace OlibKey.Core.ViewModels.Windows;

public class CreateStorageViewModel : ViewModelBase
{
	private string _masterPassword = string.Empty;

	private StorageSettings _storageSettings = new();

	private string _path = string.Empty;

	#region Properties

	public string MasterPassword
	{
		get => _masterPassword;
		set => RaiseAndSet(ref _masterPassword, value);
	}

	public StorageSettings StorageSettings
	{
		get => _storageSettings;
		set => RaiseAndSet(ref _storageSettings, value);
	}

	public string Path
	{
		get => _path;
		set => RaiseAndSet(ref _path, value);
	}

	#endregion

	public async void SelectPath()
	{
		string? storagePath = await StorageProvider.SaveFile(pickerFileTypes: FileTypes.Olib,
			defaultExtension: "olib",
			suggestedFileName: "NewStorage");

		if (storagePath is null) return;

		Path = storagePath;
	}

	public async void LoadImage()
	{
		string? imagePath = await StorageProvider.SelectFile(pickerFileTypes: FileTypes.Images);

		if (imagePath is null) return;

		StorageSettings.LoadImage(imagePath);
	}

	public void CloseWithResult(ContentDialog dialog)
	{
		if (string.IsNullOrWhiteSpace(MasterPassword) || string.IsNullOrWhiteSpace(Path) ||
		    string.IsNullOrWhiteSpace(StorageSettings.Name))
			return;

		StorageInfo storageInfo = new()
		{
			MasterPassword = _masterPassword,
			Path = _path,
			StorageSettings = StorageSettings
		};

		dialog.Close(storageInfo);
	}

	public void Close(ContentDialog dialog)
	{
		dialog.Close(null);
	}
}