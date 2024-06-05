using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for all interactors that interact via the bare hand
    /// </summary>
    public interface IHandInteractor
    {
        /// <summary>
        /// The current state of the interactable
        /// </summary>
        InteractionState CurrentState { get; }

        /// <summary>
        /// The various colliders that make up the interactor
        /// (e.g. the pinch interactor will have one on the index and one on the thumb)
        /// </summary>
        Collider[] InteractionColliders { get; }

        /// <summary>
        /// The time an interaction must be confirmed before it is considered started.
        /// Before this time is passed, the interaction is considered "possible" but not started.
        /// </summary>
        float InteractionStartConfirmationTime { get; }

        /// <summary>
        /// The time an interaction must be not available anymore before it is considered ended.
        /// Before this time is passed, the interaction is still considered ongoing,
        /// with just a temporary glitch happening.
        /// </summary>
        float InteractionEndConfirmationTime { get; }

        /// <summary>
        /// The interactable that is currently being interacted with by this interactor, if any
        /// (null if no interactable is being interacted with)
        /// </summary>
        IHandInteractable CurrentInteractable { get; }

        /// <summary>
        /// The element that provides the reference system for the hand for this interactor
        /// (the interactables could use it for instance to attach to the hand)
        /// </summary>
        IHandReferenceSystemCalculator HandReferenceSystemCalculator { get; }

        /// <summary>
        /// Event that is triggered when the interaction state of the interactor changes
        /// </summary>
        Action<IHandInteractable, InteractionState> InteractionStateChanged { get; }
    }

}