using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Base class for gameobject that can be placed in the scene by the user.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class PlaceableObject : MonoBehaviour, IPlaceableObject
    {
        /// <inheritdoc />
        [field: SerializeField]
        public PlacementHandle ObjectHandle { get; protected set; }

        /// <inheritdoc />
        public Collider OverallCollider { get; protected set; }

        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            OverallCollider = GetComponent<Collider>();
        }

        /// <inheritdoc />
        public virtual void PlacementStarted()
        {
        }

        /// <inheritdoc />
        public virtual void PlacementEnded()
        {
        }

        /// <inheritdoc />
        public virtual void PlacementFinalized()
        {

        }

    }
}