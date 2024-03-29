using System.Text.RegularExpressions;
using OlibKey.Core.StaticMembers;
using OlibKey.Core.Structures;

namespace OlibKey.Core.Helpers;

public static class PasswordChecker
{
    private static readonly Dictionary<string, double> Patterns = new()
    {
        { "1234567890", 0.0 },
        { "[a-z]", 0.1 },
        { "[ёа-я]", 0.2 },
        { "[A-Z]", 0.2 },
        { "[ЁА-Я]", 0.3 },
        { "[!,@#\\$%\\^&\\*?_~=;:'\"<>[]()~`\\\\|/]", 0.4 },
        { @"[¶©]", 0.5 }
    };


    /// <summary>
    /// Checks the complexity of the password
    /// </summary>
    /// <param name="password">Password</param>
    /// <returns>Complexity points</returns>
    public static double GetPasswordComplexity(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return 0;

        double multi0 = 1.0;
        double multi2 = 1.0;

        int score = 0;

        // TODO: Fix source generator
        /*foreach (string badPassword in TextInformation.BadPasswords)
        {
            if (password.Contains(badPassword.ToLower(), StringComparison.CurrentCultureIgnoreCase))
                multi0 = 0.75;
            else if (string.Equals(badPassword, password, StringComparison.CurrentCultureIgnoreCase))
                multi0 = 0.125;
        }*/

        List<char> usedChars = new();

        foreach (char chr in password.Where(c => !usedChars.Contains(c)))
            usedChars.Add(chr);

        double multi1 = GetFrequencyFactor(password.ToLower());
        score += password.Length * 15;

        foreach (KeyValuePair<string,double> pattern in Patterns)
        {
            if (Regex.Match(password, pattern.Key).Length > 0)
                multi2 += pattern.Value;
        }

        return score * multi0 * multi1 * multi2;
    }

    private static double GetFrequencyFactor(string password)
    {
        return GetMap(new HashSet<char>(password.ToCharArray()).Count, 1.0, password.Length, 0.1, 1);
    }

    private static double GetMap(double value, double fromLower, double fromUpper, double toLower, double toUpper)
    {
        return toLower + (value - fromLower) / (fromUpper - fromLower) * (toUpper - toLower);
    }
}