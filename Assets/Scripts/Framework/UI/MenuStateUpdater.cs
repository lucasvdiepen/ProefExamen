using System;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Utils;

namespace ProefExamen.Framework.UI
{

    /// <summary>
    /// State responsible for updating menu state.
    /// </summary>
    public class MenuStateUpdater : AbstractSingleton<MenuStateUpdater>
    {
        /// <summary>
        /// Event invoked when difficulty is changed.
        /// </summary>
        public Action<Difficulty> OnDifficultyChanged;

        /// <summary>
        /// Event invoked when high score is changed.
        /// </summary>
        public Action<int> OnHighScoreChanged;

        /// <summary>
        /// Event invoked when selected level ID is changed
        /// </summary>
        public Action<int> OnSelectedLevelIDChanged;
    }
}
