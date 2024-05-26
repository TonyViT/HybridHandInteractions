using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Sends events about a collider to the objects that are interested in them
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BroadcastColliderEvents : MonoBehaviour
    {
        /// <summary>
        /// Event that is triggered when a collision starts.
        /// The first parameter is this collider, the second is the collider of the other object
        /// </summary>
        public Action<Collider, Collider> CollisionEntered;

        /// <summary>
        /// Event that is triggered when a collision ends.
        /// The first parameter is this collider, the second is the collider of the other object
        /// </summary>
        public Action<Collider, Collider> CollisionExited;

        /// <summary>
        /// Event that is triggered when a collision is ongoing.
        /// The first parameter is this collider, the second is the collider of the other object
        /// </summary>
        public Action<Collider, Collider> CollisionStaying;

        /// <summary>
        /// Event that is triggered when a trigger collision starts.
        /// The first parameter is this collider, the second is the collider of the other object
        /// </summary>
        public Action<Collider, Collider> TriggerEntered;

        /// <summary>
        /// Event that is triggered when a trigger collision ends.
        /// The first parameter is this collider, the second is the collider of the other object
        /// </summary>
        public Action<Collider, Collider> TriggerExited;

        /// <summary>
        /// Event that is triggered when a trigger collision is ongoing.
        /// The first parameter is this collider, the second is the collider of the other object
        /// </summary>
        public Action<Collider, Collider> TriggerStaying;

        /// <summary>
        /// The collider attached to this object
        /// </summary>
        private Collider m_collider;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }

        /// <summary>
        /// Called when a collision is started
        /// </summary>
        private void OnCollisionEnter(Collision other)
        {
            CollisionEntered?.Invoke(m_collider, other.collider);
        }

        /// <summary>
        /// Called when a collision is exited
        /// </summary>
        private void OnCollisionExit(Collision other)
        {
            CollisionExited?.Invoke(m_collider, other.collider);
        }

        /// <summary>
        /// Called when a collision is ongoing
        /// </summary>
        private void OnCollisionStay(Collision other)
        {
            CollisionStaying?.Invoke(m_collider, other.collider);
        }

        /// <summary>
        /// Called when a trigger collision is started
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(m_collider, other);
        }

        /// <summary>
        /// Called when a trigger collision is exited
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke(m_collider, other);
        }

        /// <summary>
        /// Called when a trigger collision is ongoing
        /// </summary>
        private void OnTriggerStay(Collider other)
        {
            TriggerStaying?.Invoke(m_collider, other);
        }
    }
}