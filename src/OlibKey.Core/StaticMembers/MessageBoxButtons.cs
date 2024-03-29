using PleasantUI.Core.Structures;

namespace OlibKey.Core.Structures;

public static class MessageBoxButtons
{
    public static readonly IReadOnlyList<MessageBoxButton> YesNo = new []
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("Yes"), Default = true, Result = "Yes", IsKeyDown = true
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("No"), Result = "No"
        }
    };
    
    public static readonly IReadOnlyList<MessageBoxButton> ReverseYesNo = new []
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("Yes"), Result = "Yes"
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("No"), Result = "No", IsKeyDown = true, Default = true
        }
    };

    public static readonly IReadOnlyList<MessageBoxButton> YesNoCancel = new []
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("Yes"), Default = true, Result = "Yes", IsKeyDown = true
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("No"), Result = "No"
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("Cancel"), Result = "Cancel"
        }
    };

    public static readonly IReadOnlyList<MessageBoxButton> Ok = new[]
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalizationString("Ok"), Default = true, Result = "Ok", IsKeyDown = true
        },
    };
}