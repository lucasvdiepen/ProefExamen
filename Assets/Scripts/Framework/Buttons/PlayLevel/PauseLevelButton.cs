using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.PlayLevel
{
    /// <summary>
    /// A BasicButton class responsible for toggling the games paused state.
    /// </summary>
    public class PauseLevelButton : BasicButton
    {
        private protected override void OnButtonPressed()
        {
            if(SessionValues.Instance.paused)
                Framework.StateMachine.StateMachine.Instance.GoToState<GameState>();
            else
                Framework.StateMachine.StateMachine.Instance.GoToState<PauseState>();

            Gameplay.LaneSystem.LaneManager.Instance.SetPaused(!SessionValues.Instance.paused);
        }
    }
}