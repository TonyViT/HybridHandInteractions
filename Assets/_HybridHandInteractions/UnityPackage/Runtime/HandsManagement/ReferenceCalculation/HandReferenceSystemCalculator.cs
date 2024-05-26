using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

namespace HybridHandInteractions
{
    /// <summary>
    /// Base class for the hand reference system calculators, that is elements that can infer a reference system
    /// from a set of hand joints, so that objects can be placed in the hand reference system
    /// </summary>
    public abstract class HandReferenceSystemCalculator: XRHandTrackingEvents, 
        IHandReferenceSystemCalculator
    {
        /// <inheritdoc />
        public Transform ReferenceSystem { get; protected set; }

        /// <inheritdoc />
        public Dictionary<XRHandJointID, Transform> LastHandJoints { get; protected set; }

        /// <inheritdoc />
        public ScalingType AffectScale { get; set; }

        /// <summary>
        /// Gets the hand joints that are required for the reference system calculation
        /// </summary>
        /// <remarks>
        /// Every subclass will have its own set of required hand joints, that will be used to calculate the reference system
        /// </remarks>
        protected abstract XRHandJointID[] RequiredHandJointsId { get; }

        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            //create a transform for every required joint id, so that other classes can get the joint transforms
            //instead of working with joint ids, positions, rotations, etc...
            LastHandJoints = new Dictionary<XRHandJointID, Transform>();

            foreach (var jointId in RequiredHandJointsId)
            {
                var jointTransform = new GameObject(jointId.ToString()).transform;
                jointTransform.SetParent(transform, false);
                LastHandJoints.Add(jointId, jointTransform);
            }

            //create the transform of the reference system
            ReferenceSystem = new GameObject("ReferenceSystem").transform;
            ReferenceSystem.SetParent(transform, false);
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            jointsUpdated.AddListener(HandJointsUpdated);
        }

        /// <summary>
        /// On Disable
        /// </summary>
        protected override void OnDisable()
        {
            jointsUpdated.RemoveListener(HandJointsUpdated);
            base.OnDisable();
        }

        /// <summary>
        /// Callback for when the hand joints are updated
        /// </summary>
        /// <param name="handJointsEventArgs">Data about the updated joints</param>
        /// <exception cref="NotImplementedException"></exception>
        private void HandJointsUpdated(XRHandJointsUpdatedEventArgs handJointsEventArgs)
        {
            //update all transforms with the new tracking data of the joints
            foreach (var joint in LastHandJoints)
            {
                var jointData = handJointsEventArgs.hand.GetJoint(joint.Key);

                if(jointData.TryGetPose(out Pose jointPose))
                {
                    joint.Value.position = jointPose.position;
                    joint.Value.rotation = jointPose.rotation;
                }
            }

            //calculate the reference system
            CalculateReferenceSystem();
        }

        /// <summary>
        /// Calculates the reference system used the latest information on the hand joints
        /// </summary>
        protected abstract void CalculateReferenceSystem();
    }
}