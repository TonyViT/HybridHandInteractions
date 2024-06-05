using System;
using UnityEngine;
using UnityEngine.UI;

namespace HybridHandInteractions
{
    /// <summary>
    /// Implements the logic of changing the colors of a renderer to reflect the status 
    /// of a <see cref="HandInteractable"/> object 
    /// </summary>
    public class HandInteractableColorChanger : MonoBehaviour
    {
        //TODO: This class is very similar to the PlaceableObjectBasic class, maybe a common base class or a component
        //to be added via composition can be created

        /// <summary>
        /// The hand interactable object that this color changer is associated with. If it is null,
        /// the class will try to get the component from the same GameObject
        /// </summary>
        [SerializeField]
        private HandInteractable m_handInteractable;

        /// <summary>
        /// The elements of the object that can change color to reflect the state of the object.
        /// It can be an empty array
        /// </summary>
        [SerializeField]
        private Renderer[] m_colorableElements;

        /// <summary>
        /// The property of the materials that is used to change the color of the object
        /// </summary>
        [SerializeField]
        private string m_materialColorProperty = "_BaseColor";

        /// <summary>
        /// Specifies the colors that the object will have during the various phases of interaction.
        /// For now  the Normal color is used when the object is not being interacted, Highlighted color is used for the "possible" interactions
        /// and the Pressed color is used when the object is being interacted.
        /// The other colors may be used in the future (e.g. to reflect a disabled object)
        /// </summary>
        [SerializeField]
        private ColorBlock m_colors;

        /// <summary>
        /// Awake
        /// </summary>
        protected void Awake()
        {
            if(m_handInteractable == null)
                m_handInteractable = GetComponent<HandInteractable>();

            //initialize with idle color
            ChangeColor(m_colors.normalColor);
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            //register to the event of the hand interactable
            m_handInteractable.InteractionStateChanged += OnInteractionStateChanged;
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            //unregister from the event of the hand interactable
            m_handInteractable.InteractionStateChanged -= OnInteractionStateChanged;
        }

        /// <summary>
        /// Callback called when the interaction state of the hand interactable changes
        /// </summary>
        /// <param name="interactable">The interactable casting the event</param>
        /// <param name="state">The new interaction status</param>
        private void OnInteractionStateChanged(IHandInteractable interactable, InteractionState state)
        {
            switch (state)
            {
                case InteractionState.NonInteracting:
                    ChangeColor(m_colors.normalColor);
                    break;
                case InteractionState.InteractionPossible:
                    ChangeColor(m_colors.highlightedColor);
                    break;
                case InteractionState.Interacting:
                    ChangeColor(m_colors.pressedColor);
                    break;
                default:
                    throw new ArgumentException("Illegal interaction state: variable " + nameof(state));
            }
        }

        /// <summary>
        /// Changes the color of the object to a certain color
        /// </summary>
        /// <param name="color">The color to change the object to</param>
        private void ChangeColor(Color color)
        {
            foreach (var element in m_colorableElements)
            {
                element.material.SetColor(m_materialColorProperty, color);
            }
        }
    }
}