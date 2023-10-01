using PleasantUI;

namespace OlibKey.Core.Structures.StorageTypes;

public class Login : ViewModelBase, ICloneable
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
        set => RaiseAndSet(ref _username, value);
    }
    
    public string? Email
    {
        get => _email;
        set => RaiseAndSet(ref _email, value);
    }
    
    public string? Password
    {
        get => _password;
        set => RaiseAndSet(ref _password, value);
    }
    
    public string? WebSite
    {
        get => _webSite;
        set => RaiseAndSet(ref _webSite, value);
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
    
    public object Clone() => MemberwiseClone();
}