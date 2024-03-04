using ProefExamen.Framework.StateMachine;

namespace ProefExamen.Framework.Buttons.StateMachine
{
    /// <summary>
    /// A class that represents a button that goes back to the previous state in the state machine.
    /// </summary>
    public class GoStateBackButton : BasicButton
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private protected override void OnButtonPressed() => StateMachine.Instance.GoBack();
    }
}