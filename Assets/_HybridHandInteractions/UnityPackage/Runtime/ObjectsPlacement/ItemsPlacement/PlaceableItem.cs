using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Class for items that can be placed in the scene
    /// </summary>
    [Serializable]
    public class PlaceableItem: IPlaceableItem
    {
        /// <summary>
        /// The name that identifies the item
        /// </summary>
        [field: SerializeField]
        public string ItemName { get; set; }

        /// <summary>
        /// The object that can be placed in the scene
        /// </summary>
        [field: SerializeField]
        public PlaceableObject PlaceableItemObject { get; set; }
    }
}