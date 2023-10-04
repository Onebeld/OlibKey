using PleasantUI.Core.Structures;

namespace OlibKey.Core.Structures;

public static class MessageBoxButtons
{
    public static readonly IReadOnlyList<MessageBoxButton> YesNo = new []
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("Yes"), Default = true, Result = "Yes", IsKeyDown = true
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("No"), Result = "No"
        }
    };
    
    public static readonly IReadOnlyList<MessageBoxButton> ReverseYesNo = new []
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("Yes"), Result = "Yes"
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("No"), Result = "No", IsKeyDown = true, Default = true
        }
    };

    public static readonly IReadOnlyList<MessageBoxButton> YesNoCancel = new []
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("Yes"), Default = true, Result = "Yes", IsKeyDown = true
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("No"), Result = "No"
        },
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("Cancel"), Result = "Cancel"
        }
    };

    public static readonly IReadOnlyList<MessageBoxButton> Ok = new[]
    {
        new MessageBoxButton
        {
            Text = OlibKeyApp.GetLocalString("Ok"), Default = true, Result = "Ok", IsKeyDown = true
        },
    };
}