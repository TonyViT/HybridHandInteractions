using System;
using UnityEngine;
using UnityEngine.UI;

namespace HybridHandInteractions
{
    /// <summary>
    /// Treats a Unity Button as a button view, automatically calling the OnClick method when the button is clicked.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonUnityView : ButtonView
    {
        /// <summary>
        /// The button that is being managed
        /// </summary>
        private Button m_button;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_button = GetComponent<Button>();
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            m_button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            m_button.onClick.RemoveListener(OnClick);
        }
    }
}