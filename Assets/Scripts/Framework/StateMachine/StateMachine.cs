using ProefExamen.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    public class StateMachine : AbstractSingleton<StateMachine>
    {
        [SerializeField]
        private int _maxHistorySize = 10;

        public Action<State> OnStateChanged;

        public State CurrentState { get; private set; }

        public State TargetState { get; private set; }

        public State PreviousState
        {
            get
            {
                if(_navigationHistory.Count < 2)
                    return null;

                if (!TryGetState(_navigationHistory[^2], out State state))
                    return null;

                return state;
            }
        }

        public bool IsCurrentState<T>() where T : State
            => CurrentState.GetType() == typeof(T);

        public bool IsCurrentState(State state)
            => CurrentState.GetType() == state.GetType();

        private readonly Dictionary<Type, State> _states = new();
        private readonly List<Type> _navigationHistory = new();

        public void GoToState<T>(bool addToHistory = true) where T : State
            => GoToState(typeof(T), addToHistory);

        public void GoToState(State state, bool addToHistory = true)
            => GoToState(state.GetType(), addToHistory);

        public void GoToState(Type state, bool addToHistory = true)
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

            StartCoroutine(TransitionToState(targetState, addToHistory));
        }

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

        public void ClearHistory()
        {
            _navigationHistory.Clear();
            AddToHistory(CurrentState.GetType());
        }

        private void AddToHistory(Type state)
        {
            if (_navigationHistory.Count >= _maxHistorySize)
                _navigationHistory.RemoveAt(0);

            _navigationHistory.Add(state);
        }

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

        public void UnregisterState(State state)
        {
            if (!IsStateRegistered(state))
            {
                Debug.LogError($"State of type {state.GetType()} is not registered");
                return;
            }

            _states.Remove(state.GetType());
        }

        private IEnumerator TransitionToState(State state, bool addToHistory = true)
        {
            TargetState = state;

            if(CurrentState != null)
                yield return CurrentState.OnStateExit();

            CurrentState = TargetState;

            yield return CurrentState.OnStateEnter();

            if(addToHistory)
                AddToHistory(CurrentState.GetType());

            OnStateChanged?.Invoke(CurrentState);
        }

        private bool IsStateRegistered<T>() where T : State
            => _states.ContainsKey(typeof(T));

        private bool IsStateRegistered(State state)
            => _states.ContainsKey(state.GetType());

        private bool TryGetState<T>(out State state) where T : State
            => TryGetState(typeof(T), out state);

        private bool TryGetState(Type type, out State state)
            => _states.TryGetValue(type, out state);
    }
}
