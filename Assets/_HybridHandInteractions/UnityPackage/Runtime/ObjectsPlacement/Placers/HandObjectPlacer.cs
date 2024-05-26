using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Base class for the elements that place virtual objects in the scene using the hand
    /// </summary>
    public abstract class HandObjectPlacer : MonoBehaviour,
        IObjectPlacer
    {
        /// <summary>
        /// How much the current held object can be left inside a colliding plane even if
        /// it was requested to avoid compenetration
        /// </summary>
        private const float k_compenetrationAllowedDistance = 0.01f;

        /// <summary>
        /// Reference to the object in the scene responsible for creating colliders for the AR planes
        /// </summary>
        [SerializeField]        
        private ArPlanesCollidersManager m_arPlanesCollidersManager;

        /// <inheritdoc />
        [field: SerializeField]
        public ObjectPlacerOptions Options { get; protected set; }

        /// <inheritdoc />
        public PlaceableObject CurrentObject { get; protected set; }

        /// <inheritdoc />      
        public Action<PlaceableObject> ObjectPlacementStarted { get; set; }

        /// <inheritdoc />
        public Action<PlaceableObject> ObjectPlacementEnded { get; set; }

        /// <summary>
        /// The plane that the held object is currently colliding with.
        /// It is null if there is no held object of if the object is not colliding with any plane.
        /// </summary>
        protected Collider CurrentlyCollidingPlane { get; set; }

        /// <summary>
        /// On Destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            if(CurrentObject != null)
                EndPlacement();
        }

        /// <summary>
        /// Update
        /// </summary>
        protected virtual void Update()
        {
            //if the current object is colliding with a plane
            if (CurrentObject != null && CurrentlyCollidingPlane != null)
            {
                //if it was asked to align the object with colliding planes, align its up vector
                if (Options.AlignWithPlane)
                {
                    CurrentObject.transform.up = CurrentlyCollidingPlane.transform.up;
                }

                //if it was asked to avoid compenetration, move the object up so that it does not penetrate the plane
                if (Options.AvoidCompenetration)
                {
                    Physics.ComputePenetration(CurrentObject.OverallCollider, CurrentObject.transform.position, CurrentObject.transform.rotation,
                                               CurrentlyCollidingPlane, CurrentlyCollidingPlane.transform.position, CurrentlyCollidingPlane.transform.rotation,
                                               out var direction, out var distance);

                    if (distance > 0)
                        //move the object up so that it does not penetrate the plane. Notice that we subtract the compenetration allowed distance
                        //to make sure that actually it still compenetrate a bit, so the collision still holds true.
                        //If we do not this, at next frame the object won't collide anymore with the plane and it will be moved back to the hand
                        //position... but then when in the hand, it may still be colliding with the plane, so it will trigger again this mechanism
                        //and so on: basically at one frame it collides, at the next it does not, at the next it collides again, and so on.
                        //To prevent this, we leave a bit of compenetration allowed, so that the object is still colliding with the plane and is more stable
                        CurrentObject.transform.position += direction * (distance - k_compenetrationAllowedDistance);

                }
            }
        }

        /// <inheritdoc />
        public virtual void StartPlacement(PlaceableObject placeableObject)
        {
            Debug.LogFormat("[HandObjectPlacer] Request to start the placement of {0}", placeableObject.name);

            //drop the previous object, if any
            if(CurrentObject != null)
                EndPlacement();

            //register to the collision event of this object
            var broadcastColliderEvents = placeableObject.gameObject.AddComponent<BroadcastColliderEvents>();
            broadcastColliderEvents.TriggerEntered += CurrentObjectCollisionEntered;
            broadcastColliderEvents.TriggerExited += CurrentObjectCollisionExited;
            broadcastColliderEvents.CollisionEntered += CurrentObjectCollisionEntered;
            broadcastColliderEvents.CollisionExited += CurrentObjectCollisionExited;

            //notify the object that the placement has started
            placeableObject.PlacementStarted();

            //invoke the event
            ObjectPlacementStarted?.Invoke(placeableObject);
        }

        /// <inheritdoc />
        public virtual void EndPlacement()
        {
            if(CurrentObject == null)
                return;

            Debug.LogFormat("[HandObjectPlacer] Request to end the placement of {0}", CurrentObject.name);

            //notify the current object that the placement has ended
            CurrentObject.PlacementEnded();

            //unregister from the collision event of this object
            var broadcastColliderEvents = CurrentObject.GetComponent<BroadcastColliderEvents>();
            broadcastColliderEvents.TriggerEntered -= CurrentObjectCollisionEntered;
            broadcastColliderEvents.TriggerExited -= CurrentObjectCollisionExited;
            broadcastColliderEvents.CollisionEntered -= CurrentObjectCollisionEntered;
            broadcastColliderEvents.CollisionExited -= CurrentObjectCollisionExited;
            CurrentlyCollidingPlane = null;
            Destroy(broadcastColliderEvents); //we add it so we can remove it

            //invoke the event
            ObjectPlacementEnded?.Invoke(CurrentObject);

            CurrentObject = null;            
        }

        /// <summary>
        /// Sets or resets the placement pose of the current object so that it fits the hand.
        /// This is also useful to call after the collision with a plane changed the default
        /// object orientation
        /// </summary>
        protected abstract void SetPlacementPose();

        /// <summary>
        /// Called when the current object is colliding with another collider
        /// </summary>
        /// <param name="sourceCollider">The source collider, which is the one of the current object</param>
        /// <param name="otherCollider">The collider it collided with</param>
        private void CurrentObjectCollisionEntered(Collider sourceCollider, Collider otherCollider)
        {
            //if we already have a colliding plane, stick with the existing one, otherwise go on
            if(CurrentlyCollidingPlane == null)
            {
                //if the other collider is a plane detected in the room managed by the AR planes colliders manager
                if(m_arPlanesCollidersManager.IsPlaneCollider(otherCollider))
                {
                    //then save this plane as a colliding plane
                    CurrentlyCollidingPlane = otherCollider;

                    Debug.LogFormat("[HandObjectPlacer] The current object is now colliding with plane {0}", CurrentlyCollidingPlane.name);
                }
            }
        }

        /// <summary>
        /// Called when the current object has stopped colliding with another collider
        /// </summary>
        /// <param name="sourceCollider">The source collider, which is the one of the current object</param>
        /// <param name="otherCollider">The collider it stopped colliding with</param>
        private void CurrentObjectCollisionExited(Collider sourceCollider, Collider otherCollider)
        {
            //if the collider that was exited is the one we were colliding with
            if(CurrentlyCollidingPlane == otherCollider)
            {
                //then we are not colliding with any plane anymore
                CurrentlyCollidingPlane = null;

                //restore original pose of the object in hand
                SetPlacementPose();

                Debug.LogFormat("[HandObjectPlacer] The current object is not colliding with any plane anymore");
            }
        }

    }
}