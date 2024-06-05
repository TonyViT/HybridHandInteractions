using System;
using System.Data;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace HybridHandInteractions
{
    /// <summary>
    /// Data transfer object for a placed item
    /// </summary>
    [Serializable]
    public class PlacedItemDto
    {
        /// <summary>
        /// Name of the item to be spawned
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Position of the item in the scene
        /// </summary>
        /// <remarks>
        /// We don't use Unity's Vector3 because it is not serializable
        /// </remarks>
        public float[] Position { get; set; }

        /// <summary>
        /// Rotation quaternion of the item in the scene
        /// </summary>
        /// <remarks>
        /// We don't use Unity's Quaternion because it is not serializable
        /// </remarks>
        public float[] Rotation { get; set; }

        /// <summary>
        /// Scale of the item in the scene
        /// </summary>
        /// <remarks>
        /// We don't use Unity's Vector3 because it is not serializable
        /// </remarks>
        public float[] Scale { get; set; }

        /// <summary>
        /// Eventual reference plane for the positioning of the item.
        /// If this is different than none, then all the above properties are referred as relative to this plane.
        /// If it is none, then all the above properties are referred as absolute.
        /// </summary>
        /// <remarks>
        /// When the headset is turned on, its reference system may be different from the last run, so saving absolute
        /// positions may not be a good idea. In this case, we can save the position relative to the reference plane,
        /// like for instance, the floor: since the floor of the room is always the same, the pose of the objects will be
        /// saved reliably.
        /// </remarks>
        public PlaneClassification ReferencePlanceClassification { get; set; }    

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlacedItemDto()
        {
            Position = new float[3];
            Rotation = new float[4];
            Scale = new float[3];
        }

        /// <summary>
        /// Constructor that takes an item and transforms it into a DTO
        /// </summary>
        /// <param name="item">The item to convert</param>
        /// <param name="referencePlaneClassification">The desired reference for the data (e.g. the floor)</param>
        /// <param name="planesManager">The planes manager holding the list of existing planes</param>
        public PlacedItemDto(IPlaceableItem item, PlaneClassification referencePlaneClassification, ARPlaneManager planesManager)
        {
            //get the reference plane
            ARPlane referencePlane = null;

            if (referencePlaneClassification != PlaneClassification.None)
                referencePlane = planesManager.GetFirstPlaneOfTypeOrDefault(referencePlaneClassification);

            //compute the position, rotation and scale of the item either as absolute or relative to the reference plane
            Vector3 position = referencePlane == null ? item.PlaceableItemObject.transform.position : referencePlane.transform.InverseTransformPoint(item.PlaceableItemObject.transform.position);
            Quaternion rotation = referencePlane == null ? item.PlaceableItemObject.transform.rotation : Quaternion.Inverse(referencePlane.transform.rotation) * item.PlaceableItemObject.transform.rotation;
            Vector3 scale = item.PlaceableItemObject.transform.localScale;

            //set the properties of the DTO
            ItemName = item.ItemName;
            Position = new float[] { position.x, position.y, position.z };
            Rotation = new float[] { rotation.x, rotation.y, rotation.z, rotation.w };
            Scale = new float[] { scale.x, scale.y, scale.z };
            ReferencePlanceClassification = referencePlane != null ? referencePlane.classification : PlaneClassification.None; //if we did not find the plane,                                                                                                                             //the reference is absolute, even if the user asked to have a reference plane
        }

        /// <summary>
        /// Fills the provided item with the data from this DTO.
        /// </summary>
        /// <param name="item">The item to be filled with data.</param>
        /// <param name="planesManager">The ARPlaneManager that manages the list of existing planes.</param>
        /// <exception cref="InvalidOperationException">Thrown when the reference plane of the specified type is not found in the scene.</exception>
        public void FillItem(IPlaceableItem item, ARPlaneManager planesManager)
        {
            //get the reference plane
            ARPlane referencePlane = null;

            if (ReferencePlanceClassification != PlaneClassification.None)
            {
                referencePlane = planesManager.GetFirstPlaneOfTypeOrDefault(ReferencePlanceClassification);

                //check if the reference plane is found
                if (referencePlane == null)
                    throw new InvalidOperationException ($"The reference plane of type {ReferencePlanceClassification} was not found in the scene, but was needed for the object set up");
            }

            //fill the item with the data
            item.ItemName = ItemName;
            item.PlaceableItemObject.transform.position = referencePlane == null ? new Vector3(Position[0], Position[1], Position[2]) : referencePlane.transform.TransformPoint(new Vector3(Position[0], Position[1], Position[2]));
            item.PlaceableItemObject.transform.rotation = referencePlane == null ? new Quaternion(Rotation[0], Rotation[1], Rotation[2], Rotation[3]) : referencePlane.transform.rotation * new Quaternion(Rotation[0], Rotation[1], Rotation[2], Rotation[3]);
            item.PlaceableItemObject.transform.localScale = new Vector3(Scale[0], Scale[1], Scale[2]);
        }
    }
}