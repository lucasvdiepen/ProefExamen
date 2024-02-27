using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        private protected virtual void Awake()
        {
            StateMachine.RegisterState(this);
        }

        private protected virtual void OnDestroy()
        {
            StateMachine.UnregisterState(this);
        }

        public abstract void OnStateEnter();

        public abstract void OnStateExit();
    }
}