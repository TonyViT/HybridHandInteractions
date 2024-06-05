using System;
using System.Linq;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter for the UI to drive the loading operations about objects placement
    /// </summary>
    public class ObjectPlacementLoadingPresenter : MonoBehaviour
    {
        /// <summary>
        /// The storage from where to load the items
        /// </summary>
        [SerializeField]
        private DataStorage m_saveDataStorage;

        /// <summary>
        /// Key slots to use for saving the data. If null, the default key will be used
        /// </summary>
        [SerializeField]
        private DataStorageKeys m_savingSlotsKeys;

        /// <summary>
        /// Dropdown view for the save slots to be used to load the items
        /// </summary>
        [SerializeField]
        private DropdownIntView m_saveSlotsDropdown;

        /// <summary>
        /// Button to trigger the loading
        /// </summary>
        [SerializeField]
        private ButtonView m_loadButton;

        /// <summary>
        /// Button view for finalize items. It can be null if this loading window does not have
        /// finalization feature
        /// </summary>
        [SerializeField]
        private ButtonView m_finalizeItemsButton;

        /// <summary>
        /// If true, the presenter will notify the load event when the user click to press load.
        /// Otherwise, the button click will not be notified and the objects will be loaded without
        /// the user returning to main menu (this is useful if there is a finalize button to return to main menu
        /// only when objects are finalized)
        /// </summary>
        [SerializeField]
        private bool m_notifyLoad = true;

        /// <summary>
        /// Interface for the objects placement manager.
        /// </summary>
        private IObjectsPlacementManager m_objectsPlacementManager;

        /// <summary>
        /// Event called when the user selects to load the items
        /// </summary>
        public Action LoadItemsSelected;

        /// <summary>
        /// Event called when the user selects to finalize the items
        /// </summary>
        public Action FinalizeItemsSelected;

        /// <summary>
        /// Initialize this presenter
        /// </summary>
        /// <param name="objectsPlacementManager">The objects placer manager to present</param>
        public void Init(IObjectsPlacementManager objectsPlacementManager)
        {
            m_objectsPlacementManager = objectsPlacementManager;

            //initialize the dropdown of saving slots
            m_saveSlotsDropdown.Options = (m_savingSlotsKeys == null ? new string[] { DataStorageKeys.DefaultKey } : m_savingSlotsKeys.ApplicationStorageKeys).ToList();            
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_loadButton.Clicked += OnLoadButtonClicked;

            if(m_finalizeItemsButton)
                m_finalizeItemsButton.Clicked += OnFinalizeItemsClicked;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_loadButton.Clicked -= OnLoadButtonClicked;        

            if(m_finalizeItemsButton)
                m_finalizeItemsButton.Clicked -= OnFinalizeItemsClicked;
        }

        /// <summary>
        /// Callback called when the user hits the load button
        /// </summary>
        private void OnLoadButtonClicked()
        {
            //get the serialized objects from the selected slot
            string key = m_saveSlotsDropdown.Options[m_saveSlotsDropdown.Value];
            string serializedObjectsString = m_saveDataStorage.LoadData<string>(key);

            //deserialize the objects
            m_objectsPlacementManager.Deserialize(serializedObjectsString);

            //invoke the event
            if(m_notifyLoad)
                LoadItemsSelected?.Invoke();
        }

        /// <summary>
        /// Method called when the user clicks on the finalize items button
        /// </summary>
        private void OnFinalizeItemsClicked()
        {
            FinalizeItemsSelected?.Invoke();
        }
    }
}