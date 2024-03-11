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
        public Action<Difficulty> OnDifficultyChanged;

        public Action<int> OnHighScoreChanged;

        public Action<int> OnSelectedLevelIDChanged;
    }
}
