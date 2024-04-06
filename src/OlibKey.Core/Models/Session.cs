using Avalonia.Threading;
using OlibKey.Core.Enums;
using OlibKey.Core.Models.StorageModels;
using OlibKey.Core.Structures;
using PleasantUI;

namespace OlibKey.Core.Models;

public class Session : ViewModelBase
{
	private StorageType _storageType = StorageType.None;
	private Storage? _storage;
	private string? _pathToFile;

	private string _currentMasterPassword;

	public Storage? Storage
	{
		get => _storage;
		set => RaiseAndSet(ref _storage, value);
	}

	public StorageType StorageType
	{
		get => _storageType;
		set => RaiseAndSet(ref _storageType, value);
	}

	public string? PathToFile
	{
		get => _pathToFile;
		set => RaiseAndSet(ref _pathToFile, value);
	}

	private DispatcherTimer? LockerTimer { get; set; }

	public bool CreateStorage(StorageInfo storageInfo)
	{
		_currentMasterPassword = storageInfo.MasterPassword;

		Storage = new Storage
		{
			Settings = storageInfo.StorageSettings
		};

		PathToFile = storageInfo.Path;

		return true;
	}

	public void OpenStorage(string path)
	{
		PathToFile = path;
		StorageType = StorageType.Disk;
	}

	public void LockStorage()
	{
		RestartLockerTimer();

		if (PathToFile is null || Storage is null) return;

		SaveStorage();

		Storage = null;
		_currentMasterPassword = string.Empty;
	}

	public void UnlockStorage(string masterPassword)
	{
		if (PathToFile is null) return;

		Storage = Storage.Load(PathToFile, masterPassword);

		_currentMasterPassword = masterPassword;
	}

	public void SaveStorage()
	{
		RestartLockerTimer();

		if (PathToFile is null || Storage is null) return;

		Storage.Save(PathToFile, _currentMasterPassword);
	}

	public void ActivateLockerTimer(int minutes, params EventHandler[] tickActions)
	{
		LockerTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMinutes(minutes)
		};

		foreach (EventHandler tickAction in tickActions)
			LockerTimer.Tick += tickAction;

		// TODO: Activate locker timer
	}

	public void RestartLockerTimer()
	{
		if (LockerTimer is null || !LockerTimer.IsEnabled) return;

		LockerTimer.Stop();
		LockerTimer.Start();
	}
}