using System;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Utils;

namespace ProefExamen.Framework.UI
{
    /// <summary>
    /// A class that holds actions that can be called and listened to from different classes that
    /// need UI updates.
    /// </summary>
    public class MenuStateUpdater : AbstractSingleton<MenuStateUpdater>
    {
        /// <summary>
        /// An action that broadcasts a change in difficulty.
        /// </summary>
        public Action<Difficulty> OnDifficultyChanged;

        /// <summary>
        /// An action that broadcasts a change in the highscore.
        /// </summary>
        public Action<int> OnHighScoreChanged;

        /// <summary>
        /// An action that broadcasts a change in the SelectedLevelID.
        /// </summary>
        public Action<int> OnSelectedLevelIDChanged;
    }
}
