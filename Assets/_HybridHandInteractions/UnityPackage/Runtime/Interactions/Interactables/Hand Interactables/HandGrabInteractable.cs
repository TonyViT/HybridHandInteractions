
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interactable that can react to grab interactions. When it collides with a hand interactor
    /// it attaches to the hand
    /// </summary>
    public class HandGrabInteractable: HandInteractable
    {
        /// <summary>
        /// The offset from the hand position to the object position when grabbed,
        /// so we can keep the object centered around the initial grab position
        /// </summary>
        private Vector3 m_grabPositionOffset;

        /// <summary>
        /// The offset from the hand rotation to the object rotation when grabbed,
        /// so we can keep the object centered around the initial grab position
        /// </summary>
        private Quaternion m_grabRotationOffset;

        /// <summary>
        /// The rigidbody of the object, null if no rigidbody is attached
        /// </summary>
        private Rigidbody m_rigidBody;

        /// <summary>
        /// The initial kinematic state of the rigidbody
        /// </summary>
        private bool m_wasKinematic;

        /// <summary>
        /// Awake
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_rigidBody = GetComponent<Rigidbody>();
            m_wasKinematic = m_rigidBody ? m_rigidBody.isKinematic : false;
        }

        /// <summary>
        /// Update
        /// </summary>
        protected virtual void Update()
        {
            //if we are interacting
            if (CurrentState == InteractionState.Interacting)
            {
                //update the position of the object to match the hand reference system.
                //Remember to offset the position and rotation to keep the object centered around the initial grab position
                //and not make it snap to the center                
                transform.rotation = CurrentInteractor.HandReferenceSystemCalculator.ReferenceSystem.rotation * m_grabRotationOffset;
                transform.position = CurrentInteractor.HandReferenceSystemCalculator.ReferenceSystem.position + transform.rotation * m_grabPositionOffset;
            }
        }

        /// <inheritdoc />
        public override void OnInteractionStarted(IHandInteractor interactor)
        {
            base.OnInteractionStarted(interactor);

            //calculate the offset from the hand position to the object position when grabbed.
            //The position offset will be offset by the inverse rotation because the object will have to rotate
            //around the position where is the hand reference system now. If we just naively take the offset of the position
            //the object will just rotate around its center, which is not what we want... we want it to look like it has been grabbed
            m_grabPositionOffset = Quaternion.Inverse(transform.rotation) * (transform.position - interactor.HandReferenceSystemCalculator.ReferenceSystem.position);
            m_grabRotationOffset = Quaternion.Inverse(interactor.HandReferenceSystemCalculator.ReferenceSystem.rotation) * transform.rotation;

            //if we have a rigidbody, we need to make it kinematic otherwise it will fall down from gravity
            if (m_rigidBody)
                m_rigidBody.isKinematic = true;
        }

        /// <inheritdoc />
        public override void OnInteractionEnded(IHandInteractor interactor)
        {
            base.OnInteractionEnded(interactor);

            //if we have a rigidbody, we need to restore its kinematic state
            if (m_rigidBody)
                m_rigidBody.isKinematic = m_wasKinematic;
        }
        
    }

}