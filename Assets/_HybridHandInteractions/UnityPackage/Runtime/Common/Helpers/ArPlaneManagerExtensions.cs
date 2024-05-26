using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace HybridHandInteractions
{
    /// <summary>
    /// Extension methods for the <see cref="ARPlaneManager"/>
    /// </summary>
    public static class ArPlaneManagerExtensions
    {
        /// <summary>
        /// Gets the first plane of the given type, or null if no plane of that typeis found.
        /// Null is also returned if the current subsystem does not support classification.
        /// </summary>
        /// <param name="planeManager">This AR Planes Manager</param>
        /// <param name="classification">The desired classification type (e.g. floor or wall)</param>
        /// <returns>First found plane or null</returns>
        public static ARPlane GetFirstPlaneOfTypeOrDefault(this ARPlaneManager planeManager, PlaneClassification classification)
        {
            if (planeManager.descriptor != null && planeManager.descriptor.supportsClassification)
            {
                foreach (var trackable in planeManager.trackables)
                    if (trackable.classification == classification)
                        return trackable;
            }

            return null;
        }
    }
}