using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Attribute to select a single layer. It serves to make an editor to select only a layer and not a full layer mask
    /// </summary>
    /// <remarks>
    /// Code from https://discussions.unity.com/t/type-for-layer-selection/91723/3
    /// </remarks>
    public class LayerAttribute : PropertyAttribute
    {
        // NOTHING - just oxygen.
    }
}