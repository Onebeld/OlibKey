using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;
using OlibKey.Core.Structures;
using OlibKey.Core.Views.ViewerPages;
using OlibKey.Core.Windows;
using PleasantUI;
using PleasantUI.Extensions;
using PleasantUI.Reactive;

namespace OlibKey.Core.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private Session? _session;

    private string _masterPassword = string.Empty;
    private Data? _selectedData;

    private bool _isDirty;
    private bool _isCreatedDatabase;

    private string _searchText = string.Empty;
    private bool _searchByAllElements = true;
    private bool _searchByLogins;
    private bool _searchByBankCard;
    private bool _searchByPersonalData;
    private bool _searchByNotes;

    private AvaloniaList<Tag> _tags = new();
    private AvaloniaList<Tag> _selectedTags = new();
    private AvaloniaList<Data> _foundedDataList = new();

    private Control? _viewerContent;

    #region EventHandlers

    public event EventHandler? DatabaseCreated;
    public event EventHandler? DatabaseOpened;
    public event EventHandler? DatabaseBlocking;
    public event EventHandler? DatabaseBlocked;
    public event EventHandler? DatabaseUnblocked;

    #endregion

    #region Properties
    
    public IManagedNotificationManager? NotificationManager { get; set; }

    public DispatcherTimer? LockerTimer;

    public Session? Session
    {
        get => _session;
        set => RaiseAndSet(ref _session, value);
    }

    public Data? SelectedData
    {
        get => _selectedData;
        set => RaiseAndSet(ref _selectedData, value);
    }

    public bool IsDirty
    {
        get => _isDirty;
        set => RaiseAndSet(ref _isDirty, value);
    }

    public AvaloniaList<Tag> Tags
    {
        get => _tags;
        set => RaiseAndSet(ref _tags, value);
    }

    public AvaloniaList<Tag> SelectedTags
    {
        get => _selectedTags;
        set => RaiseAndSet(ref _selectedTags, value);
    }

    public AvaloniaList<Data> FoundedDataList
    {
        get => _foundedDataList;
        set => RaiseAndSet(ref _foundedDataList, value);
    }

    public Control? ViewerContent
    {
        get => _viewerContent;
        set => RaiseAndSet(ref _viewerContent, value);
    }

    public bool IsCreatedDatabase
    {
        get => _isCreatedDatabase;
        set => RaiseAndSet(ref _isCreatedDatabase, value);
    }

    public string SearchText
    {
        get => _searchText;
        set => RaiseAndSet(ref _searchText, value);
    }
    
    public bool SearchByAllElements
    {
        get => _searchByAllElements;
        set
        {
            RaiseAndSet(ref _searchByAllElements, value);
            
            if (!value) return;

            DoSearch();
                
            SearchByLogins = false;
            SearchByBankCard = false;
            SearchByPersonalData = false;
            SearchByNotes = false;
        }
    }

    public bool SearchByLogins
    {
        get => _searchByLogins;
        set
        {
            RaiseAndSet(ref _searchByLogins, value);
            
            if (!value) return;

            DoSearch();
                
            SearchByAllElements = false;
            SearchByBankCard = false;
            SearchByPersonalData = false;
            SearchByNotes = false;
        }
    }

    public bool SearchByBankCard
    {
        get => _searchByBankCard;
        set
        {
            RaiseAndSet(ref _searchByBankCard, value);
            
            if (!value) return;

            DoSearch();
                
            SearchByLogins = false;
            SearchByAllElements = false;
            SearchByPersonalData = false;
            SearchByNotes = false;
        }
    }

    public bool SearchByPersonalData
    {
        get => _searchByPersonalData;
        set
        {
            RaiseAndSet(ref _searchByPersonalData, value);
            
            if (!value) return;

            DoSearch();
                
            SearchByLogins = false;
            SearchByBankCard = false;
            SearchByAllElements = false;
            SearchByNotes = false;
        }
    }

    public bool SearchByNotes
    {
        get => _searchByNotes;
        set
        {
            RaiseAndSet(ref _searchByNotes, value);
            
            if (!value) return;

            DoSearch();
                
            SearchByLogins = false;
            SearchByBankCard = false;
            SearchByPersonalData = false;
            SearchByAllElements = false;
        }
    }

    #endregion

    public ApplicationViewModel()
    {
        this.WhenAnyValue(x => x.SearchText)
            .Skip(1)
            .Subscribe(_ => DoSearch());
    }

    public async void CreateDatabase()
    {
        if (OlibKeyApp.Main.OpenedModalWindows.Any(modalWindow => modalWindow is CreateDatabaseWindow))
            return;
        
        CreateDatabaseWindow window = new();
        bool result = await window.Show<bool>(OlibKeyApp.Main);

        if (!result) return;

        _masterPassword = window.ViewModel.MasterPassword;

        Database database = new()
        {
            Settings = new DatabaseSettings
            {
                Name = window.ViewModel.Name,
                ImageData = window.ViewModel.ImageData,
                Iterations = window.ViewModel.Iterations,
                UseTrashcan = window.ViewModel.UseTrashcan
            }
        };

        Session = new Session
        {
            Database = database,
            StorageType = StorageType.Disk,
            PathToFile = window.ViewModel.Path
        };

        IsDirty = true;
        IsCreatedDatabase = true;

        ViewerContent = new OlibKeyPage();
        
        DatabaseCreated?.Invoke(this, EventArgs.Empty);
    }

    public void AddData()
    {
        SelectedData = null;
        
    }

    public void LockDatabase()
    {
        
    }

    public void OpenDatabaseSettings()
    {
        
    }

    public void OpenTrashcan()
    {
        
    }

    public void OpenPasswordChecker()
    {
        
    }

    /*public void ActivateLockerTimer()
    {
        LockerTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMinutes(OlibKeySettings.Instance.LockoutTime)
        };
        LockerTimer.Tick += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(P))
                return;

            foreach (PleasantModalWindow modalWindow in OlibKeyApp.Main.OpenedModalWindows)
            {
                modalWindow.Close();
            }
            
            OlibKeyApp.ShowNotification();
        }
    }*/

    public void RestartLockerTimer()
    {
        if (LockerTimer is null || !LockerTimer.IsEnabled) return;
        
        LockerTimer.Stop();
        LockerTimer.Start();
    }

    public void GetAllTags()
    {
        if (Session?.Database is null) return;
        
        Tags.Clear();
        
        foreach (Data data in Session.Database.Data)
        {
            foreach (string tag in data.Tags)
            {
                Tag? reqTag = Tags.FirstOrDefault(x => x.Name == tag);

                if (reqTag is null)
                    Tags.Add(new Tag(tag));
                else
                    reqTag.Count += 1;
            }
        }
    }

    public void DoSearch()
    {
        if (Session?.Database is null) return;
        
        RestartLockerTimer();

        string lowerSearchText = SearchText.ToLower();

        List<Data> results = new(Session.Database.Data);

        if (SearchByLogins)
            results = results.FindAll(data => data.Type is DataType.Login);
        else if (SearchByBankCard)
            results = results.FindAll(data => data.Type is DataType.BankCard);
        else if (SearchByPersonalData)
            results = results.FindAll(data => data.Type is DataType.PersonalData);
        else if (SearchByNotes)
            results = results.FindAll(data => data.Type is DataType.Notes);

        if (!string.IsNullOrWhiteSpace(lowerSearchText))
        {
            results = results.FindAll(data => data.Name.IsRightElement(lowerSearchText)
                                           || data.Login is not null && (data.Login.Username.IsRightElement(lowerSearchText) || data.Login.Email.IsRightElement(lowerSearchText))
                                           || data.BankCard is not null && data.BankCard.CardNumber.IsRightElement(lowerSearchText)
                                           || data.PersonalData is not null && data.PersonalData.Fullname.IsRightElement(lowerSearchText)).ToList();
        }

        List<Data> resultsFromTags = new();

        foreach (Tag tag in SelectedTags)
            resultsFromTags.AddRange(results.FindAll(data => data.Tags.Any(x1 => x1 == tag.Name)));

        results = resultsFromTags.Distinct().ToList();

        FoundedDataList = new AvaloniaList<Data>(results);
    }

    public void Save()
    {
        if (Session?.PathToFile is null) return;
        
        Session?.Database?.Save(Session.PathToFile, _masterPassword);
    }
}