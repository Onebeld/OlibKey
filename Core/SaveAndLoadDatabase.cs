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

			if (split.Length > 3)
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

			if (split.Length > 4)
				db.ViewModel.UseTrash = bool.Parse(split[4]);

			return (Database)new XmlSerializer(typeof(Database)).Deserialize(new StringReader(s));
		}

		public static void SaveFiles(DatabaseControl db)
		{
			string file = db.ViewModel.Iterations + ":" + db.ViewModel.NumberOfEncryptionProcedures + ":";

			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Database)).Serialize(writer, db.ViewModel.Database);

			string s = Encryptor.EncryptString(db.ViewModel.UseCompression ? Archiving.Compress(writer.ToString()) : writer.ToString(), db);

			file += s + ":" + db.ViewModel.UseCompression + ":" + db.ViewModel.UseTrash;

			File.WriteAllText(db.ViewModel.PathDatabase, file);
		}
	}
}
