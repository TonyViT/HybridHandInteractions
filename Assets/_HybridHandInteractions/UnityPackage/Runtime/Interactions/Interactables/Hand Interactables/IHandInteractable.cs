using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for all interactables that can be interacted with by a hand
    /// </summary>
    public interface IHandInteractable
    {
        /// <summary>
        /// The current state of the interactable
        /// </summary>
        InteractionState CurrentState { get; }

        /// <summary>
        /// The main collider of the interactable. It is used to detect interactions
        /// </summary>
        Collider MainCollider { get; }

        /// <summary>
        /// The minimum number of interaction points required to start interacting with this interactable.
        /// For instance, if at least two interaction points are required, the interactable can not be interacted with
        /// by an interactor representing a single finger.
        /// </summary>
        int MinimumInteractionPoints { get; }

        /// <summary>
        /// The interactor that is currently interacting with this interactable
        /// (null if no interactor is interacting with this interactable)
        /// </summary>
        IHandInteractor CurrentInteractor { get; }

        /// <summary>
        /// Event that is triggered when the interaction state of the interactable changes
        /// </summary>
        Action<IHandInteractable, InteractionState> InteractionStateChanged { get; }

        /// <summary>
        /// Function that checks if the interactable can be interacted with by a certain interactor
        /// (e.g. if the interactable is already being interacted with by another interactor, it can not be interacted with)
        /// </summary>
        /// <param name="interactor">The interactor of interest</param>
        /// <returns>True if this interactable can currently be interacted by the interactor, false otherwise</returns>
        bool CanBeInteractedBy(IHandInteractor interactor);

        /// <summary>
        /// Function that must be called when an interaction with this interactable is now possible:
        /// it could start if it is confirmed after a certain amount of time
        /// </summary>
        /// <param name="interactor">Interactor that is carrying on the possible interaction</param>
        void OnInteractionPossibleStarted(IHandInteractor interactor);

        /// <summary>
        /// Function that must be called when an interaction with this interactable was possible but it is not anymore:
        /// it was not confirmed after a certain amount of time
        /// </summary>
        /// <param name="interactor">Interactor that was carrying on the possible interaction</param>
        void OnInteractionPossibleEnded(IHandInteractor interactor);

        /// <summary>
        /// Function that must be called when an interaction with this interactable has started
        /// </summary>
        /// <param name="interactor">Interactor that is carrying on the interaction</param>
        void OnInteractionStarted(IHandInteractor interactor);

        /// <summary>
        /// Function that must be called when an interaction with this interactable has ended
        /// </summary>
        /// <param name="interactor">Interactor that was carrying on the interaction</param>
        void OnInteractionEnded(IHandInteractor interactor);
    }

}