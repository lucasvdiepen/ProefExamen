using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.UI.TextUpdaters.Performance
{
    /// <summary>
    /// A TextUpdater class that is responsible for updating the HighScoreText.
    /// </summary>
    public class HighScoreTextUpdater : TextUpdater
    {
        private void OnEnable()
        {
            GetNewHighscore();
            MenuStateUpdater.Instance.OnDifficultyChanged += GetNewHighscore;
        }

        private void OnDisable() => MenuStateUpdater.Instance.OnDifficultyChanged -= GetNewHighscore;

        private void GetNewHighscore() => GetNewHighscore(SessionValues.Instance.difficulty);

        private void GetNewHighscore(Difficulty newDifficulty)
        {
            int newHighscore = PerformanceTracker.Instance.GetHighScoreFromLevel();
            UpdateHighScore(newHighscore);
        }

        private void UpdateHighScore(int newHighscore) => ReplaceTag(newHighscore.ToString());
    }
}
