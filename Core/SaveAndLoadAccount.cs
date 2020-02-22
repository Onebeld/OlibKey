using OlibKey.AccountStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace OlibKey.Core
{
    public class SaveAndLoadAccount
    {
        public static List<AccountModel> LoadFiles(string directoryLocation)
        {

            List<AccountModel> accounts = new List<AccountModel>();

            for (int i = 0; i < accname.Count; i++)
            {
                AccountModel am = new AccountModel()
                {
                    AccountName = accname[i],
                    Email = emailss[i],
                    Username = usernam[i],
                    Password = passwrd[i],
                    DateOfBirth = dofbrth[i],
                    SecurityInfo = scrtyin[i],
                    ExtraInfo1 = extinf1[i],
                    ExtraInfo2 = extinf2[i],
                    ExtraInfo3 = extinf3[i],
                    ExtraInfo4 = extinf4[i],
                    ExtraInfo5 = extinf5[i]
                };
                accounts.Add(am);
            }
            return accounts;
        }
    }
}
