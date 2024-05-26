using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Base class for elements that can store data and that live in the Unity scene
    /// </summary>
    public abstract class DataStorage: MonoBehaviour,
        IDataStorage
    {
        /// <inheritdoc />
        public abstract void SaveData<T>(string key, T data);

        /// <inheritdoc />
        public abstract T LoadData<T>(string key);

        /// <inheritdoc />
        public abstract bool HasKey(string key);
    }

}