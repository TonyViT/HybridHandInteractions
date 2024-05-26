using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


namespace HybridHandInteractions
{
    /// <summary>
    /// Manager that creates colliders for the AR planes detected by the ARPlaneManager
    /// </summary>
    public class ArPlanesCollidersManager : MonoBehaviour
    {
        /// <summary>
        /// The plane manager, so that we can check the relationship between the placed objects and the planes
        /// (e.g. align objects with the planes normal, if requested by objects)
        /// </summary>
        [SerializeField]
        private ARPlaneManager m_planeManager;

        /// <summary>
        /// The thickness of the colliders created for the planes
        /// </summary>
        [SerializeField]
        private float m_planesCollidersThickness = 0.05f;

        /// <summary>
        /// The layer at which the colliders of the planes will be placed
        /// </summary>
        [SerializeField, Layer]
        protected int m_planesCollidersLayer;

        /// <summary>
        /// If true, the renderers of the planes will be shown for debug purposes
        /// </summary>
        [SerializeField]
        protected bool m_addRenderers = false;

        /// <summary>
        /// Dictionary that maps the trackable id of the planes to the colliders created for them
        /// </summary>
        protected Dictionary<TrackableId, Collider> m_planesColliders = new Dictionary<TrackableId, Collider>();

        /// <summary>
        /// Check if a collider is a plane collider managed by this manager
        /// </summary>
        /// <param name="collider">The collider to check</param>
        /// <returns>True if the collider is managed by this element, false otherwise</returns>
        public bool IsPlaneCollider(Collider collider)
        {
            return m_planesColliders.ContainsValue(collider);
        }
        
        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_planeManager.planesChanged += OnPlanesChanged;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_planeManager.planesChanged -= OnPlanesChanged;
        }

        /// <summary>
        /// Callback called when the detected planes changed
        /// </summary>
        /// <param name="args">The data structure with how the planes changed at this detection</param>
        private void OnPlanesChanged(ARPlanesChangedEventArgs args)
        {
            foreach (ARPlane plane in args.added)
            {
                CreatePlaneCollider(plane);
            }

            foreach (ARPlane plane in args.updated)
            {
                UpdatePlaneCollider(plane);
            }

            foreach (ARPlane plane in args.removed)
            {
                RemovePlaneCollider(plane);
            }
        }

        /// <summary>
        /// Create a collider for a detected plane
        /// </summary>
        /// <param name="plane">The plane to create a collider for</param>
        private void CreatePlaneCollider(ARPlane plane)
        {
            //create a gameobject to hold the collider of this plane and save it in the dictionary
            GameObject planeColliderGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            planeColliderGO.name = "PlaneCollider_" + plane.trackableId;
            planeColliderGO.transform.position = plane.transform.position; //position, rotation, scale will be updated in AdaptColliderToPlane
            planeColliderGO.layer = m_planesCollidersLayer;

            Collider planeCollider = planeColliderGO.GetComponent<BoxCollider>();
            planeCollider.isTrigger = true;
            m_planesColliders.Add(plane.trackableId, planeCollider);

            //add a static rigidbody so that collisions are detected
            AddPlaneRigidbody(planeColliderGO);

            //hide the renderer if requested
            if(!m_addRenderers)
                planeColliderGO.GetComponent<MeshRenderer>().enabled = false;

            //adapt the collider to the plane
            AdaptColliderToPlane(planeCollider, plane);

            Debug.LogFormat("[ArPlanesCollidersManager] Created collider for plane {0}", plane.trackableId);
        }

        /// <summary>
        /// Update the collider related to a specific plane to take into account the changes in the plane
        /// </summary>
        /// <param name="plane">The plane that has been updated</param>
        private void UpdatePlaneCollider(ARPlane plane)
        {
            if (m_planesColliders.TryGetValue(plane.trackableId, out Collider collider))
            {
                AdaptColliderToPlane(collider, plane);

                Debug.LogFormat("[ArPlanesCollidersManager] Updated collider for plane {0}", plane.trackableId);
            }
            //if the collider associated to this plane does not exist, create it from scratch
            else
                CreatePlaneCollider(plane);

        }

        /// <summary>
        /// Remove the collider related to a specific plane
        /// </summary>
        /// <param name="plane">The plane that has been removed</param>
        private void RemovePlaneCollider(ARPlane plane)
        {
            if (m_planesColliders.ContainsKey(plane.trackableId))
            {
                Destroy(m_planesColliders[plane.trackableId].gameObject);
                m_planesColliders.Remove(plane.trackableId);

                Debug.LogFormat("[ArPlanesCollidersManager] Removed collider for plane {0}", plane.trackableId);
            }
        }

        /// <summary>
        /// Adapt a collider to match the size and the pose of the plane it refers to
        /// </summary>
        /// <param name="collider">Collider of interest</param>
        /// <param name="plane">Plane of interest</param>
        private void AdaptColliderToPlane(Collider collider, ARPlane plane)
        {
            collider.transform.position = plane.transform.position;
            collider.transform.rotation = plane.transform.rotation;
            collider.transform.localScale = new Vector3(plane.size.x, m_planesCollidersThickness, plane.size.y);
        }

        /// <summary>
        /// Add a rigidbody to a plane collider
        /// </summary>
        /// <param name="planeGo">the gameobject to add the collider to</param>
        protected virtual void AddPlaneRigidbody(GameObject planeGo)
        {
            Rigidbody planeRigidbody = planeGo.AddComponent<Rigidbody>();
            planeRigidbody.isKinematic = false;
            planeRigidbody.useGravity = false;
            planeRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}