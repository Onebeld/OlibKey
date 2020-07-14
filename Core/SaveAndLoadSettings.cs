using System;
using System.IO;
using System.Xml.Serialization;
using OlibKey.Structures;

namespace OlibKey.Core
{
	public static class SaveAndLoadSettings
	{
		public static Settings LoadSettings()
		{
			try
			{
				return (Settings)new XmlSerializer(typeof(Settings)).Deserialize(new StringReader(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json")));
			}
			catch
			{
				return new Settings();
			}
		}

		public static void SaveSettings()
		{
			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Settings)).Serialize(writer, App.Settings);

			File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json", writer.ToString());
		}
	}
}
