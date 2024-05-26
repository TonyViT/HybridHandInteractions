using System;
using System.Linq;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Presenter for the UI to drive the main operations about objects placement
    /// </summary>
    public class ObjectsPlacementManagerUiPresenter : MonoBehaviour
    {
        /// <summary>
        /// Dropdown view for the items that can be spawned.
        /// </summary>
        [SerializeField]
        private DropdownIntView m_itemsDropdown;

        /// <summary>
        /// Toggle view for the option of aligning with the plane.
        /// </summary>
        [SerializeField]
        private ValueView<bool> m_alignWithPlaneToggle;

        /// <summary>
        /// Toggle view for the option of avoiding compenetration.
        /// </summary>
        [SerializeField]
        private ValueView<bool> m_avoidCompenetrationToggle;

        /// <summary>
        /// Button view for placing a new item.
        /// </summary>
        [SerializeField]
        private ButtonView m_placeNewItemButton;

        /// <summary>
        /// Button view for removing the last item.
        /// </summary>
        [SerializeField]
        private ButtonView m_removeLastItemButton;

        /// <summary>
        /// Button view for clearing items.
        /// </summary>
        [SerializeField]
        private ButtonView m_clearItemsButton;

        /// <summary>
        /// Button view for saving items.
        /// </summary>
        [SerializeField]
        private ButtonView m_saveItemsButton;

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

            //initialize the dropdown with the items that can be placed
            m_itemsDropdown.Options = m_objectsPlacementManager.ItemsToPlace.ItemsToPlace.Select(item => item.ItemName).ToList();

            //initialize the toggle views with the current values of the options
            m_alignWithPlaneToggle.Value = m_objectsPlacementManager.ObjectPlacer.Options.AlignWithPlane;
            m_avoidCompenetrationToggle.Value = m_objectsPlacementManager.ObjectPlacer.Options.AvoidCompenetration;
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            //register to all the events of the views
            m_itemsDropdown.UserChangedValue += OnItemChangedByUser;
            m_alignWithPlaneToggle.UserChangedValue += OnAlignWithPlaneChangedByUser;
            m_avoidCompenetrationToggle.UserChangedValue += OnAvoidCompenetrationChangedByUser;
            m_placeNewItemButton.Clicked += OnPlaceNewItemClicked;
            m_removeLastItemButton.Clicked += OnRemoveLastItemClicked;
            m_clearItemsButton.Clicked += OnClearItemsClicked;
            m_saveItemsButton.Clicked += OnSaveItemsClicked;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            //unregister from all the events of the views
            m_itemsDropdown.UserChangedValue -= OnItemChangedByUser;
            m_alignWithPlaneToggle.UserChangedValue -= OnAlignWithPlaneChangedByUser;
            m_avoidCompenetrationToggle.UserChangedValue -= OnAvoidCompenetrationChangedByUser;
            m_placeNewItemButton.Clicked -= OnPlaceNewItemClicked;
            m_removeLastItemButton.Clicked -= OnRemoveLastItemClicked;
            m_clearItemsButton.Clicked -= OnClearItemsClicked;
            m_saveItemsButton.Clicked -= OnSaveItemsClicked;
        }

        /// <summary>
        /// Method called when the user changes the item in the dropdown
        /// </summary>
        /// <param name="chosenObject">The index of the item chosen by the user</param>
        private void OnItemChangedByUser(int chosenObject)
        {
            //we do nothing, we'll just get the value from the dropdown when we'll need it
        }

        /// <summary>
        /// Method called when the user changes the avoid compenetration option
        /// </summary>
        /// <param name="value">New value of the option</param>
        private void OnAvoidCompenetrationChangedByUser(bool value)
        {
            m_objectsPlacementManager.ObjectPlacer.Options.AvoidCompenetration = value;
        }

        /// <summary>
        /// Method called when the user changes the align with plane option
        /// </summary>
        /// <param name="value">New value of the option</param>
        private void OnAlignWithPlaneChangedByUser(bool value)
        {
            m_objectsPlacementManager.ObjectPlacer.Options.AlignWithPlane = value;
        }

        /// <summary>
        /// Method called when the user clicks on the place new item button
        /// </summary>
        private void OnPlaceNewItemClicked()
        {
            m_objectsPlacementManager.PlaceNewItem(m_objectsPlacementManager.ItemsToPlace.ItemsToPlace[m_itemsDropdown.Value].ItemName);
        }

        /// <summary>
        /// Method called when the user clicks on the remove last item button
        /// </summary>
        private void OnRemoveLastItemClicked()
        {
            m_objectsPlacementManager.RemoveLastItem();
        }

        /// <summary>
        /// Method called when the user clicks on the clear items button
        /// </summary>
        private void OnClearItemsClicked()
        {
            m_objectsPlacementManager.Clear();
        }

        /// <summary>
        /// Method called when the user clicks on the save items button
        /// </summary>
        private void OnSaveItemsClicked()
        {
            SaveItemsSelected?.Invoke();
        }

    }
}