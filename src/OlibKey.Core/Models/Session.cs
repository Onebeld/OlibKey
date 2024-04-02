using Avalonia.Threading;
using OlibKey.Core.Enums;
using OlibKey.Core.Models.DatabaseModels;
using OlibKey.Core.Structures;
using PleasantUI;

namespace OlibKey.Core.Models;

public class Session : ViewModelBase
{
    private StorageType _storageType = StorageType.None;
    private Database? _database;
    private string? _pathToFile;

    private string _currentMasterPassword;

    public Database? Database
    {
        get => _database;
        set => RaiseAndSet(ref _database, value);
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

    public bool CreateDatabase(DatabaseInfo databaseInfo)
    {
        _currentMasterPassword = databaseInfo.MasterPassword;

        Database = new Database
        {
            Settings = databaseInfo.DatabaseSettings
        };
        
        PathToFile = databaseInfo.Path;
        
        return true;
    }

    public void OpenDatabase(string path)
    {
        PathToFile = path;
        StorageType = StorageType.Disk;
    }

    public void LockDatabase()
    {
        RestartLockerTimer();
        
        if (PathToFile is null || Database is null) return;
        
        SaveDatabase();
    }
    
    public void UnlockDatabase(string masterPassword)
    {
        if (PathToFile is null) return;

        Database = Database.Load(PathToFile, masterPassword);

        _currentMasterPassword = masterPassword;
    }

    public void SaveDatabase()
    {
        RestartLockerTimer();
        
        if (PathToFile is null || Database is null) return;
        
        Database.Save(PathToFile, _currentMasterPassword);
        
        Database = null;
        _currentMasterPassword = string.Empty;
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