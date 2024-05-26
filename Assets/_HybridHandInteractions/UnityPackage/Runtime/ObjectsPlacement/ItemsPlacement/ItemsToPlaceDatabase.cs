using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Database that contains the items that can be placed in the scene
    /// </summary>
    [CreateAssetMenu(fileName = "ItemsToPlaceDatabase", menuName = "Hybrid Hand Interactions/Items To Place Database")]
    public class ItemsToPlaceDatabase : ScriptableObject
    {
        /// <summary>
        /// The items that can be placed in the scene
        /// </summary>
        [field: SerializeField]
        public ItemsToPlaceData Items { get; private set; }
    }
}