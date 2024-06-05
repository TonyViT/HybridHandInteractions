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
        /// Object placer that will place the objects in the scene.
        /// It can be null if we don't want to place the objects in the scene, but just load previously placed objects
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
            if(ObjectPlacer != null)
                ObjectPlacer.ObjectPlacementEnded += OnObjectPlacementEnded;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            if (ObjectPlacer != null)
                ObjectPlacer.ObjectPlacementEnded -= OnObjectPlacementEnded;
        }

        /// <summary>
        /// Callback called when the placement of the current object managed by the object placer ends
        /// </summary>
        /// <param name="object">Object whose placement is just ended</param>
        private void OnObjectPlacementEnded(PlaceableObject @object)
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Object placement end notified by the placer. Adding the element...");

            //add the placed object to the list of placed items
            PlacedItems.Add(m_currentlyPlacingItem);
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
            var item = ItemsToPlace.GetItem(itemName);

            if (item == null)
                throw new ArgumentException($"Item {itemName} has not been found in the list of placeable objects");

            m_currentlyPlacingItem = new PlaceableItem() { ItemName = itemName, PlaceableItemObject = GameObject.Instantiate(item.PlaceableItemObject) };

            //assign the new item to the object placer, so that it can start the placement
            m_objectPlacer.StartPlacement(m_currentlyPlacingItem.PlaceableItemObject);
        }

        /// <inheritdoc />
        public void RemoveLastItem()
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Removing last item");

            //remove the last placed item and also destroy its gameobject
            if(PlacedItems.Count > 0)
            {
                Destroy(PlacedItems[PlacedItems.Count - 1].PlaceableItemObject.gameObject);
                PlacedItems.RemoveAt(PlacedItems.Count - 1);
            }
        }

        /// <inheritdoc />
        public void FinalizePlacedItems()
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Finalizing placed items");

            //finalize all the placed items
            foreach (var item in PlacedItems)
            {
                //get the entry in the database for the item
                var dbItem = ItemsToPlace.GetItem(item.ItemName);

                //if the database entry specify that when the object is finalized, a new overriding element should be spawned at its place
                if (dbItem is ISpawnableItem spawnableItem && spawnableItem.SpawnableItemObject != null)
                {
                    //instantiate the override object in the same pose of the currently placed object
                    var spawnedObject = GameObject.Instantiate(spawnableItem.SpawnableItemObject, item.PlaceableItemObject.transform.position, item.PlaceableItemObject.transform.rotation);
                    spawnedObject.transform.localScale = item.PlaceableItemObject.transform.localScale;

                    //finalize the placement, just in case the object needs to do some finalization (mostly useless, but you never know)
                    item.PlaceableItemObject.PlacementFinalized();

                    //destroy the currently placed object
                    Destroy(item.PlaceableItemObject.gameObject);
                }
                //else, if the placed object is the final object
                else
                {
                    //finalize the placement, so the element is ready for interactions
                    item.PlaceableItemObject.PlacementFinalized();
                }
            }

            //clear the list of placed items
            PlacedItems.Clear();
        }

        /// <inheritdoc />
        public string Serialize(PlaneClassification referencePlaneClassification)
        {
            Debug.Log($"[HandBasedObjectsPlacementManager] Serializing placed items with {referencePlaneClassification} reference plane");

            //convert the placed items into DTOs that can be serialized
            List<PlacedItemDto> placedItemDtos = new List<PlacedItemDto>();

            foreach (var item in PlacedItems)
            {
                placedItemDtos.Add(new PlacedItemDto(item, referencePlaneClassification, m_planesManager));
            }

            //serialize
            return JsonConvert.SerializeObject(placedItemDtos);
        }

        /// <inheritdoc />
        public void Deserialize(string serializedPlacedObjects)
        {
            Debug.Log("[HandBasedObjectsPlacementManager] Deserializing placed items");

            //clear the current list of placed items
            Clear();

            //deserialize the list of placed items
            List<PlacedItemDto> placedItemDtos = JsonConvert.DeserializeObject<List<PlacedItemDto>>(serializedPlacedObjects);

            //recreate the placed items
            foreach (var placedItemDto in placedItemDtos)
            {
                //find the item in the database
                var item = ItemsToPlace.GetItem(placedItemDto.ItemName);

                if (item == null)
                    throw new ArgumentException($"Item {placedItemDto.ItemName} has not been found in the list of placeable objects");

                //create the item
                var placeableItem = new PlaceableItem() { ItemName = placedItemDto.ItemName, PlaceableItemObject = GameObject.Instantiate(item.PlaceableItemObject) };

                //fill the item with the data from the DTO
                placedItemDto.FillItem(placeableItem, m_planesManager);

                //add the item to the list of placed items
                PlacedItems.Add(placeableItem);
            }
        }
    }
}