using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace HybridHandInteractions
{
    /// <summary>
    /// View that represents an integer through a choice you can make in a dropdown.
    /// The string values have to be passed to this view, and the index of the selected value will be the integer value.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownIntView : ValueView<int>
    {
        /// <summary>
        /// The dropdown where you can select the string value
        /// </summary>
        private TMP_Dropdown m_dropdown;

        /// <summary>
        /// Get or set the options of the dropdown
        /// </summary>
        public List<string> Options
        {
            get
            {
                return Dropdown.options.ConvertAll(option => option.text);
            }   
            set
            {
                Dropdown.ClearOptions();
                Dropdown.AddOptions(value);
            }
        }

        /// <summary>
        /// Get the dropdown associated with this component
        /// </summary>
        private TMP_Dropdown Dropdown
        {
            get
            {
                //sometimes this gets called on disabled gameobjects so before Awake is called,
                //so we need to get the reference to the dropdown if it is null
                if (m_dropdown == null)
                    m_dropdown = GetComponent<TMP_Dropdown>();

                return m_dropdown;
            }
        }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_dropdown = GetComponent<TMP_Dropdown>();
        }

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        /// <summary>
        /// On Disable
        /// </summary>
        private void OnDisable()
        {
            Dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }

        /// <summary>
        /// Callback called when the dropdown value changes
        /// </summary>
        /// <param name="newValue">The new value from the dropdown</param>
        private void OnDropdownValueChanged(int newValue)
        {
            if (newValue != Value)
            {
                Value = newValue;
                UserChangedValue?.Invoke(Value);
            }
        }

        /// <inheritdoc/>
        protected override void OnValueChanged(int oldValue, int newValue)
        {
            Dropdown.value = newValue;
        }
    }
}