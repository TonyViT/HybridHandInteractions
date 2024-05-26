using System.Collections.Generic;
using UnityEngine;
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
        /// Object placer that will place the objects in the scene
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
        /// Serialize the placed items in the scene.
        /// </summary>
        /// <param name="referencePlaneClassification">Reference plane (if any) for the positioning of the items. If specified, all the pose of the elements
        /// to serialize will be specified as relative to this plane. For more info check the documentation of <see cref="PlacedItemDto"/></param>
        /// <returns>String representation of the list of placed items</returns>
        string Serialize(PlaneClassification referencePlaneClassification);
    }
}