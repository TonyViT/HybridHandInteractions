using UnityEngine;
using UnityEngine.XR.Hands;

namespace HybridHandInteractions
{
    /// <summary>
    /// Creates the reference system for a hand using the fingers involved in a pinch pose (index and thumb).
    /// This is useful to estimate the pose of an object that is currently being pinched by the user.
    /// </summary>
    public class PinchHandReferenceSystemCalculator : HandReferenceSystemCalculator
    {
        /// <summary>
        /// The joints that will be used to calculate the reference system
        /// </summary>
        private static readonly XRHandJointID[] k_neededJoints = new XRHandJointID[] { XRHandJointID.IndexTip, XRHandJointID.IndexProximal, XRHandJointID.ThumbTip };

        /// <summary>
        /// If true, the scale of the reference system will be affected by the distance between the index and thumb tips,
        /// otherwise the scale will always be the identity vector
        /// </summary>
        [SerializeField]
        private ScalingType m_affectScale = ScalingType.Quadratic;

        /// <summary>
        /// The distance between the index and thumb tips at the unit scale
        /// </summary>
        [field: SerializeField]
        public float FingerDistanceAtUnitScale { get; set; } = 0.06f; //6cm for the unit scale

        /// <inheritdoc />
        protected override XRHandJointID[] RequiredHandJointsId => k_neededJoints;

        /// <inheritdoc />
        override protected void Awake()
        {
            base.Awake();

            AffectScale = m_affectScale;
        }

        /// <inheritdoc />
        protected override void CalculateReferenceSystem()
        {
            var indexTipTransform = LastHandJoints[XRHandJointID.IndexTip];
            var indexProximalTransform = LastHandJoints[XRHandJointID.IndexProximal];
            var thumbTipTransform = LastHandJoints[XRHandJointID.ThumbTip];

            //we identify the reference system as follows:
            //- the origin is the middle point between the index and thumb tips
            //- the x axis is the direction from the thumb tip to the index tip
            //- the y axis is the normal of the plane that incorporate the index finger and the thumb tip (more or less when you 
            //  keep an object between the thumb and the index finger, it is oriented that way)
            //- the z axis is the cross product between the x and y axes
            Plane plane = new Plane(indexTipTransform.position, indexProximalTransform.position, thumbTipTransform.position);
            Vector3 yAxis = plane.normal;
            Vector3 xAxis = indexTipTransform.position - thumbTipTransform.position;
            Vector3 zAxis = Vector3.Cross(xAxis, yAxis);

            ReferenceSystem.transform.SetPositionAndRotation((indexTipTransform.position + thumbTipTransform.position) / 2, Quaternion.LookRotation(zAxis, yAxis));

            //if we want to affect the scale, we can do it by scaling the reference system by the distance between the index and thumb tips.            
            if (AffectScale == ScalingType.Quadratic)
                ReferenceSystem.transform.localScale = (xAxis.sqrMagnitude / (FingerDistanceAtUnitScale * FingerDistanceAtUnitScale)) * Vector3.one;
            else if (AffectScale == ScalingType.Linear)
                ReferenceSystem.transform.localScale = (xAxis.magnitude / FingerDistanceAtUnitScale) * Vector3.one;
        }
    }

}
