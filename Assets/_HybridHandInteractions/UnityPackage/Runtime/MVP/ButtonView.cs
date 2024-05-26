using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Manages an element that can be treated as a button.    
    /// </summary>
    /// <remarks>
    /// Notice that this is a generic view for whatever element that may act as a button.
    /// If you need to represent a Unity button, you must use the <see cref="ButtonUnityView"/> 
    /// </remarks>
    public class ButtonView : MonoBehaviour, IView
    {
        /// <summary>
        /// Event that is called when the button is clicked.
        /// </summary>
        public Action Clicked;

        /// <summary>
        /// Called via the Inspector when the button is clicked.
        /// </summary>
        public void OnClick()
        {
            Clicked?.Invoke();
        }
    }
}