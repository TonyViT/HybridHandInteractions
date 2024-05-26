
using System;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for an object placer: a component that allows to place virtual objects in the scene
    /// </summary>
    public interface IObjectPlacer
    {
        /// <summary>
        /// The object that is currently being placed. Null if no object is being placed
        /// </summary>
        PlaceableObject CurrentObject { get; }

        /// <summary>
        /// The options for the object placer
        /// </summary>
        ObjectPlacerOptions Options { get; }

        /// <summary>
        /// Start the placement of a placeable object.
        /// If an object was already being placed, the previous object will be dropped
        /// </summary>
        /// <param name="placeableObject">Object to place</param>
        void StartPlacement(PlaceableObject placeableObject);

        /// <summary>
        /// End the placement of the current object, if any
        /// </summary>
        void EndPlacement();

        /// <summary>
        /// Event called when the placement of an object has been started
        /// </summary>
        Action<PlaceableObject> ObjectPlacementStarted { get; set; }

        /// <summary>
        /// Event called when the placement of an object has been completed
        /// </summary>
        Action<PlaceableObject> ObjectPlacementEnded { get; set; }
    }

    /// <summary>
    /// Options for an object placer: define how the placement should happen
    /// </summary>
    /// <remarks>
    /// These features are currently experimental and may not work as expected in all situations.
    /// </remarks>
    [Serializable]
    public class ObjectPlacerOptions
    {
        /// <summary>
        /// True if the object should be aligned with the plane of the surface where it is placed,
        /// meaning that its Y axis will be aligned with the normal of the plane.
        /// The plane used for the alignment is the one that is colliding with the object 
        /// </summary>
        public bool AlignWithPlane;

        /// <summary>
        /// True if the object should be placed in a way that it does not penetrate the plane it is
        /// eventually colliding with. False if the object should be placed at the position of the
        /// hand, even if it penetrates a plane
        /// </summary>
        public bool AvoidCompenetration;
    }
}