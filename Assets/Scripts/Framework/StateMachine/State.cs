using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        private protected virtual void Awake() => RegisterState();

        private protected virtual void OnDestroy() => UnregisterState();

        private protected virtual void RegisterState() => StateMachine.RegisterState(this);

        private protected virtual void UnregisterState() => StateMachine.UnregisterState(this);

        public abstract void OnStateEnter();

        public abstract void OnStateExit();
    }
}