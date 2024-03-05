using System.Collections;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// A class that represents the main menu state in the state machine.
    /// </summary>
    public class MainMenuState : MenuState
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private protected override void RegisterState() => StateMachine.Instance.RegisterState(this, true);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateEnter() => base.OnStateEnter();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateExit() => base.OnStateExit();
    }
}