using System.Globalization;
using Avalonia.Threading;
using OlibKey.Core.Enums;
using OlibKey.Core.Structures;
using OlibKey.Core.Structures.StorageTypes;
using OlibKey.Core.Views.ViewerPages;
using OtpNet;
using PleasantUI;

namespace OlibKey.Core.ViewModels.ViewerPages;

public class DataPageViewModel : ViewModelBase
{
    private DataViewerMode _viewerMode;

    private Database _database = null!;
    private Data _data = null!;
    
    private Totp? _totp;
    private DispatcherTimer? _totpTimer;

    #region Properties

    public Database Database
    {
        get => _database;
        set => RaiseAndSet(ref _database, value);
    }

    public Data Data
    {
        get => _data;
        set => RaiseAndSet(ref _data, value);
    }

    private int DataIndex
    {
        get
        {
            if (OlibKeyApp.ViewModel.Session is null || 
                OlibKeyApp.ViewModel.Session.Database is null || 
                OlibKeyApp.ViewModel.SelectedData is null)
                throw new NullReferenceException();

            return OlibKeyApp.ViewModel.Session.Database.Data.IndexOf(OlibKeyApp.ViewModel.SelectedData);
        }
    }

    public bool IsView { get => _viewerMode is DataViewerMode.View; }
    
    public bool IsEdit { get => _viewerMode is DataViewerMode.Edit; }
    
    public bool IsCreate { get => _viewerMode is DataViewerMode.Create; }

    #endregion

    public DataPageViewModel(DataViewerMode viewerMode, Data? selectedData = null)
    {
        Database = OlibKeyApp.ViewModel.Session?.Database ?? throw new NullReferenceException();

        _viewerMode = viewerMode;

        switch (_viewerMode)
        {
            case DataViewerMode.Create:
                Data = new Data();
                break;
            case DataViewerMode.View:
                if (selectedData is null) throw new NullReferenceException();
                
                Data = (Data)selectedData.Clone();

                Data.Login ??= new Login();
                Data.BankCard ??= new BankCard();
                Data.PersonalData ??= new PersonalData();

                if (Data.Login.IsActivatedTotp) ;
                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_viewerMode), _viewerMode, null);
        }
        
        ApplicationViewModel.RestartLockerTimer();
        
        RaisePropertyChanged(nameof(IsView));
        RaisePropertyChanged(nameof(IsEdit));
        RaisePropertyChanged(nameof(IsCreate));
    }

    public void SaveData()
    {
        ApplicationViewModel.RestartLockerTimer();

        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Database is null) throw new NullReferenceException();

        switch (Data.Type)
        {
            case DataType.BankCard:
                Data.Login = null;
                Data.PersonalData = null;
                break;
            case DataType.PersonalData:
                Data.BankCard = null;
                Data.Login = null;
                break;
            case DataType.Notes:
                Data.BankCard = null;
                Data.PersonalData = null;
                Data.Login = null;
                break;
            
            default:
                Data.BankCard = null;
                Data.PersonalData = null;
                break;
        }

        switch (_viewerMode)
        {
            case DataViewerMode.Create:
                Data.TimeCreate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                OlibKeyApp.ViewModel.Session.Database.Data.Add(Data);

                OlibKeyApp.ViewModel.ViewerContent = new OlibKeyPage();
                break;
            case DataViewerMode.Edit:
                int index = DataIndex;

                if (Data.Type is DataType.Login && OlibKeyApp.ViewModel.Session.Database.Data[index].Type is DataType.Login)
                {
                    if (OlibKeyApp.ViewModel.Session.Database.Data[index].Login?.WebSite != Data.Login?.WebSite)
                        Data.IsIconChange = true;
                }

                if (Data.Type != OlibKeyApp.ViewModel.Session.Database.Data[index].Type)
                    Data.IsIconChange = true;

                Data.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                
                OlibKeyApp.ViewModel.Session.Database.Data[index] = Data;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_viewerMode), _viewerMode, null);
        }

        OlibKeyApp.ViewModel.IsDirty = true;
        OlibKeyApp.ViewModel.DoSearch();
    }

    public void ChangeData()
    {
        ApplicationViewModel.RestartLockerTimer();

        _viewerMode = DataViewerMode.Edit;
        
        RaisePropertyChanged(nameof(IsView));
        RaisePropertyChanged(nameof(IsEdit));
        RaisePropertyChanged(nameof(IsCreate));
    }

    public void DeleteData()
    {
        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Database is null)
            throw new NullReferenceException();
        
        ApplicationViewModel.RestartLockerTimer();

        int index = DataIndex;

        if (OlibKeyApp.ViewModel.Session.Database.Settings.UseTrashcan)
        {
            Data data = OlibKeyApp.ViewModel.Session.Database.Data[index];
            data.DeleteDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            OlibKeyApp.ViewModel.Session.Database.Trashcan.Data.Add(data);
        }
        
        OlibKeyApp.ViewModel.Session.Database.Data.RemoveAt(index);

        OlibKeyApp.ViewModel.IsDirty = true;
        OlibKeyApp.ViewModel.DoSearch();
    }

    public void Cancel()
    {
        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Database is null)
            throw new NullReferenceException();

        
        ApplicationViewModel.RestartLockerTimer();

        _viewerMode = DataViewerMode.View;

        Data = (Data)OlibKeyApp.ViewModel.Session.Database.Data[DataIndex].Clone();
        
        RaisePropertyChanged(nameof(IsView));
        RaisePropertyChanged(nameof(IsEdit));
        RaisePropertyChanged(nameof(IsCreate));
    }

    public void Back()
    {
        ApplicationViewModel.RestartLockerTimer();
        
        if (OlibKeyApp.ViewModel.SelectedData is null)
            OlibKeyApp.ViewModel.ViewerContent = new OlibKeyPage();

        OlibKeyApp.ViewModel.SelectedData = null;

    }

    public async void CopyString(string str)
    {
        ApplicationViewModel.RestartLockerTimer();
        
        
    }

    #region TOTP

    public void ActivateTotp()
    {
        if (Data.Login is null || string.IsNullOrWhiteSpace(Data.Login.SecretKey))
            return;
        
        if (_totpTimer is not null || _totpTimer.IsEnabled)
            _totpTimer.Stop();

        try
        {
            _totp = new Totp(Base32Encoding.ToBytes(Data.Login.SecretKey.Replace(" ", "")),
                step: 30,
                timeCorrection: new TimeCorrection(DateTime.UtcNow));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }

    public void DeactivateTotp()
    {
        if (_totpTimer is null || _totp is null || Data.Login is null)
            return;
        
        _totpTimer.Stop();
        _totp = null;
        
        
    }

    public void OnTickTotpTimer(object? sender, EventArgs e)
    {
        if (_totp is null)
            return;
        
        
    }

    #endregion
}