using System;
using System.Linq;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter for the UI that lets the user control the placement of the current object
    /// </summary>
    public class ObjectPlacementInProgressPresenter : MonoBehaviour
    {
        /// <summary>
        /// Button view for confirm the placement of the current item
        /// </summary>
        [SerializeField]
        private ButtonView m_confirmItemPlacementButton;

        /// <summary>
        /// Interface for the object placer
        /// </summary>
        private IObjectPlacer m_objectPlacer;

        /// <summary>
        /// Event called when the user selects to confirm the item placement
        /// </summary>
        public Action ItemPlacementConfirmed;

        /// <summary>
        /// Initialize this presenter
        /// </summary>
        /// <param name="objectPlacer">The object placer to present</param>
        public void Init(IObjectPlacer objectPlacer)
        {
            m_objectPlacer = objectPlacer;
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            //register to all the events of the views
            m_confirmItemPlacementButton.Clicked += OnConfirmItemPlacementClicked;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            //unregister from all the events of the views
            m_confirmItemPlacementButton.Clicked -= OnConfirmItemPlacementClicked;
        }

        /// <summary>
        /// Handles the event when the confirm item placement button is clicked.
        /// </summary>
        private void OnConfirmItemPlacementClicked()
        {
            //Check if the object placer is not null
            if (m_objectPlacer != null)
            {
                //Confirm the placement of the object
                m_objectPlacer.EndPlacement();

                //Invoke the ItemPlacementConfirmed event
                ItemPlacementConfirmed?.Invoke();
            }
            else
            {
                Debug.LogError("[ObjectPlacementInProgressPresenter] Object Placer is not initialized.");
            }
        }
    }

}