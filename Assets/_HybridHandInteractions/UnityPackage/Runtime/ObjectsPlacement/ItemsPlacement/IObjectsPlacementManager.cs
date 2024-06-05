using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for managers that place objects in the scene
    /// </summary>
    public interface IObjectsPlacementManager
    {
        /// <summary>
        /// The items that can be placed in the scene
        /// </summary>
        ItemsToPlaceData ItemsToPlace { get; }

        /// <summary>
        /// The items that have been placed in the scene
        /// </summary>
        /// <remarks>
        /// We use the interface <see cref="IPlaceableItem"/> slightly in an improper way here, because
        /// it should be used only for objects that should still be spawned in the scene. But this way we
        /// can store the placed items together with their unique name (that we need to serialize them) and we avoid
        /// doing a lot of "find" operations during the serialization process.
        /// </remarks>
        List<IPlaceableItem> PlacedItems { get; }

        /// <summary>
        /// Object placer that will place the objects in the scene.
        /// It can be null if we don't want to place the objects in the scene, but just load previously placed objects
        /// </summary>
        IObjectPlacer ObjectPlacer { get; }

        /// <summary>
        /// Clears the list of placed items
        /// </summary>
        void Clear();

        /// <summary>
        /// Starts the placement of a new item in the scene
        /// </summary>
        /// <param name="itemName">Name of the item to place</param>
        void PlaceNewItem(string itemName);

        /// <summary>
        /// Removes the last item that has been added to the sccene
        /// </summary>
        void RemoveLastItem();

        /// <summary>
        /// Performs the finalization of the placed items, so the user can proceed to interact with them
        /// </summary>
        /// <remarks>
        /// All the placed elements will be permanently placed in the scene and the list of in-progress-placement items will be cleared.
        /// </remarks>
        void FinalizePlacedItems();

        /// <summary>
        /// Serialize the placed items in the scene.
        /// </summary>
        /// <param name="referencePlaneClassification">Reference plane (if any) for the positioning of the items. If specified, all the pose of the elements
        /// to serialize will be specified as relative to this plane. For more info check the documentation of <see cref="PlacedItemDto"/></param>
        /// <returns>String representation of the list of placed items</returns>
        string Serialize(PlaneClassification referencePlaneClassification);

        /// <summary>
        /// Deserialize the list of placed items from a previous session
        /// </summary>
        /// <remarks>
        /// This function deletes the list of currently placed items and substitutes it with the deserialized one
        /// </remarks>
        /// <param name="serializedPlacedObjects">String produced by the <see cref="Serialize"/> function with the serialization of a list of elements</param>
        void Deserialize(string serializedPlacedObjects);
    }
}