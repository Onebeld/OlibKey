using Newtonsoft.Json;
using OlibKey.AccountStructures;
using OlibKey.ModelViews;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace OlibKey.Core
{
    public class SaveAndLoadAccount
    {
        public static Database LoadFiles(string directoryLocation, string masterPassword)
        {
            var s = File.ReadAllText(directoryLocation);

            XmlSerializer x = new XmlSerializer(typeof(Database));

            using TextReader reader = new StringReader(Encryptor.DecryptString(s, masterPassword));
            Database database = (Database)x.Deserialize(reader);

            return database;
        }

        public static void SaveFiles(Database database, string directoryLocation)
        {
            var writer = new StringWriter();
            new XmlSerializer(typeof(Database)).Serialize(writer, database);

            string s = writer.ToString();

            File.WriteAllText(directoryLocation, Encryptor.EncryptString(s, MainViewModel.MasterPassword));
        }
    }
}
