using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace HybridHandInteractions
{
    /// <summary>
    /// Manages the placement of objects in the scene using the hands
    /// </summary>
    public class HandBasedObjectsPlacementManager : MonoBehaviour,
        IObjectsPlacementManager
    {
        /// <summary>
        /// Database of spawnable items
        /// </summary>
        [SerializeField]
        private ItemsToPlaceDatabase m_itemsToPlaceDb;

        /// <summary>
        /// Object placer that will place the objects in the scene
        /// </summary>
        [SerializeField]
        private HandObjectPlacer m_objectPlacer;

        /// <summary>
        /// The planes manager 
        /// </summary>
        /// <remarks>
        /// It is used to find the relative position of the placed object wrt the planes in the scene.
        /// (E.g. to find the relative position of the object wrt the floor)
        /// </remarks>
        [SerializeField]
        private ARPlaneManager m_planesManager;

        /// <inheritdoc />
        public IObjectPlacer ObjectPlacer => m_objectPlacer;

        /// <inheritdoc />
        public ItemsToPlaceData ItemsToPlace => m_itemsToPlaceDb.Items;

        /// <inheritdoc />
        public List<IPlaceableItem> PlacedItems { get; private set; } = new List<IPlaceableItem>();

        /// <summary>
        /// The item that is currently being placed
        /// </summary>
        private IPlaceableItem m_currentlyPlacingItem;

        private void OnEnable()
        {
            ObjectPlacer.ObjectPlacementEnded += OnObjectPlacementEnded;
        }

        private void OnObjectPlacementEnded(PlaceableObject @object)
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Object placement end notified by the placer. Adding the element...");

            //add the placed object to the list of placed items
            PlacedItems.Add(m_currentlyPlacingItem);
        }

        private void OnDisable()
        {
            ObjectPlacer.ObjectPlacementEnded -= OnObjectPlacementEnded;
        }

        /// <inheritdoc />
        public void Clear()
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Clearing placed items list");

            //destroy all the placed items and their gameobjects
            while(PlacedItems.Count > 0)            
                RemoveLastItem();            
        }

        /// <inheritdoc />
        public void PlaceNewItem(string itemName)
        {
            Debug.Log($"[HandBasedObjectsPlacementManager] Placing new item: {itemName}");

            //spawn the object corresponding to the item name
            var item = m_itemsToPlaceDb.Items.GetItem(itemName);

            if (item == null)
                throw new ArgumentException($"Item {itemName} has not been found in the list of placeable objects");

            m_currentlyPlacingItem = new PlaceableItem() { ItemName = itemName, ItemObject = GameObject.Instantiate(item.ItemObject) };

            //assign the new item to the object placer, so that it can start the placement
            m_objectPlacer.StartPlacement(m_currentlyPlacingItem.ItemObject);
        }

        /// <inheritdoc />
        public void RemoveLastItem()
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Removing last item");

            //remove the last placed item and also destroy its gameobject
            if(PlacedItems.Count > 0)
            {
                Destroy(PlacedItems[PlacedItems.Count - 1].ItemObject.gameObject);
                PlacedItems.RemoveAt(PlacedItems.Count - 1);
            }
        }

        /// <inheritdoc />
        public string Serialize(PlaneClassification referencePlaneClassification)
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Serializing placed items");

            //convert the placed items into DTOs that can be serialized
            List<PlacedItemDto> placedItemDtos = new List<PlacedItemDto>();

            foreach (var item in PlacedItems)
            {
                placedItemDtos.Add(new PlacedItemDto(item, referencePlaneClassification, m_planesManager));
            }

            //serialize
            return JsonConvert.SerializeObject(placedItemDtos);
        }      
    }
}