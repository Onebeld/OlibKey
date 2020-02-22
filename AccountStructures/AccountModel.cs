namespace OlibKey.AccountStructures
{
    public class AccountModel
    {
        public int TypeAccount { get; set; }
        public string AccountName { get; set; }
        public string Username { get; set; }
        public string TimeCreate { get; set; }
        public string TimeChanged { get; set; }

        #region Login
        public string Password { get; set; }
        public string WebSite { get; set; }
        public string IconWebSite { get; set; }
        #endregion

        #region BankCart
        public string TypeBankCart { get; set; }
        public string DateCard { get; set; }
        public string SecurityCode { get; set; }
        #endregion

        #region Pasport
        public string PassportNumber { get; set; }
        public string PassportPlaceOfIssue { get; set; }
        #endregion
        public string Note { get; set; }
    }
}
