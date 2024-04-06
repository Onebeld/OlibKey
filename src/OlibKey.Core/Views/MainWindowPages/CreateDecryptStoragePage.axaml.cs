using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using OlibKey.Core.StaticMembers;

namespace OlibKey.Core.Views.MainWindowPages;

public partial class CreateDecryptStoragePage : UserControl
{
    public CreateDecryptStoragePage()
    {
        InitializeComponent();

        Random random = new();
        TextBlockTip.Text = OlibKeyApp.GetLocalizationString(TextInformation.Tips[random.Next(0, TextInformation.Tips.Length)]);
        
        MasterPasswordTextBox.KeyUp += MasterPasswordTextBoxOnKeyUp;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        
        MasterPasswordTextBox.Focus();
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        MasterPasswordTextBox.KeyUp -= MasterPasswordTextBoxOnKeyUp;
        
        base.OnDetachedFromLogicalTree(e);
    }
    
    private void MasterPasswordTextBoxOnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            OlibKeyApp.ViewModel.UnlockStorage();
    }
}