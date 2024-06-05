namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for items that can be placed in the scene
    /// </summary>
    public interface IPlaceableItem: INamedItem
    {
        /// <summary>
        /// The object that can be placed in the scene
        /// </summary>
        PlaceableObject PlaceableItemObject { get; set; }
    }
}