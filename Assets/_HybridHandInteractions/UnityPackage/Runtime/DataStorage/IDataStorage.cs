using System.Collections.Generic;
using System.Threading.Tasks;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for elements that can store data
    /// </summary>
    /// <remarks>
    /// This is the base of a simple system to abstract the storage of data in the application.
    /// In a production environment, probably a more complex system would be needed (e.g. implementing async operations, error handling, etc.)
    /// </remarks>
    public interface IDataStorage
    {
        /// <summary>
        /// Saves the data with the given key
        /// </summary>
        /// <typeparam name="T">Type of data to save</typeparam>
        /// <param name="key">Key at which the data should be saved (the meaning of the key depends on the implementation)</param>
        /// <param name="data">Data to save</param>
        void SaveData<T>(string key, T data);

        /// <summary>
        /// Loads the data with the given key
        /// </summary>
        /// <typeparam name="T">Type of data to load</typeparam>
        /// <param name="key">Key from which the data should be loaded (the meaning of the key depends on the implementation)</param>
        /// <returns>The data that was stored at the provided key</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the key is not found</exception>
        T LoadData<T>(string key);

        /// <summary>
        /// Loads the data with the given key
        /// </summary>
        /// <param name="key">Key of interest (the meaning of the key depends on the implementation)</param>
        /// <returns>True if the key exists, false otherwise</returns>
        bool HasKey(string key);
    }

}