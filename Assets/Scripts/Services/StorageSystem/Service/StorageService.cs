using Services.EncryptionSystem.Extension;
using Services.StorageSystem.Interface;
using UnityEngine;

namespace Services.StorageSystem.Service
{
    public class StorageService : IStorageService
    {
        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }

        public void SetData(string key, string value)
        {
            PlayerPrefs.SetString(EncryptExtension.Encrypt(key), EncryptExtension.Encrypt(value));
            PlayerPrefs.Save();
        }

        public string GetData(string key, string defaultValue = "-1")
        {
            key = EncryptExtension.Encrypt(key);
            return PlayerPrefs.HasKey(key) ? EncryptExtension.Decrypt(PlayerPrefs.GetString(key)) : defaultValue;
        }
    }
}