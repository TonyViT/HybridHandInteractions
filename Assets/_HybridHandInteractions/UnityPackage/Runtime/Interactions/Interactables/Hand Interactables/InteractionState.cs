
namespace HybridHandInteractions
{
    /// <summary>
    /// Possible states of interactables and interactors
    /// </summary>
    public enum InteractionState
    {
        /// <summary>
        /// The element is not interacting at all
        /// </summary>
        NonInteracting,

        /// <summary>
        /// The element is potentially interacting, but we are waiting for the interaction to be confirmed
        /// (e.g. by the fact that the interaction is consistent for a certain amount of time)
        /// </summary>
        InteractionPossible,

        /// <summary>
        /// The element is interacting
        /// </summary>
        Interacting
    }

}