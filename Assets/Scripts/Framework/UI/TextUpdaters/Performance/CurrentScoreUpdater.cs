using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.UI.TextUpdaters.Performance
{
    /// <summary>
    /// A TextUpdater class responsible for updating the currentScore.
    /// </summary>
    public class CurrentScoreUpdater : TextUpdater
    {
        private void OnEnable()
        {
            UpdateScoreText(PerformanceTracker.Instance.Score);
            PerformanceTracker.Instance.OnPointsChanged += UpdateScoreText;
        }

        private void OnDisable() => PerformanceTracker.Instance.OnPointsChanged -= UpdateScoreText;

        private void UpdateScoreText(int newScore) => ReplaceTag(newScore.ToString());
    }
}
