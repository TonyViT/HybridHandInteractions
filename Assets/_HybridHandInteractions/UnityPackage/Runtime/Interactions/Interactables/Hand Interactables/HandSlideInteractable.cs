using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interactable that can react to slide interactions. When it collides with a hand interactor,
    /// it tries to follow the hand, but it has some frozen axes, so it ends sliding on the specified line or plane.
    /// It is useful for instance to simulate a sheet that is moved on the desk by the hand.
    /// </summary>
    public class HandSlideInteractable: HandGrabInteractable
    {
        /// <summary>
        /// Freeze the local X position of the object while following the hand movement
        /// </summary>
        [SerializeField]
        private bool m_freezeLocalX = false;

        /// <summary>
        /// Freeze the local Y position of the object while following the hand movement
        /// </summary>
        [SerializeField]
        private bool m_freezeLocalY = true;

        /// <summary>
        /// Freeze the local Z position of the object while following the hand movement
        /// </summary>
        [SerializeField]
        private bool m_freezeLocalZ = false;

        /// <summary>
        /// Freeze the local X rotation of the object while following the hand movement
        /// </summary>
        [SerializeField]
        private bool m_freezeLocalRotationX = true;

        /// <summary>
        /// Freeze the local Y rotation of the object while following the hand movement
        /// </summary>
        [SerializeField]
        private bool m_freezeLocalRotationY = false;

        /// <summary>
        /// Freeze the local Z rotation of the object while following the hand movement
        /// </summary>
        [SerializeField]
        private bool m_freezeLocalRotationZ = true;

        /// <summary>
        /// Update
        /// </summary>
        protected override void Update()
        {
            //backup the position and rotation of the object before the grab update changes them
            var preGrabPosition = transform.localPosition;
            var preGrabRotation = transform.localRotation;

            //let the base class update the grab
            base.Update();

            //if we are interacting
            if (CurrentState == InteractionState.Interacting)
            {      
                //update the position of the object to match the hand reference system, by taking in count the frozen axes
                transform.localPosition = new Vector3(m_freezeLocalX ? preGrabPosition.x : transform.localPosition.x,
                    m_freezeLocalY ? preGrabPosition.y : transform.localPosition.y,
                    m_freezeLocalZ ? preGrabPosition.z : transform.localPosition.z);
                transform.localRotation = Quaternion.Euler(
                    m_freezeLocalRotationX ? preGrabRotation.x : transform.localRotation.eulerAngles.x,
                    m_freezeLocalRotationY ? preGrabRotation.y : transform.localRotation.eulerAngles.y,
                    m_freezeLocalRotationZ ? preGrabRotation.z : transform.localRotation.eulerAngles.z);
            }
        }
        
    }

}