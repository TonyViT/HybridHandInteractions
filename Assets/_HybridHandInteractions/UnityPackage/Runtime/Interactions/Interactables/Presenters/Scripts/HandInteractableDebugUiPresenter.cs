using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter that shows debug info about a <see cref="HandInteractable"/>
    /// </summary>
    public class HandInteractableDebugUiPresenter : MonoBehaviour
    {
        /// <summary>
        /// Interactable that this presenter is showing debug info for
        /// </summary>
        [SerializeField]
        private HandInteractable m_interactable;

        /// <summary>
        /// Label that shows the name of the interactable
        /// </summary>
        [SerializeField]
        private ValueView<string> m_interactableLabel;

        /// <summary>
        /// Label that shows the status of the interactable
        /// </summary>
        [SerializeField]
        private ValueView<string> m_interactableStatusLabel;

        /// <summary>
        /// Label that shows the current interactor of the interactable
        /// </summary>
        [SerializeField]
        private ValueView<string> m_currentInteractorLabel;

        /// <summary>
        /// The initial local position of this object
        /// </summary>
        private Vector3 m_initialLocalPosition;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_interactableLabel.Value = $"INTERACTABLE {m_interactable.name}";
            m_initialLocalPosition = transform.localPosition;
            OnInteractionStateChanged(m_interactable, m_interactable.CurrentState); //to initialize the UI
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_interactable.InteractionStateChanged += OnInteractionStateChanged;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_interactable.InteractionStateChanged -= OnInteractionStateChanged;
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            //we assume the dev has put this element already where he wanted it to be, so we just work with its rotation
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0); //UI elements have negative Z axis as forward
        }

        /// <summary>
        /// Callback called when the interaction state of the hand interactable changes
        /// </summary>
        /// <param name="interactable">The interactable casting the event</param>
        /// <param name="state">The new interaction status</param>
        private void OnInteractionStateChanged(IHandInteractable interactable, InteractionState state)
        {
            m_interactableStatusLabel.Value = $"STATE: {state.ToString()}";
            m_currentInteractorLabel.Value = "CURRENT INT:" + ((m_interactable.CurrentInteractor is MonoBehaviour currentInteractorMb) ? currentInteractorMb.name : "None");         
        }
    }
}