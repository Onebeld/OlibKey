using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OlibKey.Core.Enums;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures.StorageTypes;
using PleasantUI;

namespace OlibKey.Core.Structures;

public class Data : ViewModelBase, ICloneable
{
    private DataType _type = DataType.Login;
    
    private string? _name;
    private string? _timeCreate;
    private string? _timeChanged;
    private string? _deleteDate;
    private uint _color;
    private bool _useColor;
    private string? _note;
    
    private bool _isFavorite;

    private AvaloniaList<string> _tags = new();
    
    private Login? _login = new();
    private BankCard? _bankCard = new();
    private PersonalData? _personalData = new();
    
    public DataType Type
    {
        get => _type;
        set => RaiseAndSet(ref _type, value);
    }
    
    public string? Name
    {
        get => _name;
        set => RaiseAndSet(ref _name, value);
    }
    
    public string? TimeCreate
    {
        get => _timeCreate;
        set => RaiseAndSet(ref _timeCreate, value);
    }

    public string? TimeChanged
    {
        get => _timeChanged;
        set => RaiseAndSet(ref _timeChanged, value);
    }
    
    public string? DeleteDate
    {
        get => _deleteDate;
        set => RaiseAndSet(ref _deleteDate, value);
    }

    public uint Color
    {
        get => _color;
        set => RaiseAndSet(ref _color, value);
    }

    public bool UseColor
    {
        get => _useColor;
        set => RaiseAndSet(ref _useColor, value);
    }
    
    public string? Note
    {
        get => _note;
        set => RaiseAndSet(ref _note, value);
    }
    
    public bool IsFavorite
    {
        get => _isFavorite;
        set => RaiseAndSet(ref _isFavorite, value);
    }

    public AvaloniaList<string> Tags
    {
        get => _tags;
        set => RaiseAndSet(ref _tags, value);
    }
    
    #region Types

    public Login? Login
    {
        get => _login;
        set => RaiseAndSet(ref _login, value);
    }
    
    public BankCard? BankCard
    {
        get => _bankCard;
        set => RaiseAndSet(ref _bankCard, value);
    }
    
    public PersonalData? PersonalData
    {
        get => _personalData;
        set => RaiseAndSet(ref _personalData, value);
    }

    #endregion
    
    #region JsonIgnored

    [JsonIgnore]
    public bool IsDeleted;
    
    [JsonIgnore]
    public bool IsIconChange;
    
    [JsonIgnore]
    public Task<IImage> Icon => GetIcon();

    #endregion
    
    private async Task<IImage> GetIcon()
    {
        if (Type is not DataType.Login || Login is null)
            return GetDataIcon(Type);

        if (string.IsNullOrWhiteSpace(Login.WebSite))
        {
            Login.Favicon = null;
            return GetDataIcon(DataType.Login);
        }
                
        try
        {
            if (IsIconChange || string.IsNullOrEmpty(Login.Favicon))
                Login.Favicon = await ImageInteractions.DownloadFavicon(Login.WebSite);

            IsIconChange = false;
                        
            using MemoryStream ms = new(Convert.FromBase64String(Login.Favicon ?? throw new NullReferenceException()));
            return new Bitmap(ms);
        }
        catch
        { 
            Login.Favicon = null;
            return GetDataIcon(DataType.Login);
        }
    }

    private static DrawingImage GetDataIcon(DataType dataType) => dataType switch
    {
        DataType.Login => (DrawingImage)Application.Current!.FindResource("GlobeIcon")!,
        DataType.BankCard => (DrawingImage)Application.Current!.FindResource("CardIcon")!,
        DataType.PersonalData => (DrawingImage)Application.Current!.FindResource("PersonalDataIcon")!,
        DataType.Notes => (DrawingImage)Application.Current!.FindResource("NoteIcon")!,
            
        _ => (DrawingImage)Application.Current!.FindResource("UnknownIcon")!
    };

    public object Clone()
    {
        Data data = (Data)MemberwiseClone();
        
        if (Login is not null)
            data.Login = (Login)Login.Clone();
        if (BankCard is not null)
            data.BankCard = (BankCard)BankCard.Clone();
        if (PersonalData is not null)
            data.PersonalData = (PersonalData)PersonalData.Clone();
        
        

        return data;
    }
}