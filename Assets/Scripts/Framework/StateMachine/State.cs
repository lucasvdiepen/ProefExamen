using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        private protected virtual void Awake() => RegisterState();

        private protected virtual void OnDestroy() => UnregisterState();

        private protected virtual void RegisterState() => StateMachine.Instance.RegisterState(this);

        private protected virtual void UnregisterState() => StateMachine.Instance.UnregisterState(this);

        public abstract IEnumerator OnStateEnter();

        public abstract IEnumerator OnStateExit();
    }
}