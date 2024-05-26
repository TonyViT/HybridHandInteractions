using UnityEngine;
using UnityEngine.XR.Hands;

namespace HybridHandInteractions
{
    /// <summary>
    /// Creates the reference system for a hand using a single joint (e.g. the wrist)
    /// </summary>
    public class SingleJointReferenceSystemCalculator : HandReferenceSystemCalculator
    {
        /// <summary>
        /// The joint id that will be used to calculate the reference system
        /// </summary>
        [SerializeField]
        private XRHandJointID m_jointId;

        /// <inheritdoc />
        protected override XRHandJointID[] RequiredHandJointsId => m_neededJoints;

        /// <summary>
        /// The joints that will be used to calculate the reference system
        /// </summary>
        private XRHandJointID[] m_neededJoints = new XRHandJointID[1] { XRHandJointID.Invalid };

        /// <inheritdoc />
        protected override void Awake()
        {
            m_neededJoints[0] = m_jointId;
            base.Awake();
        }

        /// <inheritdoc />
        protected override void CalculateReferenceSystem()
        {
            //we have only one joint: the reference system just mirrors its pose
            var jointTransform = LastHandJoints[m_jointId];

            ReferenceSystem.transform.SetPositionAndRotation(jointTransform.position, jointTransform.rotation);

            if(AffectScale != ScalingType.None)
                ReferenceSystem.transform.localScale = jointTransform.localScale;
        }
    }

}