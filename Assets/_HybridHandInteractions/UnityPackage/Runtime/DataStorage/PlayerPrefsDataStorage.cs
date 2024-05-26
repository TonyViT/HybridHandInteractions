using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Data storage that uses Unity's PlayerPrefs to store data
    /// </summary>
    /// <remarks>
    /// Every data is converted to JSON and store in PlayerPrefs, no matter the type
    /// </remarks>
    public class PlayerPrefsDataStorage : DataStorage
    {
        /// <inheritdoc />
        public override bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <inheritdoc />
        public override T LoadData<T>(string key)
        {
            if (!HasKey(key))
                throw new KeyNotFoundException($"Key {key} not found in PlayerPrefs");

            string json = PlayerPrefs.GetString(key);

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <inheritdoc />
        public override void SaveData<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, json);
        }
    }

}
