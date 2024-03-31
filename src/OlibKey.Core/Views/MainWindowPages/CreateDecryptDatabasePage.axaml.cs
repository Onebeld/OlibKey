using Avalonia.Controls;
using OlibKey.Core.StaticMembers;
using OlibKey.Core.Structures;

namespace OlibKey.Core.Views.MainWindowPages;

public partial class CreateDecryptDatabasePage : UserControl
{
    public CreateDecryptDatabasePage()
    {
        InitializeComponent();

        Random random = new();
        TextBlockTip.Text = OlibKeyApp.GetLocalizationString(TextInformation.Tips[random.Next(0, TextInformation.Tips.Length)]);
    }
}