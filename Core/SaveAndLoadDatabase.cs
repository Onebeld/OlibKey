using OlibKey.Structures;
using OlibKey.Views.Controls;
using System.IO;
using System.Xml.Serialization;

namespace OlibKey.Core
{
	public class SaveAndLoadDatabase
	{
		public static Database LoadFiles(DatabaseControl db) => (Database)new XmlSerializer(typeof(Database)).Deserialize(new StringReader(Encryptor.DecryptString(File.ReadAllText(db.ViewModel.PathDatabase), db)));

		public static void SaveFiles(DatabaseControl db)
		{
			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Database)).Serialize(writer, db.ViewModel.Database);

			File.WriteAllText(db.ViewModel.PathDatabase, Encryptor.EncryptString(writer.ToString(), db));
		}
	}
}
