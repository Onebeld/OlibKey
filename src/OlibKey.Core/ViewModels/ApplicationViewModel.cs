using OlibKey.Core.Structures;
using OlibKey.Core.ViewModels.Windows;
using OlibKey.Core.Windows;
using PleasantUI;

namespace OlibKey.Core.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private Database? _database;

    private string _masterPassword = string.Empty;

    private bool _isDirty;

    #region Properties

    public Database? Database
    {
        get => _database;
        private set => RaiseAndSet(ref _database, value);
    }

    public bool IsDirty
    {
        get => _isDirty;
        set => RaiseAndSet(ref _isDirty, value);
    }

    #endregion

    public async void CreateDatabase()
    {
        CreateDatabaseWindow window = new CreateDatabaseWindow();
        window.Show(OlibKeyApp.MainWindow);
    }
}