using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using OlibKey.Core.Enums;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models;
using OlibKey.Core.Models.Database;
using OlibKey.Core.StaticMembers;
using OlibKey.Core.Structures;
using OlibKey.Core.Views.ViewerPages;
using OlibKey.Core.Windows;
using PleasantUI;
using PleasantUI.Controls;
using PleasantUI.Extensions;
using PleasantUI.Reactive;

namespace OlibKey.Core.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
    private Session _session;

    private Data? _selectedData;
    private DataType _selectedDataType = DataType.All;

    private bool _isDirty;

    private string _searchText = string.Empty;

    private AvaloniaList<Tag> _tags = [];
    private AvaloniaList<Tag> _selectedTags = [];
    private AvaloniaList<Data> _foundedDataList = [];

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

    public Session Session
    {
        get => _session;
        set => RaiseAndSet(ref _session, value);
    }

    public Data? SelectedData
    {
        get => _selectedData;
        set
        {
            RaiseAndSet(ref _selectedData, value);
            
            ShowData(value);
        }
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

    public string SearchText
    {
        get => _searchText;
        set => RaiseAndSet(ref _searchText, value);
    }

    public DataType SelectedDataType
    {
        get => _selectedDataType;
        set
        {
            RaiseAndSet(ref _selectedDataType, value);
            
            DoSearch();
        }
    }

    #endregion

    public ApplicationViewModel()
    {
        this.WhenAnyValue(x => x.SearchText)
            .Skip(1)
            .Subscribe(_ => DoSearch());

        Session = new Session();
    }

    public async void CreateDatabase()
    {
        if (OlibKeyApp.Main.ModalWindows.Any(modalWindow => modalWindow is CreateDatabaseWindow))
            return;
        
        CreateDatabaseWindow window = new();
        DatabaseInfo? result = await window.Show<DatabaseInfo?>(OlibKeyApp.Main);

        if (result is null) return;

        Session.CreateDatabase(result.Value);

        IsDirty = true;

        ViewerContent = new OlibKeyPage();
        
        DatabaseCreated?.Invoke(this, EventArgs.Empty);
    }

    public async void OpenDatabase()
    {
        string? path = await StorageProvider.SelectFile(pickerFileTypes: FileTypes.Olib);
        
        if (path is null) return;

        Session.OpenDatabase(path);
        
        // TODO: Open database
    }

    public void AddData()
    {
        SelectedData = null;
        
        // TODO: Add data
        ViewerContent = new DataPage();
    }

    public void ShowData(Data? data)
    {
        if (data is null)
        {
            ViewerContent = new OlibKeyPage();
            return;
        }
        
        ViewerContent = new DataPage(data);
    }

    public void LockDatabase()
    {
        DatabaseBlocking?.Invoke(this, EventArgs.Empty);
        
        Session.LockDatabase();
        
        DatabaseBlocked?.Invoke(this, EventArgs.Empty);
    }

    public void OpenDatabaseSettings()
    {
        Session.RestartLockerTimer();
        
        // TODO: Open database settings
    }

    public void OpenTrashcanWindow()
    {
        Session.RestartLockerTimer();
        
        // TODO: Open trashcan
    }

    public void OpenPasswordCheckerWindow()
    {
        Session.RestartLockerTimer();
        
        // TODO: Open password checker
    }

    public void ActivateLockerTimer()
    {
        Session.ActivateLockerTimer(OlibKeySettings.Instance.LockoutTime, TickLockerTimer);
        
        // TODO: Activate locker timer
    }

    private void TickLockerTimer(object? sender, EventArgs e)
    {
        foreach (PleasantModalWindow modalWindow in OlibKeyApp.Main.ModalWindows) 
            modalWindow.Close();
        
        LockDatabase();
    }

    public void GetAllTags()
    {
        if (Session.Database is null) return;
        
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
        if (Session.Database is null) return;
        
        Session.RestartLockerTimer();

        string lowerSearchText = SearchText.ToLower();

        List<Data> results = new(Session.Database.Data);

        if (SelectedDataType is not DataType.All)
            results = results.FindAll(data => data.MatchesDataType(SelectedDataType));

        if (!string.IsNullOrWhiteSpace(lowerSearchText)) 
            results = results.FindAll(data => data.MatchesSearchCriteria(lowerSearchText)).ToList();

        List<Data> resultsFromTags = [];

        if (SelectedTags.Count > 0)
        {
            foreach (Tag tag in SelectedTags)
                resultsFromTags.AddRange(results.FindAll(data => data.Tags.Any(x1 => x1 == tag.Name)));

            results = resultsFromTags.Distinct().ToList();
        }
        
        FoundedDataList = new AvaloniaList<Data>(results);
    }

    public void Save()
    {
        Session.SaveDatabase();
    }
}