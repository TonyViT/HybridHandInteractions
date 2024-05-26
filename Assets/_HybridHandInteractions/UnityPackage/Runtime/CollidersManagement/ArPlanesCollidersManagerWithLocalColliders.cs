using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


namespace HybridHandInteractions
{
    /// <summary>
    /// Manager that creates colliders for the AR planes detected by the ARPlaneManager,
    /// and also accepts external colliders to be managed
    /// </summary>
    /// <remarks>
    /// This class is useful to test in editor the interaction of objects with the planes,
    /// because at the time of writing this script, the ARFoundation package does not work in editor
    /// taking the data of the Scene Setup via Oculus Link
    /// </remarks>
    public class ArPlanesCollidersManagerWithLocalColliders : ArPlanesCollidersManager
    {
        /// <summary>
        /// The plane manager, so that we can check the relationship between the placed objects and the planes
        /// (e.g. align objects with the planes normal, if requested by objects)
        /// </summary>
        [SerializeField]
        private Collider[] m_localColliders;

        /// <summary>
        /// If true, the local colliders will be added only in the editor and not also in build
        /// </summary>
        [SerializeField]
        private bool m_editorOnly;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            if (m_editorOnly && !Application.isEditor)
                Debug.Log("[ArPlanesCollidersManagerWithLocalColliders] Editor only mode, skipping the addition of local colliders");
            else
                foreach (var localCollider in m_localColliders)
                {
                    AddLocalCollider(localCollider);
                }
        }

        /// <summary>
        /// Add a local collider to the manager
        /// </summary>
        /// <param name="localCollider">The collider to add</param>
        private void AddLocalCollider(Collider localCollider)
        {
            if (localCollider != null)
            {
                //move the collider to the layer of the planes colliders
                localCollider.gameObject.layer = m_planesCollidersLayer;

                //make the collider trigger
                localCollider.isTrigger = true;

                //enable or disable the renders as requested
                if (localCollider.TryGetComponent<Renderer>(out Renderer colliderRenderer))
                    colliderRenderer.enabled = m_addRenderers;

                //add a static rigidbody if not present
                if (!localCollider.TryGetComponent<Rigidbody>(out Rigidbody colliderRigidbody))
                {
                    AddPlaneRigidbody(localCollider.gameObject);
                }

                //add the collider to the dictionary
                m_planesColliders.Add(new TrackableId((ulong)localCollider.GetInstanceID(), (ulong)localCollider.GetInstanceID()), localCollider);

                Debug.LogFormat("[ArPlanesCollidersManagerWithLocalColliders] Added local collider {0}", localCollider.name);
            }
            
        }
    }

}