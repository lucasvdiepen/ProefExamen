using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.LevelSelector
{
    /// <summary>
    /// A class responsible for handling the button press of the back to main menu button.
    /// </summary>
    public class BackToMainMenuButton : BasicButton
    {
        private protected override void OnButtonPressed()
            => Framework.StateMachine.StateMachine.Instance.GoToState<MainMenuState>();
    }
}