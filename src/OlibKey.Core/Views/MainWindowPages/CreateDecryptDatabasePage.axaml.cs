using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using OlibKey.Core.StaticMembers;

namespace OlibKey.Core.Views.MainWindowPages;

public partial class CreateDecryptDatabasePage : UserControl
{
    public CreateDecryptDatabasePage()
    {
        InitializeComponent();

        Random random = new();
        TextBlockTip.Text = OlibKeyApp.GetLocalizationString(TextInformation.Tips[random.Next(0, TextInformation.Tips.Length)]);
        
        MasterPasswordTextBox.KeyUp += MasterPasswordTextBoxOnKeyUp;
    }

    private void MasterPasswordTextBoxOnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            OlibKeyApp.ViewModel.UnlockDatabase();
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        MasterPasswordTextBox.KeyUp -= MasterPasswordTextBoxOnKeyUp;
        
        base.OnDetachedFromLogicalTree(e);
    }
}