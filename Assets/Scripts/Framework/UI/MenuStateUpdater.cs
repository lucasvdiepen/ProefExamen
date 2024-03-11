using System;
using System.Collections;
using UnityEngine;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.UI
{
    public class MenuStateUpdater : AbstractSingleton<MenuStateUpdater>
    {
        public Action<Difficulty> OnDifficultyChanged;

        public Action<int> OnHighScoreChanged;

        public Action<int> OnSelectedLevelIDChanged;
    }
}
