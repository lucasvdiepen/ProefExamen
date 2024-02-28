using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    public static class StateMachine
    {
        private const int MAX_HISTORY_SIZE = 10;

        public static Action<State> OnStateChanged;

        public static State CurrentState { get; private set; }

        public static State TargetState { get; private set; }

        public static State PreviousState
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

        private readonly static Dictionary<Type, State> _states = new();
        private readonly static List<Type> _navigationHistory = new();

        public static void GoToState<T>(bool addToHistory = true) where T : State
            => GoToState(typeof(T), addToHistory);

        public static void GoToState(State state, bool addToHistory = true)
            => GoToState(state.GetType(), addToHistory);

        public static void GoToState(Type state, bool addToHistory = true)
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

            TransitionToState(targetState, addToHistory);
        }

        public static void GoBack()
        {
            if (_navigationHistory.Count < 2)
            {
                Debug.LogError("No previous state to go back to");
                return;
            }

            GoToState(_navigationHistory[^2], false);

            _navigationHistory.RemoveAt(_navigationHistory.Count - 1);
        }

        public static void ClearHistory()
        {
            _navigationHistory.Clear();
            AddToHistory(CurrentState.GetType());
        }

        private static void AddToHistory(Type state)
        {
            if (_navigationHistory.Count >= MAX_HISTORY_SIZE)
                _navigationHistory.RemoveAt(0);

            _navigationHistory.Add(state);
        }

        public static void RegisterState(State state, bool isDefault = false)
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

            TransitionToState(state);
        }

        public static void UnregisterState(State state)
        {
            if (!IsStateRegistered(state))
            {
                Debug.LogError($"State of type {state.GetType()} is not registered");
                return;
            }

            _states.Remove(state.GetType());
        }

        private static void TransitionToState(State state, bool addToHistory = true)
        {
            TargetState = state;

            if(CurrentState != null)
                CurrentState.OnStateExit();

            CurrentState = TargetState;

            CurrentState.OnStateEnter();

            if(addToHistory)
                AddToHistory(CurrentState.GetType());

            OnStateChanged?.Invoke(CurrentState);
        }

        public static bool IsCurrentState<T>() where T : State
            => CurrentState.GetType() == typeof(T);

        public static bool IsCurrentState(State state)
            => CurrentState.GetType() == state.GetType();

        private static bool IsStateRegistered<T>() where T : State
            => _states.ContainsKey(typeof(T));

        private static bool IsStateRegistered(State state)
            => _states.ContainsKey(state.GetType());

        private static bool TryGetState<T>(out State state) where T : State
            => TryGetState(typeof(T), out state);

        private static bool TryGetState(Type type, out State state)
            => _states.TryGetValue(type, out state);
    }
}
