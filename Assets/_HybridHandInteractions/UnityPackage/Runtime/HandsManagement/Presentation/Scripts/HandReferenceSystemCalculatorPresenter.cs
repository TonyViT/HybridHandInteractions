using System.Collections.Generic;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter that shows the most important elements of a <see cref="HandReferenceSystemCalculator"/>
    /// </summary>
    public class HandReferenceSystemCalculatorPresenter : MonoBehaviour
    {
        /// <summary>
        /// The hand reference system calculator to show
        /// </summary>
        [SerializeField]
        private HandReferenceSystemCalculator m_handReferenceSystemCalculator;

        /// <summary>
        /// The prefab to use for the visualization of the hand joints
        /// </summary>
        [SerializeField]
        private GameObject m_jointVisualizationPrefab;

        /// <summary>
        /// The prefab to use for the visualization of the reference system
        /// </summary>
        [SerializeField]
        private GameObject m_referenceSystemVisualizationPrefab;

        /// <summary>
        /// The joint visualization gameobjects
        /// </summary>
        private List<GameObject> m_jointVisualizations = new List<GameObject>();

        /// <summary>
        /// The reference system visualization gameobject
        /// </summary>
        private GameObject m_referenceSystemVisualization;

        /// <summary>
        /// True if the hand tracking is working, so the presenter should show the visualization
        /// </summary>
        private bool m_isTracking = false;

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_handReferenceSystemCalculator.trackingAcquired.AddListener(OnTrackingAcquired);
            m_handReferenceSystemCalculator.trackingLost.AddListener(OnTrackingLost);
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_handReferenceSystemCalculator.trackingAcquired.RemoveListener(OnTrackingAcquired);
            m_handReferenceSystemCalculator.trackingLost.RemoveListener(OnTrackingLost);
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            //if the hand is being tracked update the pose of the reference system and the joints
            if(m_isTracking)
            {
                m_referenceSystemVisualization.transform.SetPositionAndRotation(m_handReferenceSystemCalculator.ReferenceSystem.position, m_handReferenceSystemCalculator.ReferenceSystem.rotation);
                m_referenceSystemVisualization.transform.localScale = m_handReferenceSystemCalculator.ReferenceSystem.localScale;

                int i = 0;
                foreach(var handJointValue in m_handReferenceSystemCalculator.LastHandJoints.Values)
                {
                    m_jointVisualizations[i].transform.SetPositionAndRotation(handJointValue.position, handJointValue.rotation);
                    i++;
                }
            }
        }

        /// <summary>
        /// Callback called when the tracking of the hand is acquired
        /// </summary>
        private void OnTrackingAcquired()
        {
            m_isTracking = true;

            //create the visualization for the reference system and the joints
            m_referenceSystemVisualization = Instantiate(m_referenceSystemVisualizationPrefab, transform, false);
            m_jointVisualizations.Clear();

            for (int i = 0; i < m_handReferenceSystemCalculator.LastHandJoints.Count; i++)
            {
                GameObject jointVisualization = Instantiate(m_jointVisualizationPrefab, transform, false);
                m_jointVisualizations.Add(jointVisualization);
            }
        }

        /// <summary>
        /// Callback called when the tracking of the hand is lost
        /// </summary>
        private void OnTrackingLost()
        {
            m_isTracking = false;

            //destroy the visualization for the reference system and the joints
            Destroy(m_referenceSystemVisualization);

            foreach (GameObject jointVisualization in m_jointVisualizations)
            {
                Destroy(jointVisualization);
            }

            m_jointVisualizations.Clear();
        }
    }
}