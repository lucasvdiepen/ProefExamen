using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.MainMenu
{
    /// <summary>
    /// A class that is responsible for handling the play button in the main menu.
    /// </summary>
    public class PlayButton : BasicButton
    {
        private protected override void OnButtonPressed()
            => Framework.StateMachine.StateMachine.Instance.GoToState<LevelSelectorState>();
    }
}