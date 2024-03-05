using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProefExamen.Framework.Utils;
using System.Linq;

namespace ProefExamen.Framework.StateMachine
{
    /// <summary>
    /// A class responsible for managing all the states.
    /// </summary>
    public class StateMachine : AbstractSingleton<StateMachine>
    {
        /// <summary>
        /// The maximum size of the navigation history.
        /// </summary>
        [SerializeField]
        private int _maxHistorySize = 10;

        /// <summary>
        /// The registered states.
        /// </summary>
        private readonly Dictionary<Type, State> _states = new();

        /// <summary>
        /// The navigation history.
        /// </summary>
        private readonly List<Type> _navigationHistory = new();

        /// <summary>
        /// Invoked when the state has changed.
        /// </summary>
        public Action<State> OnStateChanged;

        /// <summary>
        /// The current state.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// The target state.
        /// </summary>
        public State TargetState { get; private set; }

        /// <summary>
        /// The previous state.
        /// </summary>
        public State PreviousState
        {
            get
            {
                if(_navigationHistory.Count < 2)
                    return null;

                // Tries to get the second to last state in the navigation history.
                if (!TryGetState(_navigationHistory[^2], out State state))
                    return null;

                return state;
            }
        }

        /// <summary>
        /// Checks if the given state is the current state.
        /// </summary>
        /// <typeparam name="T">The type of the state to check.</typeparam>
        /// <returns>True if the given state is the current state, otherwise false.</returns>
        public bool IsCurrentState<T>() where T : State => CurrentState.GetType() == typeof(T);

        /// <summary>
        /// Checks if the given state is the current state.
        /// </summary>
        /// <param name="state">The type of the state to check.</param>
        /// <returns>True if the given state is the current state, otherwise false.</returns>
        public bool IsCurrentState(State state)
            => CurrentState.GetType() == state.GetType();

        /// <summary>
        /// Goes to the given state.
        /// </summary>
        /// <typeparam name="T">The target state.</typeparam>
        /// <param name="shouldAddToHistory">Whether to add the state to the navigation history.</param>
        public void GoToState<T>(bool shouldAddToHistory = true) where T : State
            => GoToState(typeof(T), shouldAddToHistory);

        /// <summary>
        /// Goes to the given state.
        /// </summary>
        /// <param name="state">The target state.</param>
        /// <param name="shouldAddToHistory">Whether to add the state to the navigation history.</param>
        public void GoToState(State state, bool shouldAddToHistory = true)
            => GoToState(state.GetType(), shouldAddToHistory);

        /// <summary>
        /// Goes to the given state.
        /// </summary>
        /// <param name="state">The target state</param>
        /// <param name="shouldAddToHistory">Whether to add the state to the navigation history.</param>
        public void GoToState(Type state, bool shouldAddToHistory = true)
        {
            if (CurrentState != null && CurrentState.GetType() == state)
            {
                Debug.LogError($"State of type {state} is already active");
                return;
            }

            if (!TryGetState(state, out State targetState))
            {
                Debug.LogError($"State of type {state} is not registered");
                return;
            }

            StartCoroutine(TransitionToState(targetState, shouldAddToHistory));
        }

        /// <summary>
        /// Goes back to the previous state.
        /// </summary>
        public void GoBack()
        {
            if (_navigationHistory.Count < 2)
            {
                Debug.LogError("No previous state to go back to");
                return;
            }

            GoToState(_navigationHistory[^2], false);

            _navigationHistory.RemoveAt(_navigationHistory.Count - 1);
        }

        /// <summary>
        /// Clears the navigation history.
        /// </summary>
        public void ClearHistory()
        {
            _navigationHistory.Clear();
            AddToHistory(CurrentState.GetType());
        }

        /// <summary>
        /// Adds the given state to the navigation history.
        /// </summary>
        /// <param name="state">The state to add to the navigation history.</param>
        private void AddToHistory(Type state)
        {
            if (_navigationHistory.Count >= _maxHistorySize)
                _navigationHistory.RemoveAt(0);

            _navigationHistory.Add(state);
        }

        /// <summary>
        /// Registers the given state.
        /// </summary>
        /// <param name="state">The state to register.</param>
        /// <param name="isDefault">Whether the given state should be the default state.</param>
        public void RegisterState(State state, bool isDefault = false)
        {
            if (IsStateRegistered(state))
            {
                Debug.LogError($"State of type {state.GetType()} is already registered");
                return;
            }
            
            _states.Add(state.GetType(), state);

            if (!isDefault)
                return;

            if(CurrentState != null)
            {
                Debug.LogError($"Default state is already set to {CurrentState.GetType()}");
                return;
            }

            StartCoroutine(TransitionToState(state));
        }

        /// <summary>
        /// Unregisters the given state.
        /// </summary>
        /// <param name="state">The state to unregister.</param>
        public void UnregisterState(State state)
        {
            if (!IsStateRegistered(state))
            {
                Debug.LogError($"State of type {state.GetType()} is not registered");
                return;
            }

            _states.Remove(state.GetType());
        }

        /// <summary>
        /// Transitions to the given state.
        /// </summary>
        /// <param name="state">The target state.</param>
        /// <param name="shouldAddToHistory">Whether to add the state to the navigation history.</param>
        private IEnumerator TransitionToState(State state, bool shouldAddToHistory = true)
        {
            TargetState = state;

            List<State> targetStateTree = GetStateTree(TargetState);

            if (CurrentState != null)
            {
                List<State> currentStateTree = GetStateTree(CurrentState);

                IEnumerable<State> statesToExit = currentStateTree.Except(targetStateTree);

                yield return CurrentState.OnStateExit();
            }

            CurrentState = TargetState;

            yield return CurrentState.OnStateEnter();

            if(shouldAddToHistory)
                AddToHistory(CurrentState.GetType());

            OnStateChanged?.Invoke(CurrentState);
        }

        private List<State> GetStateTree(State state)
        {
            List<State> tree = new();

            State currentState = state;

            while(true)
            {
                ParentStateAttribute parentStateAttribute = (ParentStateAttribute)Attribute.GetCustomAttribute(currentState.GetType(), typeof(ParentStateAttribute));
                if (parentStateAttribute == null)
                    break;

                if (parentStateAttribute.ParentState == null || TryGetState(parentStateAttribute.ParentState, out State parentState))
                    break;
                
                tree.Add(parentState);
                currentState = parentState;
            }

            tree.Add(state);
            
            return tree;
        }

        /// <summary>
        /// Checks if the given state is registered.
        /// </summary>
        /// <typeparam name="T">The type of the state to check.</typeparam>
        /// <returns>True if the given state is registered, otherwise false.</returns>
        private bool IsStateRegistered<T>() where T : State
            => _states.ContainsKey(typeof(T));

        /// <summary>
        /// Checks if the given state is registered.
        /// </summary>
        /// <param name="state">The type of the state to check.</param>
        /// <returns>True if the given state is registered, otherwise false.</returns>
        private bool IsStateRegistered(State state)
            => _states.ContainsKey(state.GetType());

        /// <summary>
        /// Tries to get the state of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the state to get.</typeparam>
        /// <param name="state">The output state.</param>
        /// <returns>True if the state was found, otherwise false.</returns>
        private bool TryGetState<T>(out State state) where T : State
            => TryGetState(typeof(T), out state);

        /// <summary>
        /// Tries to get the state of the given type.
        /// </summary>
        /// <param name="type">The type of the state to get.</param>
        /// <param name="state">The output state.</param>
        /// <returns>True if the state was found, otherwise false.</returns>
        private bool TryGetState(Type type, out State state)
            => _states.TryGetValue(type, out state);
    }
}
