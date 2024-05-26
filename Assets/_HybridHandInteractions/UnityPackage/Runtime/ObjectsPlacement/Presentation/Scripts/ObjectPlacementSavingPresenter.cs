using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter for the UI to drive the saving operations about objects placement
    /// </summary>
    public class ObjectPlacementSavingPresenter : MonoBehaviour
    {
        /// <summary>
        /// The storage where to save the items
        /// </summary>
        [SerializeField]
        private DataStorage m_saveDataStorage;

        /// <summary>
        /// Key slots to use for saving the data. If null, the default key will be used
        /// </summary>
        [SerializeField]
        private DataStorageKeys m_savingSlotsKeys;

        /// <summary>
        /// Chooses which classification to use as default
        /// </summary>
        [SerializeField]
        private PlaneClassification m_defaultClassification = PlaneClassification.Floor;

        /// <summary>
        /// Dropdown view for the classification of the planes to be used as a reference of the placed items
        /// </summary>
        [SerializeField]
        private DropdownIntView m_referencePlanesClassificationsDropdown;

        /// <summary>
        /// Dropdown view for the save slots to be used to save the items
        /// </summary>
        [SerializeField]
        private DropdownIntView m_saveSlotsDropdown;

        /// <summary>
        /// Button to trigger the saving
        /// </summary>
        [SerializeField]
        private ButtonView m_saveButton;

        /// <summary>
        /// Interface for the objects placement manager.
        /// </summary>
        private IObjectsPlacementManager m_objectsPlacementManager;

        /// <summary>
        /// Event called when the user selects to save the items
        /// </summary>
        public Action SaveItemsSelected;

        /// <summary>
        /// Initialize this presenter
        /// </summary>
        /// <param name="objectsPlacementManager">The objects placer manager to present</param>
        public void Init(IObjectsPlacementManager objectsPlacementManager)
        {
            m_objectsPlacementManager = objectsPlacementManager;

            //initialize the dropdown of the classification using the Enum values of the classification
            m_referencePlanesClassificationsDropdown.Options = Enum.GetNames(typeof(PlaneClassification)).ToList();
            m_referencePlanesClassificationsDropdown.Value = m_referencePlanesClassificationsDropdown.Options.IndexOf(m_defaultClassification.ToString());

            //initialize the dropdown of saving slots
            m_saveSlotsDropdown.Options = (m_savingSlotsKeys == null ? new string[] { DataStorageKeys.DefaultKey } : m_savingSlotsKeys.ApplicationStorageKeys).ToList();            
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_saveButton.Clicked += OnSaveButtonClicked;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_saveButton.Clicked -= OnSaveButtonClicked;        
        }

        /// <summary>
        /// Callback called when the user hits the save button
        /// </summary>
        private void OnSaveButtonClicked()
        {
            //serialize the objects using the classification selected
            string objectsSerialization = m_objectsPlacementManager.Serialize(Enum.Parse<PlaneClassification>(m_referencePlanesClassificationsDropdown.Options[m_referencePlanesClassificationsDropdown.Value]));

            //save the objects at the selected slot
            string key = m_saveSlotsDropdown.Options[m_saveSlotsDropdown.Value];
            m_saveDataStorage.SaveData(key, objectsSerialization);

            //invoke the event
            SaveItemsSelected?.Invoke();

            //uncomment this lines to see the deserialization in the console to check everything worked
            //string deserialized = m_saveDataStorage.LoadData<string>(key);
            //Debug.LogFormat("[ObjectPlacementSavingPresenter] Checking the deserialization of {0}... Found {1}", key, deserialized);
        }
    }
}