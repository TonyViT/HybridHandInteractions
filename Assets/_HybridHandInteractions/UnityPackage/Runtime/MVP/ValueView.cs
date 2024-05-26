using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Base class for all value views
    /// </summary>
    public abstract class ValueView<TValue> : MonoBehaviour,
        IValueView<TValue> 
    {
        /// <summary>
        /// The value to display
        /// </summary>
        private TValue m_value;

        /// <summary>
        /// Event called when the user changed the value stored by this view
        /// </summary>
        /// <remarks>
        /// This event is triggered only as a result of a user interaction and not 
        /// when the value is changed programmatically via the <see cref="Value"/> property.
        /// </remarks>
        public Action<TValue> UserChangedValue;

        /// <summary>
        /// Get or set the value to display
        /// </summary>
        public TValue Value
        {
            get => m_value;
            set
            {
                if ((m_value == null && value != null) || !m_value.Equals(value))
                {
                    TValue oldValue = m_value;
                    m_value = value;
                    OnValueChanged(oldValue, m_value);
                }
            }
        }

        /// <summary>
        /// Callback called when the value changes
        /// </summary>
        /// <remarks>
        /// This callback is called only as a result of a change in the <see cref="Value"/> property
        /// and should be used by all the views to update themselves to adapt to the new value.
        /// </remarks>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        protected abstract void OnValueChanged(TValue oldValue, TValue newValue);
    }
}