using System;
using System.IO;
using System.Text;

namespace OlibKey.Core
{
	public class Log
	{
		private static readonly object Sync = new object();

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
				string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory! , "Log");
				if(!Directory.Exists(pathToLog)) Directory.CreateDirectory(pathToLog);
				
				if (ex.TargetSite != null)
				{
					lock (Sync)
						File.AppendAllText(Path.Combine(pathToLog,
								$"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log"),
							$"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] | Fatal | [{ex.TargetSite.DeclaringType}.{ex.TargetSite.Name}()] {ex}\r\n",
							Encoding.UTF8);
				}
			}
			catch
			{
				// ignored
			}
		}
		public static void Write(string message, Type type)
		{
			try
			{
				string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Log");
				if(!Directory.Exists(pathToLog)) Directory.CreateDirectory(pathToLog);

				lock (Sync)
					File.AppendAllText(Path.Combine(pathToLog,
							$"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log"),
						$"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] | {type} | {message}\r\n", Encoding.UTF8);

			}
			catch
			{
				// ignored
			}
		}
	}
}
