using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using OlibKey.Views.Windows;

namespace OlibKey.Core
{
	public static class PasswordGenerator
	{
		public static string RandomPassword()
		{
			const string lower = "abcdefghijklmnopqrstuvwxyz";
			const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string number = "0123456789";
			const string special = @"~!@#$%^&*():;[]{}<>,.?/\|";
			string other = App.Settings.GeneratorTextOther;

			string allowed = "";
			string password = "";
			if (App.Settings.GeneratorAllowLowercase) allowed += lower;
			if (App.Settings.GeneratorAllowUppercase) allowed += upper;
			if (App.Settings.GeneratorAllowNumber) allowed += number;
			if (App.Settings.GeneratorAllowSpecial) allowed += special;
			if (App.Settings.GeneratorAllowUnderscore && password.IndexOfAny("_".ToCharArray()) == -1)
			{
				allowed += "_";
				password += "_";
			}
			if (App.Settings.GeneratorAllowOther) allowed += other;
			int minChars = int.Parse(App.Settings.GenerationCount);
			int numChars = Encryptor.RandomInteger(minChars, minChars);
			while (password.Length < numChars)
				password += allowed.Substring(Encryptor.RandomInteger(0, allowed.Length - 1), 1);
			password = RandomizeString(password);
			return password;
		}
		private static string RandomizeString(string str)
		{
			string result = "";
			while (str.Length > 0)
			{
				int i = Encryptor.RandomInteger(0, str.Length - 1);
				result += str.Substring(i, 1);
				str = str.Remove(i, 1);
			}

			return result;
		}
	}
}
