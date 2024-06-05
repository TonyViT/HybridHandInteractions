using TMPro;
using UnityEngine.UI;

namespace HybridHandInteractions
{
    /// <summary>
    /// View that represents a string value through a Text Mesh Pro label.
    /// </summary>
    public class TextStringView : ValueView<string>
    {
        /// <summary>
        /// The text that is being managed
        /// </summary>
        private TMP_Text m_text;

        /// <summary>
        /// Get the text associated with this component
        /// </summary>
        private TMP_Text Text
        {
            get
            {
                //sometimes this gets called on disabled gameobjects so before Awake is called,
                //so we need to get the reference to the dropdown if it is null
                if (m_text == null)
                    m_text = GetComponent<TMP_Text>();

                return m_text;
            }
        }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        /// <inheritdoc />
        protected override void OnValueChanged(string oldValue, string newValue)
        {
            Text.text = newValue;
        }

    }
}