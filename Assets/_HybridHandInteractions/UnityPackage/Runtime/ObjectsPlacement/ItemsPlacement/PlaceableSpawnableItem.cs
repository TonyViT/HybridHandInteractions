using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Class for items that can be placed in the scene, but that can then be spawned as a different item when they are finalized
    /// </summary>
    /// <remarks>
    /// This class lets the developer specify an item to spawn in place of a placeable object when the placeable object should be finalized.
    /// This is useful because maybe the placeable object is just a simple placeholder that is used for the placement, but it is not the object we want the user
    /// to interact with after the finalization. For instance, an application may use a generic Cylinder placement for both a bottle and a jar, but then when the 
    /// interactive part of the application starts, a bottle and jar prefab should be instantiated.
    /// When it's time to launch the interactive parts of the application, the placer will: either finalize the placement object
    /// if no object to spawn was found (in this case it is assumed that the object to place after the finalization can be interactive), or will destroy the
    /// placement object and instantiate the object to spawn (and in this case it is assumed that the object to place was just meant for the initial placement, 
    /// while the object to spawn is interactive)
    /// </remarks>
    [Serializable]
    public class PlaceableSpawnableItem: PlaceableItem, ISpawnableItem
    {
        /// <summary>
        /// The object that can be spawned in the scene when the placement is finalized.
        /// It can be null, if the user can simply interact with finalized version of the placement object
        /// </summary>
        [field: SerializeField]
        public GameObject SpawnableItemObject { get; set; }
    }
}