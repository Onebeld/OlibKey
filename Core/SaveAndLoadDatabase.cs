using OlibKey.Structures;
using OlibKey.Views.Controls;
using System.IO;
using System.Xml.Serialization;

namespace OlibKey.Core
{
	public class SaveAndLoadDatabase
	{
		public static Database LoadFiles(DatabaseControl db)
		{
			string s = File.ReadAllText(db.ViewModel.PathDatabase);

			string[] split = s.Split(':');
			int iterations = int.Parse(split[0]);
			int numberOfEncryptionProcedures = int.Parse(split[1]);
			string encryptString = split[2];

			if (split != null && split.Length > 3)
			{
				bool useArchiving = bool.Parse(split[3]);

				if (useArchiving)
					s = Archiving.Decompress(Encryptor.DecryptString(encryptString, db, iterations, numberOfEncryptionProcedures));
				

				db.ViewModel.UseCompression = useArchiving;
			}
			else
			{
				s = Encryptor.DecryptString(encryptString, db, iterations, numberOfEncryptionProcedures);
				db.ViewModel.UseCompression = false;
			}


			return (Database)new XmlSerializer(typeof(Database)).Deserialize(new StringReader(s));
		}

		public static void SaveFiles(DatabaseControl db)
		{
			string file = db.ViewModel.Iterations + ":" + db.ViewModel.NumberOfEncryptionProcedures + ":";

			string s;

			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Database)).Serialize(writer, db.ViewModel.Database);

			if (db.ViewModel.UseCompression)
				s = Encryptor.EncryptString(Archiving.Compress(writer.ToString()), db);
			else s = Encryptor.EncryptString(writer.ToString(), db);

			file += s + ":" + db.ViewModel.UseCompression;

			File.WriteAllText(db.ViewModel.PathDatabase, file);
		}
	}
}
