using UnityEngine;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.UI.DifficultySelector
{
    public class DifficultySelector : MonoBehaviour
    {
        public void ChangeDifficulty(Difficulty newDifficulty)
        {
            SessionValues.Instance.difficulty = newDifficulty;
            MenuStateUpdater.Instance.OnDifficultyChanged?.Invoke(newDifficulty);
        }

        public Difficulty GetCurrentDifficulty() => SessionValues.Instance.difficulty;
    }
}

