using System.Collections.Generic;

namespace OlibKey.Structures
{
    public class Login
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string TimeCreate { get; set; }
        public string TimeChanged { get; set; }

        public string FolderID { get; set; }

        #region Login

        public string Password { get; set; }
        public string WebSite { get; set; }

        #endregion

        #region BankCart

        public string TypeBankCard { get; set; }
        public string DateCard { get; set; }
        public string SecurityCode { get; set; }

        #endregion

        #region PersonalData

        public string PersonalDataNumber { get; set; }
        public string PersonalDataPlaceOfIssue { get; set; }

        #endregion

        #region Reminder

        public bool IsReminderActive { get; set; }

        #endregion

        public string Note { get; set; }

        public List<CustomElement> CustomElements;
    }
    public class CustomFolder
    {
        public string Name { get; set; }
        public string ID { get; set; }
    }
    public class CustomElement
    {
        public string Name { get; set; }
        public int Type { get; set; }

        public string TextElement { get; set; }
        public bool CheckedElement { get; set; }
    }

    public class Database
    {
        public List<Login> Logins;
        public List<CustomFolder> CustomFolders;
    }
}
