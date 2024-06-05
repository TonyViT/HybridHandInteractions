using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for items that must be spawned (instantiated) in the scene
    /// </summary>
    public interface ISpawnableItem : INamedItem
    {
        /// <summary>
        /// The object that must be instantiated in the scene
        /// </summary>
        GameObject SpawnableItemObject { get; set; }
    }
}