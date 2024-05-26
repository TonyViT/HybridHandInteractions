using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for the hand reference system calculators, that is elements that can infer a reference system
    /// from a set of hand joints, so that objects can be placed in the hand reference system
    /// </summary>
    public interface IHandReferenceSystemCalculator
    {
        /// <summary>
        /// Gets the reference system that should for the hand
        /// </summary>
        public Transform ReferenceSystem { get; }

        /// <summary>
        /// Gets the last hand joints that were used to calculate the reference system
        /// </summary>
        public Dictionary<XRHandJointID, Transform> LastHandJoints { get; }

        /// <summary>
        /// Gets whether the reference system should affect the scale of the objects placed in it.
        /// If it is None, the scale of the objects should be kept constant, otherwise the calculator
        /// will try to infer the scale from the hand joints (e.g. by using the distance between the index and thumb fingertips)
        /// </summary>
        public ScalingType AffectScale { get; }
    }

    /// <summary>
    /// Defines which type of scaling can be applied to a reference system
    /// </summary>
    public enum ScalingType
    {
        None,
        Linear,
        Quadratic
    }
}