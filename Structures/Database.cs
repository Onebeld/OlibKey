using System.Collections.Generic;

namespace OlibKey.Structures
{
	public class Login
	{
		public int Type { get; set; }
		public string Name { get; set; } = "";
		public string Username { get; set; }
		public string Email { get; set; }
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

		public string Number { get; set; }
		public string PlaceOfIssue { get; set; }
		public string SocialSecurityNumber { get; set; }
		public string TIN { get; set; }
		public string Telephone { get; set; }
		public string Company { get; set; }
		public string Postcode { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string City { get; set; }
		public string Address { get; set; }

		#endregion

		#region Reminder

		public bool IsReminderActive { get; set; }

		#endregion

		public string Note { get; set; }

		public List<CustomField> CustomFields;
	}
	public class Folder
	{
		public string Name { get; set; }
		public string ID { get; set; }
	}
	public class CustomField
	{
		public string Name { get; set; }
		public int Type { get; set; }

		public string TextElement { get; set; }
		public bool CheckedElement { get; set; }
	}

	public class Database
	{
		public List<Login> Logins;
		public List<Folder> Folders;
	}
}
