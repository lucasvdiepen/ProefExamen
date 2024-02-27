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
        public static Action<State> OnStateChanged;

        public static State CurrentState { get; private set; }

        public static State TargetState { get; private set; }

        private readonly static Dictionary<Type, State> _states = new();

        public static void ChangeState<T>() where T : State
        {
            if(CurrentState != null && CurrentState.GetType() == typeof(T))
            {
                Debug.LogError($"State of type {typeof(T)} is already active");
                return;
            }

            if (!TryGetState<T>(out State targetState))
            {
                Debug.LogError($"State of type {typeof(T)} is not registered");
                return;
            }

            TransitionToState(targetState);
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

        private static void TransitionToState(State state)
        {
            TargetState = state;

            if(CurrentState != null)
                CurrentState.OnStateExit();

            CurrentState = TargetState;

            CurrentState.OnStateEnter();

            OnStateChanged?.Invoke(CurrentState);
        }

        public static bool IsCurrentState<T>() where T : State => CurrentState.GetType() == typeof(T);

        public static bool IsCurrentState(State state) => CurrentState.GetType() == state.GetType();

        private static bool IsStateRegistered<T>() where T : State => _states.ContainsKey(typeof(T));

        private static bool IsStateRegistered(State state) => _states.ContainsKey(state.GetType());

        private static bool TryGetState<T>(out State state) where T : State => _states.TryGetValue(typeof(T), out state);
    }
}
