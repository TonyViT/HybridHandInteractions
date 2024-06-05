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
            //the following line should be uncommented, because the search should be done only if the subsystem supports plane classification.
            //Actually I've verified this check is not reliable. Quest supports classification, but supportsClassification returns false.
            //so I've commented the check and the search is done anyway (which is not optimized).
            //TODO: check when the bug is fixed and uncomment the line below
            //
            //if (planeManager.descriptor != null && planeManager.descriptor.supportsClassification)
            {
                foreach (var trackable in planeManager.trackables)
                    if (trackable.classification == classification)
                        return trackable;
            }

            return null;
        }
    }
}