using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

namespace HybridHandInteractions
{
    /// <summary>
    /// Implements an interactor that interacts via the bare hand and whose detection colliders
    /// are placed on the hand joints defined by the user
    /// </summary>
    public class HandInteractorCustomJointColliders : HandInteractor
    {
        /// <summary>
        /// Gets the hand joints that are required for the reference system calculation
        /// </summary>
        [SerializeField]
        private XRHandJointID[] m_requiredHandJointsId;

        /// <summary>
        /// The transforms of the gameobjects that have been created to represent the joints
        /// </summary>
        private Transform[] m_createdJointsTransforms;

        /// <summary>
        /// Awake
        /// </summary>
        protected void Awake()
        {
            //create a transform for every required joint id, so that the colliders can be added to them
            m_createdJointsTransforms = new Transform[m_requiredHandJointsId.Length];

            for(int i = 0; i < m_requiredHandJointsId.Length; i++)
            {
                var jointTransform = new GameObject(m_requiredHandJointsId[i].ToString()).transform;
                jointTransform.SetParent(transform, false);
                m_createdJointsTransforms[i] = jointTransform;
            }
        }

        /// <summary>
        /// On Enable
        /// </summary>
        protected void OnEnable()
        {
            //notice that we still use the reference system calculator to get updates about the hand joints
            HandReferenceSystemCalculatorBehaviour.jointsUpdated.AddListener(HandJointsUpdated);
        }

        /// <summary>
        /// On Disable
        /// </summary>
        protected void OnDisable()
        {
            HandReferenceSystemCalculatorBehaviour.jointsUpdated.RemoveListener(HandJointsUpdated);
        }

        /// <summary>
        /// Callback for when the hand joints are updated
        /// </summary>
        /// <param name="handJointsEventArgs">Data about the updated joints</param>
        private void HandJointsUpdated(XRHandJointsUpdatedEventArgs handJointsEventArgs)
        {
            //update all transforms with the new tracking data of the joints
            for(int i = 0; i < m_requiredHandJointsId.Length; i++)
            {
                var jointData = handJointsEventArgs.hand.GetJoint(m_requiredHandJointsId[i]);

                if (jointData.TryGetPose(out Pose jointPose))
                {
                    m_createdJointsTransforms[i].position = jointPose.position;
                    m_createdJointsTransforms[i].rotation = jointPose.rotation;
                }
            }
        }

        /// <inheritdoc />
        protected override Transform[] GetInteractionJointsTransforms()
        {
            return m_createdJointsTransforms;
        }
    }

}