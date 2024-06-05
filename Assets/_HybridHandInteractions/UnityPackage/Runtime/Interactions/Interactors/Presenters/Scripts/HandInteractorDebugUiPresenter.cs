using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter that shows debug info about a <see cref="HandInteractor"/>
    /// </summary>
    public class HandInteractorDebugUiPresenter : MonoBehaviour
    {
        /// <summary>
        /// Interactor that this presenter is showing debug info for
        /// </summary>
        [SerializeField]
        private HandInteractor m_interactor;

        /// <summary>
        /// Label that shows the name of the interactor
        /// </summary>
        [SerializeField]
        private ValueView<string> m_interactorLabel;

        /// <summary>
        /// Label that shows the status of the interactor
        /// </summary>
        [SerializeField]
        private ValueView<string> m_interactorStatusLabel;

        /// <summary>
        /// Label that shows the current interactable of the interactor
        /// </summary>
        [SerializeField]
        private ValueView<string> m_currentInteractableLabel;

        /// <summary>
        /// Label that shows the interactable that is the subject of the latest event of the interactor
        /// </summary>
        [SerializeField]
        private ValueView<string> m_lastEventInteractableLabel;

        /// <summary>
        /// The initial local position of this object
        /// </summary>
        private Vector3 m_initialLocalPosition;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_interactorLabel.Value = $"INTERACTOR {m_interactor.name}";
            m_initialLocalPosition = transform.localPosition;
            OnInteractionStateChanged(null, m_interactor.CurrentState); //to initialize the UI
        }        

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_interactor.InteractionStateChanged += OnInteractionStateChanged;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_interactor.InteractionStateChanged -= OnInteractionStateChanged;
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            //show the debug info close to the hand reference system.
            //We do not set this as a child of the hand reference system because we do not want to change the local scale of this object
            //(remember that the reference system may have a scale, for instance in the case of a pinch interactor)
            transform.position = m_interactor.HandReferenceSystemCalculator.ReferenceSystem.position + m_initialLocalPosition;
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0); //UI elements have negative Z axis as forward
        }

        /// <summary>
        /// Called when the state of the interactor changes
        /// </summary>
        /// <param name="interactable">The interactable involved in the state change of the interactor</param>
        /// <param name="state">The new state of the interactor</param>
        private void OnInteractionStateChanged(IHandInteractable interactable, InteractionState state)
        {
            m_interactorStatusLabel.Value = $"STATE: {state.ToString()}";
            m_currentInteractableLabel.Value = "CURRENT INT:" + ((m_interactor.CurrentInteractable is MonoBehaviour currentInteractableMb) ? currentInteractableMb.name : "None");
            m_lastEventInteractableLabel.Value = "LAST EVENT INT:" + ((interactable is MonoBehaviour interactableMb) ? interactableMb.name : "None");
        }
    }
}