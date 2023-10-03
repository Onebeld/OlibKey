using OlibKey.Core.Structures;
using PleasantUI;

namespace OlibKey.Core.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private Database? _database;

    private string _masterPassword = string.Empty;

    #region Properties

    public Database? Database
    {
        get => _database;
        private set => RaiseAndSet(ref _database, value);
    }

    #endregion
}