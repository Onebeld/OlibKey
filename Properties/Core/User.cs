using OlibPasswordManager.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OlibPasswordManager.Properties.Core
{
    public class User
    {
        public static ObservableCollection<ListBoxItemControl> AccountsList { get; set; }
        public static List<User> UsersList { get; set; }

        public static int IndexUser { get; set; }

        public int Type { get; set; }
        public string PasswordName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public string WebSite { get; set; }
        public string TimeCreate { get; set; }
        public string TimeChanged { get; set; }

        public string Image { get; set; }

        public string CardName { get; set; }
        public string DateCard { get; set; }
        public string SecurityCode { get; set; }

        public string PassportNumber { get; set; }
        public string PassportPlaceOfIssue { get; set; }
    }
}
