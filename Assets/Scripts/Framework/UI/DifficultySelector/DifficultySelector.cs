using System;
using UnityEngine;

using ProefExamen.Framework.Gameplay.Level;

namespace ProefExamen.Framework.UI.DifficultySelector
{
    public class DifficultySelector : MonoBehaviour
    {
        public Action<Difficulty> OnDifficultyChanged;

        private Difficulty _currentSelectedDifficulty = Difficulty.Normal;

        public void ChangeDifficulty(Difficulty newDifficulty)
        {
            _currentSelectedDifficulty = newDifficulty;
            OnDifficultyChanged?.Invoke(newDifficulty);
        }

        public Difficulty GetCurrentDifficulty() => _currentSelectedDifficulty;
    }
}

