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
        /// The items that can be placed in the scene (and can be eventually spawned in a different way)
        /// </summary>
        [SerializeField]
        private PlaceableSpawnableItem[] m_itemsToPlace;

        /// <summary>
        /// The items that can be placed in the scene
        /// </summary>
        public ItemsToPlaceData Items => new ItemsToPlaceData() { ItemsToPlace = m_itemsToPlace };
    }
}