using System.Globalization;
using System.Runtime.CompilerServices;
using OlibKey.Core.Enums;
using OlibKey.Core.Models;
using OlibKey.Core.Models.Database;
using OlibKey.Core.Models.Database.StorageTypes;
using OlibKey.Core.Views.ViewerPages;
using PleasantUI;

namespace OlibKey.Core.ViewModels.ViewerPages;

public class DataPageViewModel : ViewModelBase
{
    private DataType _selectedType = DataType.Login;
    private DataViewerMode _viewerMode;

    private Session _session = null!;
    private Data _data = null!;

    private string? _tagName;
    
    private OlibTotp _totp;

    #region Properties

    public Session Session
    {
        get => _session;
        set => RaiseAndSet(ref _session, value);
    }

    public Data Data
    {
        get => _data;
        set => RaiseAndSet(ref _data, value);
    }

    public string? TagName
    {
        get => _tagName;
        set => RaiseAndSet(ref _tagName, value);
    }
    
    public int SelectedTypeIndex
    {
        get => (int)_selectedType;
        set
        {
            _selectedType = (DataType)value;
            RaisePropertyChanged();
            
            ChangeDataType(_selectedType);
        }
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

    public bool IsView => _viewerMode is DataViewerMode.View;

    public bool IsEdit => _viewerMode is DataViewerMode.Edit;

    public bool IsCreate => _viewerMode is DataViewerMode.Create;

    #endregion

    public DataPageViewModel(DataViewerMode viewerMode, Data? selectedData = null)
    {
        Session = OlibKeyApp.ViewModel.Session;

        _viewerMode = viewerMode;

        switch (_viewerMode)
        {
            case DataViewerMode.Create:
                Data = new Login();
                break;
            case DataViewerMode.View:
                if (selectedData is null) throw new NullReferenceException();
                
                Data = (Data)selectedData.Clone();

                _selectedType = Data switch
                {
                    Login => DataType.Login,
                    BankCard => DataType.BankCard,
                    PersonalData => DataType.PersonalData,
                    Note => DataType.Note,
                    _ => _selectedType
                };
                
                RaisePropertyChanged(nameof(SelectedTypeIndex));

                if (Data is Login { IsActivatedTotp: true }) ;
                
                break;
            
            case DataViewerMode.Edit:
            default:
                throw new ArgumentOutOfRangeException(nameof(_viewerMode), _viewerMode, null);
        }
        
        Session.RestartLockerTimer();
        
        RaisePropertyChanged(nameof(IsView));
        RaisePropertyChanged(nameof(IsEdit));
        RaisePropertyChanged(nameof(IsCreate));
    }

    public void SaveData()
    {
        Session.RestartLockerTimer();

        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Database is null) throw new NullReferenceException();

        switch (_viewerMode)
        {
            case DataViewerMode.Create:
                Data.TimeCreate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                OlibKeyApp.ViewModel.Session.Database.Data.Add(Data);

                OlibKeyApp.ViewModel.ViewerContent = new OlibKeyPage();
                
                OlibKeyApp.ViewModel.DoSearch();
                break;
            case DataViewerMode.Edit:
                int index = DataIndex;

                /*if (Data.Type is DataType.Login && OlibKeyApp.ViewModel.Session.Database.Data[index].Type is DataType.Login)
                {
                    if (OlibKeyApp.ViewModel.Session.Database.Data[index].Login?.WebSite != Data.Login?.WebSite)
                        Data.IsIconChange = true;
                }

                if (Data.Type != OlibKeyApp.ViewModel.Session.Database.Data[index].Type)
                    Data.IsIconChange = true;*/

                Data.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                
                OlibKeyApp.ViewModel.Session.Database.Data[index] = Data;
                
                OlibKeyApp.ViewModel.DoSearch();
                OlibKeyApp.ViewModel.SelectedData = Data;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_viewerMode), _viewerMode, null);
        }

        OlibKeyApp.ViewModel.IsDirty = true;
    }

    public void ChangeData()
    {
        Session.RestartLockerTimer();

        _viewerMode = DataViewerMode.Edit;
        
        RaisePropertyChanged(nameof(IsView));
        RaisePropertyChanged(nameof(IsEdit));
        RaisePropertyChanged(nameof(IsCreate));
    }

    public void DeleteData()
    {
        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Database is null)
            throw new NullReferenceException();
        
        Session.RestartLockerTimer();

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

    public void AddTag()
    {
        if (string.IsNullOrWhiteSpace(TagName)) return;
        
        Data.Tags.Add(TagName);

        TagName = null;
    }

    public void DeleteTag(string tag)
    {
        Data.Tags.Remove(tag);
    }

    public void Cancel()
    {
        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Database is null)
            throw new NullReferenceException();
        
        Session.RestartLockerTimer();

        _viewerMode = DataViewerMode.View;

        Data = (Data)OlibKeyApp.ViewModel.Session.Database.Data[DataIndex].Clone();
        
        RaisePropertyChanged(nameof(IsView));
        RaisePropertyChanged(nameof(IsEdit));
        RaisePropertyChanged(nameof(IsCreate));
    }

    public void Back()
    {
        Session.RestartLockerTimer();
        
        if (OlibKeyApp.ViewModel.SelectedData is null)
            OlibKeyApp.ViewModel.ViewerContent = new OlibKeyPage();

        OlibKeyApp.ViewModel.SelectedData = null;
    }

    public async void CopyString(string str)
    {
        Session.RestartLockerTimer();
        
        // TODO: Copy to clipboard
    }

    private void ChangeDataType(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Login:
                Data = Data.ConvertData<Login>();
                break;
            case DataType.BankCard:
                Data = Data.ConvertData<BankCard>();
                break;
            case DataType.PersonalData:
                Data = Data.ConvertData<PersonalData>();
                break;
            case DataType.Note:
                Data = Data.ConvertData<Note>();
                break;
            
            case DataType.All:
            default:
                throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
        }
    }

    public void ActivateTotp()
    {
        if (Data is not Login login)
            return;
        
        if (string.IsNullOrWhiteSpace(login.SecretKey)) 
            return;

        try
        {
            _totp = new OlibTotp(login.SecretKey);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void DeactivateTotp()
    {
        if (Data is not Login)
            return;

        _totp.Dispose();
    }
}