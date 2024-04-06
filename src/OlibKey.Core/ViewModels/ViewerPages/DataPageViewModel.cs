using System.Globalization;
using Avalonia.Controls.Notifications;
using OlibKey.Core.Enums;
using OlibKey.Core.Models;
using OlibKey.Core.Models.StorageModels;
using OlibKey.Core.Models.StorageModels.StorageTypes;
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

    public OlibTotp Totp
    {
        get => _totp;
        set => RaiseAndSet(ref _totp, value);
    }

    private int DataIndex
    {
        get
        {
            if (OlibKeyApp.ViewModel.Session is null || 
                OlibKeyApp.ViewModel.Session.Storage is null || 
                OlibKeyApp.ViewModel.SelectedData is null)
                throw new NullReferenceException();

            return OlibKeyApp.ViewModel.Session.Storage.Data.IndexOf(OlibKeyApp.ViewModel.SelectedData);
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

                if (Data is Login { IsActivatedTotp: true }) 
                    ActivateTotp();
                
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

        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Storage is null) throw new NullReferenceException();

        switch (_viewerMode)
        {
            case DataViewerMode.Create:
                Data.TimeCreate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                Session.Storage?.Data.Add(Data);

                OlibKeyApp.ViewModel.ViewerContent = new OlibKeyPage();
                
                Session.Storage?.UpdateInfo();
                OlibKeyApp.ViewModel.DoSearch();
                break;
            case DataViewerMode.Edit:
                int index = DataIndex;

                /*if (Data.Type is DataType.Login && OlibKeyApp.ViewModel.Session.StorageModels.Data[index].Type is DataType.Login)
                {
                    if (OlibKeyApp.ViewModel.Session.StorageModels.Data[index].Login?.WebSite != Data.Login?.WebSite)
                        Data.IsIconChange = true;
                }

                if (Data.Type != OlibKeyApp.ViewModel.Session.StorageModels.Data[index].Type)
                    Data.IsIconChange = true;*/

                Data.TimeChanged = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                
                OlibKeyApp.ViewModel.Session.Storage.Data[index] = Data;
                
                Session.Storage?.UpdateInfo();
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
        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Storage is null)
            throw new NullReferenceException();
        
        Session.RestartLockerTimer();

        int index = DataIndex;

        if (OlibKeyApp.ViewModel.Session.Storage.Settings.UseTrashcan)
        {
            Data data = OlibKeyApp.ViewModel.Session.Storage.Data[index];
            data.DeleteDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            OlibKeyApp.ViewModel.Session.Storage.Trashcan.Data.Add(data);
        }
        
        OlibKeyApp.ViewModel.Session.Storage.Data.RemoveAt(index);

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
        if (OlibKeyApp.ViewModel.Session is null || OlibKeyApp.ViewModel.Session.Storage is null)
            throw new NullReferenceException();
        
        Session.RestartLockerTimer();

        _viewerMode = DataViewerMode.View;

        Data = (Data)OlibKeyApp.ViewModel.Session.Storage.Data[DataIndex].Clone();
        
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

        if (await OlibKeyApp.CopyStringToClipboard(str))
            OlibKeyApp.ShowNotification("Successful", "FieldCopied", NotificationType.Information);
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
            Totp = new OlibTotp(login.SecretKey);

            login.IsActivatedTotp = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void DeactivateTotp()
    {
        if (Data is not Login login)
            return;

        Totp.Dispose();

        login.IsActivatedTotp = false;
    }
}