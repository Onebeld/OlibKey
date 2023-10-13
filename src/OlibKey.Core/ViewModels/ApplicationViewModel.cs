using Avalonia.Controls.Notifications;
using OlibKey.Core.Structures;
using OlibKey.Core.Windows;
using PleasantUI;

namespace OlibKey.Core.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private Database? _database;

    private string _masterPassword = string.Empty;

    private bool _isDirty;
    private bool _isCreatedDatabase;

    #region EventHandlers

    public event EventHandler? DatabaseCreated;
    public event EventHandler? DatabaseOpened;
    public event EventHandler? DatabaseBlocking;
    public event EventHandler? DatabaseBlocked;
    public event EventHandler? DatabaseUnblocked;

    #endregion

    #region Properties
    
    public IManagedNotificationManager? NotificationManager { get; set; }

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

    public bool IsCreatedDatabase
    {
        get => _isCreatedDatabase;
        set => RaiseAndSet(ref _isCreatedDatabase, value);
    }

    #endregion

    public async void CreateDatabase()
    {
        if (OlibKeyApp.Main.OpenedModalWindows.Any(modalWindow => modalWindow is CreateDatabaseWindow))
            return;
        
        CreateDatabaseWindow window = new();
        bool result = await window.Show<bool>(OlibKeyApp.Main);

        if (!result) return;

        _masterPassword = window.ViewModel.MasterPassword;

        Database = new Database
        {
            Settings = new DatabaseSettings
            {
                Name = window.ViewModel.Name,
                ImageData = window.ViewModel.ImageData,
                Iterations = window.ViewModel.Iterations,
                UseTrashcan = window.ViewModel.UseTrashcan
            }
        };

        IsDirty = true;
        IsCreatedDatabase = true;
        
        DatabaseCreated?.Invoke(this, EventArgs.Empty);
    }
}