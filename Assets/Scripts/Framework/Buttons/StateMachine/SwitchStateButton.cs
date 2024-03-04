using UnityEngine;

using ProefExamen.Framework.StateMachine;

namespace ProefExamen.Framework.Buttons.StateMachine
{
    /// <summary>
    /// A class that represents a button that switches to a specific state in the state machine.
    /// </summary>
    public class SwitchStateButton : BasicButton
    {
        /// <summary>
        /// The state to switch to.
        /// </summary>
        [SerializeField]
        private State _targetState;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private protected override void OnButtonPressed()
            => Framework.StateMachine.StateMachine.Instance.GoToState(_targetState);
    }
}