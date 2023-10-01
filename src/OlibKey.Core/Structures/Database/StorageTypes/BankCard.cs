using PleasantUI;

namespace OlibKey.Core.Structures.StorageTypes;

public class BankCard : ViewModelBase, ICloneable
{
    private string? _typeBankCard;
    private string? _cardNumber;
    private string? _dateCard;
    private string? _securityCode;

    public string? TypeBankCard
    {
        get => _typeBankCard;
        set => RaiseAndSet(ref _typeBankCard, value);
    }

    public string? CardNumber
    {
        get => _cardNumber;
        set => RaiseAndSet(ref _cardNumber, value);
    }

    public string? DateCard
    {
        get => _dateCard;
        set => RaiseAndSet(ref _dateCard, value);
    }

    public string? SecurityCode
    {
        get => _securityCode;
        set => RaiseAndSet(ref _securityCode, value);
    }

    public object Clone() => MemberwiseClone();
}