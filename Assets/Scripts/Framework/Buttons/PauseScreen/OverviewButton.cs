using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.PauseScreen
{
    /// <summary>
    /// A class responsible for handling the overview button.
    /// </summary>
    public class OverviewButton : BasicButton
    {
        private protected override void OnButtonPressed()
            => Framework.StateMachine.StateMachine.Instance.GoToState<LevelSelectorState>();
    }
}