using ProefExamen.Framework.Gameplay.Level;
using System;
using UnityEngine;

namespace ProefExamen.Framework.UI.DifficultySelector
{
    public class DifficultySelector : MonoBehaviour
    {
        public Action<Difficulty> OnDifficultyChanged;

        private Difficulty _currentSelectedDifficulty = Difficulty.Normal;
    }
}

