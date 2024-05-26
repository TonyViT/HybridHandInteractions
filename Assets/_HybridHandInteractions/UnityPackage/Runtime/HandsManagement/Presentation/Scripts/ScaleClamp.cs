using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Prevents an object from having a scale that is too big or too small, by clamping it to a certain range
    /// </summary>
    public class ScaleClamp : MonoBehaviour
    {
        /// <summary>
        /// The minimum scale allowed for the object
        /// </summary>
        [SerializeField]
        private float m_minScale = 0.1f;

        /// <summary>
        /// The maximum scale allowed for the object
        /// </summary>
        [SerializeField]
        private float m_maxScale = 10.0f;

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            //clamp the scale of the object
            transform.localScale = new Vector3(
                               Mathf.Clamp(transform.localScale.x, m_minScale, m_maxScale),
                               Mathf.Clamp(transform.localScale.y, m_minScale, m_maxScale),
                               Mathf.Clamp(transform.localScale.z, m_minScale, m_maxScale));
        }
    }
}