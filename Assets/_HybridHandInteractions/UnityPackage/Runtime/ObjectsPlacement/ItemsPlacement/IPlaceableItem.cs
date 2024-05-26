namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for items that can be placed in the scene
    /// </summary>
    public interface IPlaceableItem
    {
        /// <summary>
        /// The object that can be placed in the scene
        /// </summary>
        PlaceableObject ItemObject { get; set; }

        /// <summary>
        /// The name that identifies the item
        /// </summary>
        string ItemName { get; set; }
    }
}