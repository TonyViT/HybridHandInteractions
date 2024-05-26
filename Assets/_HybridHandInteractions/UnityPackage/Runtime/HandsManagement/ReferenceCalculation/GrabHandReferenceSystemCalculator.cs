using UnityEngine.XR.Hands;

namespace HybridHandInteractions
{
    /// <summary>
    /// Creates the reference system for a hand using the hand joints involved in grabbing an object.
    /// This is useful to estimate the pose of an object that is currently being pinched by the user.
    /// </summary>
    public class GrabHandReferenceSystemCalculator : HandReferenceSystemCalculator
    {
        /// <summary>
        /// The joints that will be used to calculate the reference system
        /// </summary>
        private static readonly XRHandJointID[] k_neededJoints = new XRHandJointID[] { XRHandJointID.Palm, XRHandJointID.IndexProximal, XRHandJointID.ThumbProximal };

        /// <inheritdoc />
        protected override XRHandJointID[] RequiredHandJointsId => k_neededJoints;

        /// <inheritdoc />
        protected override void CalculateReferenceSystem()
        {
            //the reference system will be calculated using the palm, index and thumb joints.
            //Since on the XR Hand plugins, we have a reliable joint of the palm, we just copy the palm pose. 
            //The other two joints are still taken for possible future use (e.g. in some plugins the palm joint is not reliable
            //and should be computed using other joints)
            var palmTransform = LastHandJoints[XRHandJointID.Palm];

            ReferenceSystem.transform.SetPositionAndRotation(palmTransform.position, palmTransform.rotation);

            //we calculate the scale, if needed... but actually with this calculator, the scale will always be the same
            if (AffectScale != ScalingType.None)
                ReferenceSystem.transform.localScale = palmTransform.localScale;
        }
    }

}
