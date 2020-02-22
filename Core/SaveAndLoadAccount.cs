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
            List<AccountModel> accounts = new List<AccountModel>();
            var s = File.ReadAllText(directoryLocation);

            accounts = JsonConvert.DeserializeObject<List<AccountModel>>(Encryptor.DecryptString(
                Encryptor.DecryptString(
                    Encryptor.DecryptString(
                        Encryptor.DecryptString(Encryptor.DecryptString(s, masterPassword),
                            masterPassword), masterPassword), masterPassword),
                masterPassword));

            return accounts;
        }
        public static void SaveFiles(List<AccountModel> accounts, string directoryLocation, bool b)
        {
            try
            {
                string json = JsonConvert.SerializeObject(accounts);

                File.WriteAllText(directoryLocation,
                        Encryptor.EncryptString(
                            Encryptor.EncryptString(
                                Encryptor.EncryptString(
                                    Encryptor.EncryptString(Encryptor.EncryptString(json, MainViewModel.MasterPassword),
                                        MainViewModel.MasterPassword), MainViewModel.MasterPassword), MainViewModel.MasterPassword),
                            MainViewModel.MasterPassword));
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
