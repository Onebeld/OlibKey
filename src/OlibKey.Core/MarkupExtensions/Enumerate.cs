using Avalonia.Markup.Xaml;

namespace OlibKey.Core.MarkupExtensions;

public class Enumerate : MarkupExtension
{
    public Enumerate(Type type)
    {
        Type = type;
    }
    
    public Type Type { get; set; }
    
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type.IsEnum)
        {
            return Type.GetEnumValues();
        }

        return new List<object>();
    }
}