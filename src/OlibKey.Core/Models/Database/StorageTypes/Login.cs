using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;
using OlibKey.Core.Helpers;

namespace OlibKey.Core.Models.Database.StorageTypes;

public class Login : Data
{
    private string? _favicon;
    private string? _username;
    private string? _email;
    private string? _password;
    private string? _webSite;
    private string? _secretKey;
    private bool _isActivatedTotp;
    
    public string? Favicon
    {
        get => _favicon;
        set => RaiseAndSet(ref _favicon, value);
    }
    
    public string? Username
    {
        get => _username;
        set => RaiseAndSetNullIfEmpty(ref _username, value);
    }

    public string? Email
    {
        get => _email;
        set => RaiseAndSetNullIfEmpty(ref _email, value);
    }

    public string? Password
    {
        get => _password;
        set => RaiseAndSetNullIfEmpty(ref _password, value);
    }

    public string? WebSite
    {
        get => _webSite;
        set => RaiseAndSetNullIfEmpty(ref _webSite, value);
    }

    public string? SecretKey
    {
        get => _secretKey;
        set => RaiseAndSet(ref _secretKey, value);
    }
    
    public bool IsActivatedTotp
    {
        get => _isActivatedTotp;
        set => RaiseAndSet(ref _isActivatedTotp, value);
    }

    public override async Task<IImage> GetIcon()
    {
        if (string.IsNullOrWhiteSpace(WebSite))
        {
            Favicon = null;
            return await Task.FromResult<IImage>((DrawingImage)Application.Current!.FindResource("GlobeIcon")!);
        }
                
        try
        {
            if (IsIconChange || string.IsNullOrEmpty(Favicon))
                Favicon = await ImageInteractions.DownloadFavicon(WebSite);

            IsIconChange = false;
                        
            using MemoryStream ms = new(Convert.FromBase64String(Favicon ?? throw new NullReferenceException()));
            return new Bitmap(ms);
        }
        catch
        { 
            Favicon = null;
            return await Task.FromResult<IImage>((DrawingImage)Application.Current!.FindResource("GlobeIcon")!);
        }
    }
    
    public override bool MatchesSearchCriteria(string text)
    {
        if (Username.IsDesiredString(text))
            return true;
        if (Email.IsDesiredString(text))
            return true;
        
        return base.MatchesSearchCriteria(text);
    }

    public override bool MatchesDataType(DataType dataType) => dataType is DataType.Login;

    [JsonIgnore]
    public override string? Information
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Username))
                return Username;
            if (!string.IsNullOrWhiteSpace(Email))
                return Email;

            return null;
        }
    }
}