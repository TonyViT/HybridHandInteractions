using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Place virtual objects in the scene using the hand pinch gesture
    /// </summary>
    public class HandPinchObjectPlacer : HandObjectPlacer
    {
        /// <summary>
        /// The pinch hand reference system calculator, to calculate the reference system of the hand
        /// </summary>
        /// <remarks>
        /// Notice that this placer will affect the scale calculation of the pinch hand reference system calculator,
        /// so beware if other components are using it.
        /// This may be fixed in the future by generating a separate reference system calculator for the object placement
        /// or by using a child object of the reference system calculator with its own scale for the object placement.
        /// But for now, this is the simplest solution.
        /// </remarks>
        [SerializeField]
        private PinchHandReferenceSystemCalculator m_pinchHandReferenceSystemCalculator;

        /// <inheritdoc />
        public override void StartPlacement(PlaceableObject placeableObject)
        {
            base.StartPlacement(placeableObject);

            //change the reference system calculator to consider the scale of the pinching:
            //we set as scale 1 of the reference system the current distance of the handle points, this way when we will parent
            //the object to the hands, it will be scaled down from its current size to the size that the handle will exactly fit
            //the distance between the index and thumb fingertips.
            //Thanks to this trick, we can simply parent the object to the hand reference system and it will be scaled correctly
            //no matter its initial size
            m_pinchHandReferenceSystemCalculator.AffectScale = ScalingType.Linear;
            m_pinchHandReferenceSystemCalculator.FingerDistanceAtUnitScale = Vector3.Distance(placeableObject.ObjectHandle.PointIndex.position, placeableObject.ObjectHandle.PointThumb.position);

            //we need to set the placeable object as child of the reference system of the hand pinch,
            //so that it moves with the hand and scales with the hand.
            //But we can not do that naively, we need to do that so that the handle of the object
            //coincides with the fingertips of the index and thumb fingers, and the orientation of the object
            //coincides with the orientation of the pinch gesture. This is what the SetPlacementPose method does.
            CurrentObject = placeableObject;
            placeableObject.transform.SetParent(m_pinchHandReferenceSystemCalculator.ReferenceSystem.transform, false);
            SetPlacementPose();
        }

        /// <inheritdoc />
        public override void EndPlacement()
        {           
            if(CurrentObject != null)
                //detach the object, and leave it in the curent pose and scale
                CurrentObject.transform.SetParent(null, true);

            //we do it after, because it sets CurrentObject to null
            base.EndPlacement();
        }

        /// <inheritdoc />
        protected override void SetPlacementPose()
        {
            //fit the object so that the handle coincides with the fingertips of the index and thumb fingers

            //the object should place itself so that the reference system (the new parent of the object) has its origin coninciding with the handle center,
            //so we have to counter-translate it (remembering that the object is scaled down by the pinch hand reference system calculator).
            //Also, to make things more complext, the rotation which will come next will put the object upside down on the left hand, so we have to make the opposite
            //translation on the Y axis for the left hand.
            Vector3 scaling = new Vector3(CurrentObject.transform.localScale.x,
                m_pinchHandReferenceSystemCalculator.handedness == UnityEngine.XR.Hands.Handedness.Right ? CurrentObject.transform.localScale.y : -CurrentObject.transform.localScale.y,
                CurrentObject.transform.localScale.z);
            CurrentObject.transform.localPosition = Vector3.Scale(scaling, 
                -CurrentObject.transform.InverseTransformPoint(0.5f * (CurrentObject.ObjectHandle.PointIndex.position + CurrentObject.ObjectHandle.PointThumb.position)));

            //the reference system cares about the rotation, but we have to remember that Y axis are inverted between the two hands
            CurrentObject.transform.localRotation = m_pinchHandReferenceSystemCalculator.handedness == UnityEngine.XR.Hands.Handedness.Right ? Quaternion.identity : Quaternion.AngleAxis(180, Vector3.forward); 
        }



    }
}