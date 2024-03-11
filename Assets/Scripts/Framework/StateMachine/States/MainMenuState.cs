using System.Collections;

using ProefExamen.Framework.StateMachine.Attributes;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// A class that represents the main menu state in the state machine.
    /// </summary>
    [DefaultState]
    public class MainMenuState : MenuState
    {
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