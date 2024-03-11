using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.PlayLevel
{
    public class PlayLevelButton : BasicButton
    {
        private protected override void OnButtonPressed()
            => Framework.StateMachine.StateMachine.Instance.GoToState<GameState>();
    }
}