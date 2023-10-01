using OlibKey.Assets;

namespace OlibKey.Core.UnitTests;

public class PasswordTests
{
    [Test]
    public void Check_File_For_Duplicate_Bad_Passwords()
    {
        string[] badPasswords = Resources.BadPasswords.Split("\r\n");
        
        Array.Sort(badPasswords);
        
        List<string> duplicatedPasswords = new();

        for (int i = 0; i < badPasswords.Length; i++)
        {
            if (i != 0 && badPasswords[i] == badPasswords[i - 1])
                duplicatedPasswords.Add(badPasswords[i]);
        }
        
        if (duplicatedPasswords.Count == 0)
            Assert.Pass();
        else
        {
            TestContext.WriteLine("Duplicated passwords:");
            
            foreach (string duplicatedPassword in duplicatedPasswords)
                TestContext.WriteLine(duplicatedPassword);
            
            Assert.Fail("Bad password file contains duplicates");
        }
    }
}