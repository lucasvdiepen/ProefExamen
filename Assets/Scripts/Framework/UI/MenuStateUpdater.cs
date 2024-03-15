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
        /// Event invoked when the difficulty is changed.
        /// </summary>
        public Action<Difficulty> OnDifficultyChanged;

        /// <summary>
        /// Event invoked when the highscore is changed.
        /// </summary>
        public Action<int> OnHighScoreChanged;

        /// <summary>
        /// Event invoked when the SelectedLevelID is changed.
        /// </summary>
        public Action<int> OnSelectedLevelIDChanged;
    }
}
