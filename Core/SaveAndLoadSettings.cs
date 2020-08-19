using OlibKey.Structures;
using System;
using System.IO;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace OlibKey.Core
{
	public static class SaveAndLoadSettings
	{
		public static Settings LoadSettings()
		{
			try
			{
				return (Settings)new XmlSerializer(typeof(Settings)).Deserialize(new StringReader(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.xml")));
			}
			catch
			{
				return new Settings();
			}
		}
		[SecurityPermission(SecurityAction.Demand)]
		public static void SaveSettings()
		{
			using StringWriter writer = new StringWriter();
			new XmlSerializer(typeof(Settings)).Serialize(writer, Program.Settings);

			File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.xml", writer.ToString());
		}
	}
}
