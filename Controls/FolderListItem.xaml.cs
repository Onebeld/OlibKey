using OlibKey.AccountStructures;
using System.Windows.Controls;

namespace OlibKey.Controls
{
    /// <summary>
    /// Логика взаимодействия для FolderListItem.xaml
    /// </summary>
    public partial class FolderListItem : UserControl
    {
        public CustomFolder FolderContext => DataContext as CustomFolder;
        public FolderListItem() => InitializeComponent();
    }
}
