using System.Collections;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        private protected virtual void Awake()
        {
            gameObject.SetActive(false);

            RegisterState();
        }

        private protected virtual void OnDestroy() => UnregisterState();

        private protected virtual void RegisterState() => StateMachine.Instance.RegisterState(this);

        private protected virtual void UnregisterState() => StateMachine.Instance.UnregisterState(this);

        public virtual IEnumerator OnStateEnter()
        {
            gameObject.SetActive(true);
            yield return null;
        }

        public virtual IEnumerator OnStateExit()
        {
            gameObject.SetActive(false);
            yield return null;
        }
    }
}