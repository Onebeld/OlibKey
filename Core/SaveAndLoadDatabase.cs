using System.IO;
using System.Xml.Serialization;
using OlibKey.Structures;

namespace OlibKey.Core
{
	public class SaveAndLoadDatabase
	{
		public static Database LoadFiles(string directoryLocation, string masterPassword) => (Database)new XmlSerializer(typeof(Database)).Deserialize(new StringReader(Encryptor.DecryptString(File.ReadAllText(directoryLocation), masterPassword)));

		public static void SaveFiles(Database database, string directoryLocation, string masterPassword)
		{
			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Database)).Serialize(writer, database);

			File.WriteAllText(directoryLocation, Encryptor.EncryptString(writer.ToString(), masterPassword));
		}
	}
}
