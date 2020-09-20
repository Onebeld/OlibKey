using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Structures;
using System;

namespace OlibKey.Views.Controls
{
    public class ImportedFileListItem : UserControl
    {
        public string ID;
        public Action<string> DeleteFileCallback;

        public Separator SLine;
        public Button BDelete;

        public ImportedFileListItem()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            SLine = this.FindControl<Separator>("sLine");
            BDelete = this.FindControl<Button>("bDelete");
        }

        private void DeleteFile(object sender, RoutedEventArgs e) => DeleteFileCallback?.Invoke(ID);
        private async void ExportFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialFileName = ((ImportedFile)DataContext).Name
            };
            string res = await saveFileDialog.ShowAsync(App.MainWindow);
			if (res != null)
            {
                Core.FileInteractions.ExportFile(((ImportedFile)DataContext).Data, res);
                App.MainWindow.MessageStatusBar((string)Application.Current.FindResource("Not10"));
            }
        }
    }
}
