using OlibKey.Structures;
using OlibKey.ViewModels.Controls;
using OlibKey.Views.Controls;
using System.IO;
using System.Xml.Serialization;

namespace OlibKey.Core
{
	public class SaveAndLoadDatabase
	{
		public static Database LoadFiles(DatabaseViewModel db)
		{
			string s = File.ReadAllText(db.PathDatabase);

			string[] split = s.Split(':');
			int iterations = int.Parse(split[0]);
			int numberOfEncryptionProcedures = int.Parse(split[1]);
			string encryptString = split[2];

			if (split.Length > 3)
			{
				bool useArchiving = bool.Parse(split[3]);

				if (useArchiving)
					s = Compressing.Decompress(Encryptor.DecryptString(encryptString, db, iterations, numberOfEncryptionProcedures));
				

				db.UseCompression = useArchiving;
			}
			else
			{
				s = Encryptor.DecryptString(encryptString, db, iterations, numberOfEncryptionProcedures);
				db.UseCompression = false;
            }

			if (split.Length > 4)
				db.UseTrash = bool.Parse(split[4]);

			return (Database)new XmlSerializer(typeof(Database)).Deserialize(new StringReader(s));
		}

		public static void SaveFiles(DatabaseViewModel db)
		{
			string file = db.Iterations + ":" + db.NumberOfEncryptionProcedures + ":";

			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Database)).Serialize(writer, db.Database);

			string s = Encryptor.EncryptString(db.UseCompression ? Compressing.Compress(writer.ToString()) : writer.ToString(), db);

			file += s + ":" + db.UseCompression + ":" + db.UseTrash;

			File.WriteAllText(db.PathDatabase, file);
		}
	}
}
