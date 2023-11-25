using OlibKey.Core.Enums;
using PleasantUI;

namespace OlibKey.Core.Structures;

public class Session : ViewModelBase
{
    private StorageType _storageType = StorageType.Disk;
    private Database? _database;
    private string? _pathToFile;

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
}