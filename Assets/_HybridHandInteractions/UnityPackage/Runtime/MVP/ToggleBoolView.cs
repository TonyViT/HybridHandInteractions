using UnityEngine.UI;

namespace HybridHandInteractions
{
    /// <summary>
    /// View that represents a boolean value through a toggle.
    /// </summary>
    public class ToggleBoolView : ValueView<bool>
    {
        /// <summary>
        /// The button that is being managed
        /// </summary>
        private Toggle m_toggle;

        /// <summary>
        /// Get the dropdown associated with this component
        /// </summary>
        private Toggle Toggle
        {
            get
            {
                //sometimes this gets called on disabled gameobjects so before Awake is called,
                //so we need to get the reference to the dropdown if it is null
                if (m_toggle == null)
                    m_toggle = GetComponent<Toggle>();

                return m_toggle;
            }
        }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_toggle = GetComponent<Toggle>();
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            Toggle.onValueChanged.AddListener(OnToggleValueChangedByUser);
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            Toggle.onValueChanged.RemoveListener(OnToggleValueChangedByUser);
        }

        /// <inheritdoc />
        protected override void OnValueChanged(bool oldValue, bool newValue)
        {
            Toggle.isOn = newValue;
        }

        /// <summary>
        /// Called when the toggle value is changed by the user.
        /// </summary>
        /// <param name="newValue">New value of the toggle</param>
        private void OnToggleValueChangedByUser(bool newValue)
        {
            UserChangedValue?.Invoke(newValue);
        }

    }
}