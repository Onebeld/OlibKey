using System;
using System.IO;
using System.Text;

namespace OlibKey.Core
{
	public class Log
	{
		private static readonly object sync = new object();

		public enum Type
		{
			Info,
			Warning,
			Error
		}

		public static void WriteFatal(Exception ex)
		{
			try
			{
				string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
				if(!Directory.Exists(pathToLog)) Directory.CreateDirectory(pathToLog);
				string fileName = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log", AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
				string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] | Fatal | [{1}.{2}()] {4}\r\n", DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message, ex);
				lock (sync)
				{
					File.AppendAllText(fileName, fullText, Encoding.UTF8);
				}

			}
			catch {}
		}
		public static void Write(string message, Type type)
		{
			try
			{
				string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
				if(!Directory.Exists(pathToLog)) Directory.CreateDirectory(pathToLog);
				string fileName = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log", AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
				string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] | {1} | {2}\r\n", DateTime.Now, type ,message);
				lock (sync)
				{
					File.AppendAllText(fileName, fullText, Encoding.UTF8);
				}

			}
			catch {}
		}
	}
}
