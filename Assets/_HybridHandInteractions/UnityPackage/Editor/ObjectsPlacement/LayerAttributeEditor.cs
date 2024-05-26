using UnityEditor;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Attribute editor to select a single layer. It serves to make an editor to select only a layer and not a full layer mask
    /// </summary>
    /// <remarks>
    /// Code from https://discussions.unity.com/t/type-for-layer-selection/91723/3
    /// </remarks>
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    class LayerAttributeEditor : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // One line of  oxygen free code.
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }

    }

}