using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.StateMachine.States;
using ProefExamen.Framework.UI;

namespace ProefExamen.Framework.Buttons.LevelSelector
{
    /// <summary>
    /// A class responsible for handling the button that selects a level.
    /// </summary>
    public class SelectLevelButton : BasicButton
    {
        /// <summary>
        /// The levelID of the level to select.
        /// </summary>
        public int levelID;

        private protected override void OnButtonPressed()
        {
            SessionValues.Instance.SelectLevel(levelID);
            MenuStateUpdater.Instance.OnSelectedLevelIDChanged?.Invoke(levelID);
            Framework.StateMachine.StateMachine.Instance.GoToState<PlayLevelState>();
        }
    }
}