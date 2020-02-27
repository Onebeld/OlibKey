using OlibKey.AccountStructures;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OlibKey.Controls
{
    /// <summary>
    /// Interaction logic for AccountListItem.xaml
    /// </summary>
    public partial class AccountListItem : UserControl
    {
        public Action<AccountModel> ShowContentCallback { get; set; }
        public Action<AccountModel> EditContentCallback { get; set; }
        public Action FocusCallback { get; set; }
        private AccountModel AccountContext { get => DataContext as AccountModel; }
        public AccountListItem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.Clear();
                switch (int.Parse(((FrameworkElement)e.Source).Uid))
                {
                    case 1: Clipboard.SetText(AccountContext.Username); break;
                    case 2: Clipboard.SetText(AccountContext.Password); break;
                    case 3: EditContentCallback?.Invoke(AccountContext); break;
                }
            }
            catch{}
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowContentCallback?.Invoke(AccountContext);
            FocusCallback?.Invoke();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (AccountContext.TypeAccount)
            {
                case 1:
                    imageIcon.Source = (ImageSource)FindResource("debit_cardDrawingImage");
                    break;
                case 2:
                    imageIcon.Source = (ImageSource)FindResource("contactDrawingImage");
                    break;
                default:
                    if (string.IsNullOrEmpty(AccountContext.WebSite))
                        imageIcon.Source = (ImageSource) FindResource("globeDrawingImage");
                    break;
            }
        }
    }
}
