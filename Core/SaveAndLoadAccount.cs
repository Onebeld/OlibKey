using Newtonsoft.Json;
using OlibKey.AccountStructures;
using OlibKey.ModelViews;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace OlibKey.Core
{
    public class SaveAndLoadAccount
    {
        public static List<AccountModel> LoadFiles(string directoryLocation, string masterPassword)
        {
            var s = File.ReadAllText(directoryLocation);

            List<AccountModel> accounts = JsonConvert.DeserializeObject<List<AccountModel>>(Encryptor.DecryptString(s, masterPassword));

            return accounts;
        }
        public static void SaveFiles(List<AccountModel> accounts, string directoryLocation, bool b)
        {
            try
            {
                string json = JsonConvert.SerializeObject(accounts);

                File.WriteAllText(directoryLocation, Encryptor.EncryptString(json, MainViewModel.MasterPassword));
            }
            catch
            {
                if (b)
                    MessageBox.Show("Не удалось сохранить.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
