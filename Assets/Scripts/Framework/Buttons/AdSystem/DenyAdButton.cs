using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    /// <summary>
    /// A button class that is responsible for exiting from the Ad menu.
    /// </summary>
    public class DenyAdButton : BasicButton
    {
        private protected override void OnButtonPressed() =>
            Framework.StateMachine.StateMachine.Instance.GoToState<LoseState>();
    }
}
