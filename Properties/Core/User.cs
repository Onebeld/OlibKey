using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace OlibPasswordManager.Properties.Core
{
    public class User
    {
        public static List<User> UsersList { get; set; }
        public static int IndexUser { get; set; }

        public string PasswordName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public string WebSite { get; set; }
        public string TimeCreate { get; set; }
        public string TimeChanged { get; set; }
        public string Image { get; set; }
    }
}
