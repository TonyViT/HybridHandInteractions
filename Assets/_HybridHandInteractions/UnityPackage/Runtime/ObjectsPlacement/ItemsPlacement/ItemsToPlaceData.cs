using System;

namespace HybridHandInteractions
{
    /// <summary>
    /// Stores the data necessary to know which items can be placed in the scene
    /// </summary>
    [Serializable]
    public class ItemsToPlaceData
    {
        /// <summary>
        /// The items that can be placed in the scene
        /// </summary>
        public PlaceableItem[] ItemsToPlace;

        /// <summary>
        /// Gets the item with the given name
        /// </summary>
        /// <param name="itemName">Name of interest</param>
        /// <returns>The item with the desired name, or null if no item is found</returns>
        public PlaceableItem GetItem(string itemName)
        {
            foreach (var item in ItemsToPlace)
            {
                if (item.ItemName == itemName)
                {
                    return item;
                }
            }

            return null;
        }
    }
}