using UnityEngine;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.UI.TextUpdaters
{
    public class HighScoreTextUpdater : TextUpdater
    {
        private void OnEnable() => MenuStateUpdater.Instance.OnDifficultyChanged += GetNewHighscore;

        private void OnDisable() => MenuStateUpdater.Instance.OnDifficultyChanged -= GetNewHighscore;
        private void GetNewHighscore(Difficulty newDifficulty)
        {
            int newHighscore = PerformanceTracker.Instance.GetHighScoreFromLevel();
            UpdateHighScore(newHighscore);
        }

        private void UpdateHighScore(int newHighscore) => ReplaceTag(newHighscore.ToString());
    }
}
