namespace OlibKey.Core.UnitTests;

public class PasswordTests
{
    [Test]
    public void Check_For_Duplicate_Bad_Passwords()
    {
        string[] badPasswords = Structures.TextInformation.BadPasswords;
        
        Array.Sort(badPasswords);
        
        List<string> duplicatedPasswords = new();

        for (int index = 0; index < badPasswords.Length; index++)
        {
            if (index != 0 && badPasswords[index] == badPasswords[index - 1])
                duplicatedPasswords.Add(badPasswords[index]);
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