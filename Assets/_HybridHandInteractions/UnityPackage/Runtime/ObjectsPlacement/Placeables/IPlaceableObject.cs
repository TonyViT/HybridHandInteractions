using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for objects that can be placed in the scene by the user
    /// </summary>
    public interface IPlaceableObject
    {
        /// <summary>
        /// The handle that is used to attach the object to the hand that is placing it
        /// </summary>
        PlacementHandle ObjectHandle { get; }

        /// <summary>
        /// The collider that represents the overall shape of the object
        /// </summary>
        Collider OverallCollider { get; }

        /// <summary>
        /// Callback to be called when the placement of the object is started
        /// </summary>
        void PlacementStarted();

        /// <summary>
        /// Callback to be called when the placement of the object is ended
        /// </summary>
        void PlacementEnded();

        /// <summary>
        /// Callback to be called when the placement of the object is finalized.
        /// This means that the user has finished placing all objects in the scene and it is now
        /// ready to interact with them
        /// </summary>
        /// <remarks>
        /// Use this function to perform any final operations on the object that are needed,
        /// like for instance disable all the logic related to placement itself, because 
        /// it won't be needed anymore
        /// </remarks>
        void PlacementFinalized();
    }

    /// <summary>
    /// Represents how an object that can be placed in the scene is attached to the hand that is
    /// placing it
    /// </summary>
    [Serializable]
    public class PlacementHandle
    {
        /// <summary>
        /// First point of the handle.
        /// If the object is grabbed by a pinch pose of the right hand, this point will be attached to the index finger of the user
        /// </summary>
        [field:SerializeField]
        public Transform PointIndex { get; set; }

        /// <summary>
        /// Second point of the handle.
        /// If the object is grabbed by a pinch pose of the right hand, this point will be attached to the thumb finger of the user
        /// </summary>
        [field:SerializeField]
        public Transform PointThumb { get; set; }
    }
}