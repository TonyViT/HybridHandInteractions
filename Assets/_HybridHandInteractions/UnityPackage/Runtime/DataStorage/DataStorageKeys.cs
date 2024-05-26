using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Storage of preset keys to be used in the data storage of the application
    /// </summary>
    /// <remarks>
    /// This system can be used to implement logic similar to "save slots"
    /// </remarks>
    [CreateAssetMenu(fileName = "DataStorageKeys", menuName = "Hybrid Hand Interactions/Data Storage Keys")]
    public class DataStorageKeys : ScriptableObject
    {
        /// <summary>
        /// Default key to be used in the data storage in case the application needs a common key name to use
        /// </summary>
        public const string DefaultKey = "DefaultStorageSlot";

        /// <summary>
        /// Keys to be used in the application storage
        /// </summary>
        [field: SerializeField]
        public string[] ApplicationStorageKeys;
    }
}