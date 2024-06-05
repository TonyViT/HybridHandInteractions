using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HybridHandInteractions
{
    /// <summary>
    /// Implements the basic logic for all interactors that interact via the bare hand
    /// </summary>
    /// <remarks>
    /// In this class we assume that the joints that make up the reference system of the hand are the same that should be 
    /// used to evaluate if this interactor is interacting with an interactable: e.g. if the interactor is a pinch interactor
    /// it is using the thumb and the index finger to calculate its reference system, and it is considering to interact with 
    /// an interactable only if the thumb and the index finger are colliding with it.
    /// If this assumption is not true in this case, use the subclass <see cref="HandInteractorCustomJointColliders"/> .
    /// </remarks>
    public class HandInteractor: MonoBehaviour,
        IHandInteractor
    {
        /// <summary>
        /// Class that memorizes a current interaction state and the colliders that are currently interacting.
        /// It is used to check which colliders of this interactor are actually interacting with the interactable.
        /// </summary>
        protected class CollidersInteractionData
        {
            /// <summary>
            /// The colliders that are currently colliding with the interactable
            /// </summary>
            public List<Collider> CollidingColliders;

            /// <summary>
            /// The last time we entered or exited a collision with the interactable and the entries of the 
            /// <see cref="CollidingColliders"/> list have been changed so that either we collide with all the joints
            /// or not anymore. Basically this is the time we either have a new interaction becoming possible because we 
            /// now collide with all the joints, or an existing interaction becoming not possible anymore because we stopped colliding
            /// with all the joints
            /// </summary>
            public float LastCollisionMajorEventTime;

            /// <summary>
            /// The state of this collision interaction
            /// </summary>
            public InteractionState InteractionState;
        }

        /// <summary>
        /// Reference to the <see cref="HandReferenceSystemCalculator"/> in the scene that provides the reference system for the hand
        /// </summary>
        [field: SerializeField]
        protected HandReferenceSystemCalculator HandReferenceSystemCalculatorBehaviour { get; set; }

        /// <inheritdoc />
        [field: SerializeField]
        public float InteractionStartConfirmationTime { get; set; } = 0.75f;

        /// <inheritdoc />
        [field: SerializeField]
        public float InteractionEndConfirmationTime { get; set; } = 0.75f;

        /// <summary>
        /// The diameter of the colliders used to detect interactions
        /// </summary>
        /// <remarks>
        /// This class will add some trigger spheres to the various hand joints of the hand involved in the interactor: if all the joints
        /// are colliding with the interactable for enough time, the interaction is considered started. We need to know which is the diameter of these
        /// spheres to add
        /// </remarks>
        [field: SerializeField]
        private float m_interactionCollidersDiameter = 0.03f;

        /// <inheritdoc />
        public InteractionState CurrentState { get; protected set; }

        /// <inheritdoc />
        public Collider[] InteractionColliders { get; protected set; }

        /// <inheritdoc />
        public IHandInteractable CurrentInteractable { get; protected set; }

        /// <inheritdoc />
        public IHandReferenceSystemCalculator HandReferenceSystemCalculator => HandReferenceSystemCalculatorBehaviour;

        /// <summary>
        /// The potential interactions that this interactor could start.
        /// The key is the interactable, the value is the data about the interaction with this interactable.
        /// </summary>
        private Dictionary<IHandInteractable, CollidersInteractionData> m_potentialInteractions = new Dictionary<IHandInteractable, CollidersInteractionData>();

        /// <inheritdoc />
        public Action<IHandInteractable, InteractionState> InteractionStateChanged { get; set; }

        /// <summary>
        /// Start
        /// </summary>
        protected virtual void Start()
        {
            InteractionColliders = new Collider[0];
            CurrentState = InteractionState.NonInteracting;

            CreateInteractionColliders();
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            DestroyInteractionColliders();
        }

        /// <summary>
        /// Update
        /// </summary>
        protected virtual void Update()
        {
            //check the current collision data and evaluate if we should start or end an interaction
            EvaluateInteractions();
        }

        /// <summary>
        /// Create the interaction colliders on the hand joints of interest
        /// (if the collider of the interactable collides with all of them, the interaction is considered possible/started)
        /// </summary>
        protected virtual void CreateInteractionColliders()
        {
            //gets the list of interaction joints
            var collidingJointsTransforms = GetInteractionJointsTransforms();
            
            //creates a sphere collider for each joint, adding it as a gameobject child of the joint
            InteractionColliders = new Collider[collidingJointsTransforms.Length];

            for (int i = 0; i < InteractionColliders.Length; i++)
            {
                GameObject go = new GameObject("InteractionCollider_" + i);
                go.transform.SetParent(collidingJointsTransforms[i], false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;

                SphereCollider sphereCollider = go.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = true;
                sphereCollider.radius = m_interactionCollidersDiameter / 2;

                //we also add a rigidbody otherwise the trigger events are not fired
                Rigidbody rigidBody = go.AddComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                rigidBody.useGravity = false;

                InteractionColliders[i] = sphereCollider;

                //remember to add a broadcaster to be able to be notified of the events of that collider
                var broadcastColliderEvents = go.AddComponent<BroadcastColliderEvents>();
                broadcastColliderEvents.CollisionEntered += OnInteractionColliderEnter;
                broadcastColliderEvents.CollisionExited += OnInteractionColliderExit;
                broadcastColliderEvents.TriggerEntered += OnInteractionColliderEnter;
                broadcastColliderEvents.TriggerExited += OnInteractionColliderExit;
            }

        }

        /// <summary>
        /// Destroy the interaction colliders on the hand joints of interest
        /// </summary>
        protected virtual void DestroyInteractionColliders()
        {
            //there are various null-checks because this method could be called in the OnDestroy method, and the objects could be already destroyed
            if(InteractionColliders == null)
                return;

            //loop all the interaction colliders
            for (int i = 0; i < InteractionColliders.Length; i++)
            {
                if (InteractionColliders[i] == null)
                    continue;

                //unregister from all the collision events
                if (InteractionColliders[i].gameObject.TryGetComponent(out BroadcastColliderEvents broadcastColliderEvents))
                {
                    broadcastColliderEvents.CollisionEntered -= OnInteractionColliderEnter;
                    broadcastColliderEvents.CollisionExited -= OnInteractionColliderExit;
                    broadcastColliderEvents.TriggerEntered -= OnInteractionColliderEnter;
                    broadcastColliderEvents.TriggerExited -= OnInteractionColliderExit;
                }

                //destroy the gameobject we created to add the collider
                Destroy(InteractionColliders[i].gameObject);
            }
        }       

        /// <summary>
        /// Get which are the joint transforms that should be used to detect if an interactable is colliding with this interactor
        /// (if the collider of the interactable collides with all of them, the interaction is considered possible/started)
        /// </summary>
        /// <returns>List of transforms of interest</returns>
        protected virtual Transform[] GetInteractionJointsTransforms()
        {
            return HandReferenceSystemCalculator.LastHandJoints.Values.ToArray();
        }

        /// <summary>
        /// Callback called when a collider we put on the hands joint enters in collision with another collider.
        /// This is called both for trigger collisions and standard collisions.
        /// </summary>
        /// <param name="jointCollider">The hand joint collider</param>
        /// <param name="otherCollider">The other collider we collided with</param>
        protected virtual void OnInteractionColliderEnter(Collider jointCollider, Collider otherCollider)
        {
            //if the other collider we collided with is an interactable, we add it to the potential interactions
            if(otherCollider.TryGetComponent<IHandInteractable>(out var interactable))
            {
                //consider the case we never interacted with this interactable before
                if (!m_potentialInteractions.ContainsKey(interactable))
                {
                    m_potentialInteractions.Add(interactable, new CollidersInteractionData
                    {
                        CollidingColliders = new List<Collider>(),
                        LastCollisionMajorEventTime = Time.time,
                        InteractionState = InteractionState.NonInteracting
                    });
                }

                //add the current collider to the list of colliding colliders, and update the time if needed
                var potentialInteraction = m_potentialInteractions[interactable];

                if (!potentialInteraction.CollidingColliders.Contains(jointCollider)) //should never contain it, but just to be sure
                {
                    potentialInteraction.CollidingColliders.Add(jointCollider);

                    if (potentialInteraction.CollidingColliders.Count == InteractionColliders.Length)
                        potentialInteraction.LastCollisionMajorEventTime = Time.time;
                }
            }
        }

        /// <summary>
        /// Callback called when a collider we put on the hands joint exits an exiting collision with another collider.
        /// This is called both for trigger collisions and standard collisions.
        /// </summary>
        /// <param name="jointCollider">The hand joint collider</param>
        /// <param name="otherCollider">The other collider we stopped colliding with</param>
        private void OnInteractionColliderExit(Collider jointCollider, Collider otherCollider)
        {
            //if the other collider we collided with is an interactable, we remove it from the potential interactions
            if(otherCollider.TryGetComponent<IHandInteractable>(out var interactable))
            {
                //of course to remove it from the list of interactions, we must make sure it was there before
                if (m_potentialInteractions.ContainsKey(interactable))
                {
                    //remove the current collider from the list of colliding colliders and update the time if needed
                    var potentialInteraction = m_potentialInteractions[interactable];

                    if (potentialInteraction.CollidingColliders.Contains(jointCollider)) //should always contain it, but just to be sure
                    {
                        if (potentialInteraction.CollidingColliders.Count == InteractionColliders.Length) //it was colliding with all the joints, now not anymore, it is a major event
                            potentialInteraction.LastCollisionMajorEventTime = Time.time;

                        potentialInteraction.CollidingColliders.Remove(jointCollider);

                        //even if the list is empty, we don't remove the interactable from the potential interactions
                        //so the update cycle can detect if there has been a change of status while looping the list of
                        //interacting elements. The cleanup should so be done in the Update method
                        //(currently we do not have a cleanup because we assume this list of interactins never becomes
                        //too long, but it could be added if needed)
                    }
                }
            }
        }

        /// <summary>
        /// Evaluate the current collisions status and decide if we should start or end an interaction
        /// with an interactable
        /// </summary>
        private void EvaluateInteractions()
        {
            if (CurrentState == InteractionState.NonInteracting || CurrentState == InteractionState.InteractionPossible)
                EvaluateNonInteractingInteractions();
            else if (CurrentState == InteractionState.Interacting)
                EvaluateInteractingInteractions();
        }

        /// <summary>
        /// Evaluate the interactions when we are not already interacting with an interactable
        /// and so we are looking for one
        /// </summary>
        private void EvaluateNonInteractingInteractions()
        {
            bool foundInteractionsWithAllJoints = false;

            //for all the possible interactions
            foreach (var potentialInteraction in m_potentialInteractions)
            {
                //if the interactable can not interact with this interactor, we skip it
                if(!potentialInteraction.Key.CanBeInteractedBy(this))
                    continue;

                //if all the interactor's joint colliders are colliding with the interactable)
                if (potentialInteraction.Value.CollidingColliders.Count == InteractionColliders.Length)
                {
                    foundInteractionsWithAllJoints = true;

                    //if the collision is going on since enough time, we are interacting
                    if (Time.time - potentialInteraction.Value.LastCollisionMajorEventTime >= InteractionStartConfirmationTime)
                    {
                        //set the global state of the interactor as interacting
                        CurrentState = InteractionState.Interacting;

                        //set the state of the interactable as interacting and trigger all the necessary events
                        potentialInteraction.Value.InteractionState = InteractionState.Interacting;
                        CurrentInteractable = potentialInteraction.Key; //save also which is the current interactable that we are handling
                        CurrentInteractable.OnInteractionStarted(this);
                        InteractionStateChanged?.Invoke(CurrentInteractable, InteractionState.Interacting);
                        
                        //we have found our interactable, so it has no sense we still consider our other "possible" interactions, so cancel them all
                        foreach(var otherInteraction in m_potentialInteractions)
                            if (otherInteraction.Key != CurrentInteractable && otherInteraction.Value.InteractionState == InteractionState.InteractionPossible)
                            {
                                otherInteraction.Value.InteractionState = InteractionState.NonInteracting;
                                otherInteraction.Key.OnInteractionPossibleEnded(this);
                            }

                        //exit from the loop, we found our interactable
                        break;
                    }
                    //otherwise we are in the "possible" state, waiting for the confirmation of this collision lasting for enough time
                    else
                    {
                        //if the possible interaction was not possible before, set it as possible and trigger the necessary events
                        if (potentialInteraction.Value.InteractionState != InteractionState.InteractionPossible)
                        {
                            potentialInteraction.Value.InteractionState = InteractionState.InteractionPossible;
                            potentialInteraction.Key.OnInteractionPossibleStarted(this);
                        }

                        //if the global state of the interactor was not possible before, set it as possible and trigger the necessary events
                        if (CurrentState != InteractionState.InteractionPossible)
                        {
                            CurrentState = InteractionState.InteractionPossible;
                            InteractionStateChanged?.Invoke(potentialInteraction.Key, InteractionState.InteractionPossible);
                        }
                        //else, re-set the state as possible
                        else
                            CurrentState = InteractionState.InteractionPossible;
                    }
                }
                //else, it means we are not colliding with all the joints with this interactable. If the interaction was possible before, we should stop it
                else if (potentialInteraction.Value.InteractionState == InteractionState.InteractionPossible)
                {
                    potentialInteraction.Value.InteractionState = InteractionState.NonInteracting;
                    potentialInteraction.Key.OnInteractionPossibleEnded(this);
                }
            }

            //if we are not colliding with all the joints with any interactable, we have no possible interaction anymore
            if (!foundInteractionsWithAllJoints && CurrentState == InteractionState.InteractionPossible)
            {
                CurrentState = InteractionState.NonInteracting;
                InteractionStateChanged?.Invoke(null, InteractionState.NonInteracting);
            }
        }

        /// <summary>
        /// Evaluate the interactions when we are already interacting with an interactable
        /// and we have to check if the interaction is still going on
        /// </summary>
        private void EvaluateInteractingInteractions()
        {
            //we just need to check if we are still colliding with the current interactor
            if (m_potentialInteractions.ContainsKey(CurrentInteractable))
            {
                var currentInteraction = m_potentialInteractions[CurrentInteractable];

                //if the count of joint colliders colliding with the current interactable is not the same as the count of all the joint colliders,
                //it means we have stopped interacting with the interactable. If this happens for enough time, we have to abandon the interaction
                if (currentInteraction.CollidingColliders.Count < InteractionColliders.Length)
                {
                    //if the missed collision is going on since enough time, we are not interacting anymore
                    if (Time.time - currentInteraction.LastCollisionMajorEventTime >= InteractionEndConfirmationTime)
                    {
                        //set the global state of the interactor as non-interacting
                        CurrentState = InteractionState.NonInteracting;

                        //set the state of the interactable as non-interacting and trigger all the necessary events
                        currentInteraction.InteractionState = InteractionState.NonInteracting;
                        CurrentInteractable.OnInteractionEnded(this);
                        InteractionStateChanged?.Invoke(CurrentInteractable, InteractionState.NonInteracting);
                        CurrentInteractable = null;
                    }
                }
            }
        }
    }

}