using UnityEngine;
using UnityEngine.UI;

namespace HybridHandInteractions
{
    /// <summary>
    /// Represents a basic object that can be placed in the scene by the user.
    /// It changes color during the various phases of selection and placement
    /// </summary>
    public class PlaceableObjectBasic : PlaceableObject
    {
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
        /// Specifies the colors that the object will have during the various phases of selection and placement.
        /// For now only the Normal color is used when the object is not being placed and the Pressed color is used when the object is being placed.
        /// The other colors may be used in the future (e.g. when the object is selected but not being placed)
        /// </summary>
        [SerializeField]
        private ColorBlock m_colors;

        /// <summary>
        /// Awake
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            
            //initialize with idle color
            ChangeColor(m_colors.normalColor);
        }

        /// <inheritdoc />
        public override void PlacementStarted()
        {
            ChangeColor(m_colors.pressedColor);
        }

        /// <inheritdoc />
        public override void PlacementEnded()
        {
            ChangeColor(m_colors.normalColor);
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