using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Main presenter that controls the overall UI to drive the objects placement manager
    /// </summary>
    public class ObjectsPlacementMainUiPresenter : MonoBehaviour
    {
        /// <summary>
        /// Possible states of the presenter
        /// </summary>
        private enum State
        {
            MainMenu,
            PlacingObject,
            SavingObjects
        };

        /// <summary>
        /// Gameobject containing the objects placement manager
        /// </summary>
        [SerializeField]
        private GameObject m_objectsPlacementManagerGo;

        /// <summary>
        /// The presenter that controls the UI for the objects placement manager
        /// </summary>
        [SerializeField]
        private ObjectsPlacementManagerUiPresenter m_managerUiPresenter;

        /// <summary>
        /// The presenter that controls the UI for the object placement in progress
        /// </summary>
        [SerializeField]
        private ObjectPlacementInProgressPresenter m_objectPlacementInProgressPresenter;

        /// <summary>
        /// The presenter that controls the UI for the object placement saving
        /// </summary>
        [SerializeField]
        private ObjectPlacementSavingPresenter m_objectPlacementSavingPresenter;

        /// <summary>
        /// Interface for the objects placement manager.
        /// </summary>
        private IObjectsPlacementManager m_objectsPlacementManager;

        /// <summary>
        /// The array of the gameobjects that are the presenters.
        /// Enabling/disabling them we can show different parts of the UI
        /// </summary>
        private GameObject[] m_presenters;

        /// <summary>
        /// The current state of the presenter
        /// </summary>
        private State m_currentState;

        /// <summary>
        /// Get or set the current state of the presenter,
        /// and enable/disable the corresponding sub-presenters if needed
        /// </summary>
        private State CurrentState
        {
            get => m_currentState;
            set
            {
                m_currentState = value;
                
                for(int i = 0; i < m_presenters.Length; i++)
                    m_presenters[i].SetActive(i == (int)m_currentState);
            }
        }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_objectsPlacementManager = m_objectsPlacementManagerGo.GetComponent<IObjectsPlacementManager>();       
            
            m_presenters = new GameObject[]
            {
                m_managerUiPresenter.gameObject,
                m_objectPlacementInProgressPresenter.gameObject,
                m_objectPlacementSavingPresenter.gameObject
            };
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_managerUiPresenter.SaveItemsSelected += OnSaveItemsSelected;
            m_objectsPlacementManager.ObjectPlacer.ObjectPlacementStarted += OnItemPlacementStarted;
            m_objectsPlacementManager.ObjectPlacer.ObjectPlacementEnded += OnItemPlacementEnded;
            m_objectPlacementSavingPresenter.SaveItemsSelected += OnSaveFinalizationButtonClicked;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_managerUiPresenter.SaveItemsSelected -= OnSaveItemsSelected;
            m_objectsPlacementManager.ObjectPlacer.ObjectPlacementStarted -= OnItemPlacementStarted;
            m_objectsPlacementManager.ObjectPlacer.ObjectPlacementEnded -= OnItemPlacementEnded;
            m_objectPlacementSavingPresenter.SaveItemsSelected -= OnSaveFinalizationButtonClicked;
        }

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            m_managerUiPresenter.Init(m_objectsPlacementManager);
            m_objectPlacementInProgressPresenter.Init(m_objectsPlacementManager.ObjectPlacer);
            m_objectPlacementSavingPresenter.Init(m_objectsPlacementManager);
            CurrentState = State.MainMenu;
        }

        /// <summary>
        /// Method called when the user selects to save the items
        /// </summary>
        private void OnSaveItemsSelected()
        {
            CurrentState = State.SavingObjects;
        }

        /// <summary>
        /// Method called when the user starts placing an item
        /// </summary>
        /// <param name="placeableObject">Object of interest</param>
        private void OnItemPlacementStarted(PlaceableObject placeableObject)
        {
            CurrentState = State.PlacingObject;
        }

        /// <summary>
        /// Method called when the user confirms the the current item is placed
        /// </summary>
        /// <param name="placeableObject">Object of interest</param>
        private void OnItemPlacementEnded(PlaceableObject placeableObject)
        {
            CurrentState = State.MainMenu;
        }

        /// <summary>
        /// Method called when the user confirms the finalization of the saving of the items
        /// </summary>
        private void OnSaveFinalizationButtonClicked()
        {
            CurrentState = State.MainMenu;
        }

#if UNITY_EDITOR

        /// <summary>
        /// On Validate
        /// </summary>
        private void OnValidate()
        {
            //make sure the object assigned to the Objects Placement Manager field implements the IObjectsPlacementManager interface
            if (m_objectsPlacementManagerGo != null)
            {
                if (m_objectsPlacementManagerGo.GetComponent<IObjectsPlacementManager>() == null)
                {
                    Debug.LogError("The object assigned to the Objects Placement Manager field must implement the IObjectsPlacementManager interface");
                    m_objectsPlacementManagerGo = null;
                }
            }
        }

#endif

    }
}