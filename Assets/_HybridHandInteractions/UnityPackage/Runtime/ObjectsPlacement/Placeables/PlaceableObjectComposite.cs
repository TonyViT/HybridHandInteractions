using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Represents an object that can be placed in the scene by the user.
    /// It has some graphics to show during the placement phase and others during the interaction phase.
    /// When the placement is finalized, it disables all the visuals related to placement itself
    /// and activates the interaction ones
    /// </summary>
    public class PlaceableObjectComposite : PlaceableObjectBasic
    {
        /// <summary>
        /// The elements of the object that are shown during the placement phase
        /// </summary>
        [SerializeField]
        private GameObject[] m_placementElements;

        /// <summary>
        /// The elements of the object that are shown during the interaction phase
        /// </summary>
        [SerializeField]
        private GameObject[] m_interactionElements;

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            // Activate placement elements, we assume the element needs to be placed before being interacted
            ChangeState(true); 
        }

        /// <inheritdoc />
        public override void PlacementStarted()
        {
            base.PlacementStarted();
        }

        /// <inheritdoc />
        public override void PlacementEnded()
        {
            base.PlacementEnded();
        }

        /// <inheritdoc />
        public override void PlacementFinalized()
        {
            base.PlacementFinalized();

            //The object is now placed and ready to be interacted with
            ChangeState(false);
        }

        /// <summary>
        /// Changes the state of the object between placement and interaction
        /// </summary>
        /// <param name="isPlacement">True to activate placement mode, false to activate interaction mode</param>
        private void ChangeState(bool isPlacement)
        {
            foreach (var element in m_placementElements)
            {
                element.SetActive(isPlacement);
            }

            foreach (var element in m_interactionElements)
            {
                element.SetActive(!isPlacement);
            }
        }
    }
}