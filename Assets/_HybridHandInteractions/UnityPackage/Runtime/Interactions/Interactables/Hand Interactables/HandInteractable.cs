using System;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Base class for all interactables that can be interacted with by a hand
    /// </summary>
    /// <remarks>
    /// It is fundamental that the collider is on the same gameobject as the script to make the interaction system to work
    /// </remarks>
    [RequireComponent(typeof(Collider))]
    public abstract class HandInteractable: MonoBehaviour,
        IHandInteractable
    {
        /// <inheritdoc />
        [field: SerializeField]
        public int MinimumInteractionPoints { get; protected set; }

        /// <inheritdoc />
        public InteractionState CurrentState { get; protected set; }

        /// <inheritdoc />
        public Collider MainCollider { get; protected set; }

        /// <inheritdoc />
        public IHandInteractor CurrentInteractor { get; protected set; }

        /// <inheritdoc />
        public Action<IHandInteractable, InteractionState> InteractionStateChanged { get; set; }

        /// <summary>
        /// The number of "possible interactions" that are currently active on this interactable.
        /// This element can be in an "interaction possible" state with many interactors in the scene,
        /// so we need to keep count of it
        /// </summary>
        protected int CurrentPossibleInteractionsCount { get; set; } = 0;

        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            MainCollider = GetComponent<Collider>();
        }

        /// <inheritdoc />
        public virtual bool CanBeInteractedBy(IHandInteractor interactor)
        {
            //we can be interacted if:
            // - we are not interacting with anyone and the interactor has enough interaction points
            // - we are interacting with someone and the interactor is exactly the one with which we are interacting
            return interactor != null && 
                ((CurrentState != InteractionState.Interacting && interactor.InteractionColliders.Length >= MinimumInteractionPoints) || 
                (CurrentState == InteractionState.Interacting && CurrentInteractor == interactor));
        }

        /// <inheritdoc />
        public virtual void OnInteractionPossibleStarted(IHandInteractor interactor)
        {
            CurrentPossibleInteractionsCount++;

            //if this is the first time we are in a possible interaction, we need to change our status
            if (CurrentPossibleInteractionsCount == 1 && CurrentState == InteractionState.NonInteracting)
            {
                CurrentState = InteractionState.InteractionPossible;
                InteractionStateChanged?.Invoke(this, CurrentState);
            }
        }

        /// <inheritdoc />
        public virtual void OnInteractionPossibleEnded(IHandInteractor interactor)
        {
            CurrentPossibleInteractionsCount--;

            //if this is the last possible interaction, we need to change our status
            if (CurrentPossibleInteractionsCount == 0 && CurrentState == InteractionState.InteractionPossible)
            {
                CurrentState = InteractionState.NonInteracting;
                InteractionStateChanged?.Invoke(this, CurrentState);
            }
        }

        /// <inheritdoc />
        public virtual void OnInteractionStarted(IHandInteractor interactor)
        {
            //we escalated from possible to interacting, so we need to decrease the count of possible interactions
            //because the current interaction is not "possible" anymore
            if(CurrentState == InteractionState.InteractionPossible)
                CurrentPossibleInteractionsCount--;

            //update our status
            CurrentState = InteractionState.Interacting;
            CurrentInteractor = interactor;
            InteractionStateChanged?.Invoke(this, CurrentState);
        }

        /// <inheritdoc />
        public virtual void OnInteractionEnded(IHandInteractor interactor)
        {
            //update our status
            CurrentState = CurrentPossibleInteractionsCount == 0 ? InteractionState.NonInteracting : InteractionState.InteractionPossible;
            CurrentInteractor = null;
            InteractionStateChanged?.Invoke(this, CurrentState);
        }
    }

}