using System.Collections;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine
{
    /// <summary>
    /// A class that represents a state in the state machine.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// Deactive the game object and registers the state.
        /// </summary>
        private protected virtual void Awake()
        {
            gameObject.SetActive(false);

            RegisterState();
        }

        /// <summary>
        /// Unregisters the state.
        /// </summary>
        private protected virtual void OnDestroy() => UnregisterState();

        /// <summary>
        /// Registers the state to the state machine.
        /// </summary>
        private protected virtual void RegisterState() => StateMachine.Instance.RegisterState(this);

        /// <summary>
        /// Unregisters the state from the state machine.
        /// </summary>
        private protected virtual void UnregisterState() => StateMachine.Instance.UnregisterState(this);

        /// <summary>
        /// Method that gets called when the state is entered.
        /// </summary>
        public virtual IEnumerator OnStateEnter()
        {
            gameObject.SetActive(true);
            yield return null;
        }

        /// <summary>
        /// Method that gets called when the state is exited.
        /// </summary>
        public virtual IEnumerator OnStateExit()
        {
            gameObject.SetActive(false);
            yield return null;
        }
    }
}