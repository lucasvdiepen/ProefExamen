using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.PlayLevel
{
    /// <summary>
    /// Class responsible for handling logic behind play level button
    /// </summary>
    public class PlayLevelButton : BasicButton
    {
        private protected override void OnButtonPressed()
            => Framework.StateMachine.StateMachine.Instance.GoToState<GameState>();
    }
}