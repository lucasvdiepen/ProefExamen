using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.UI.TextUpdaters.Performance
{
    /// <summary>
    /// A TextUpdater class that is responsible for updating the current streak.
    /// </summary>
    public class CurrentStreakUpdater : TextUpdater
    {
        private void OnEnable()
        {
            UpdateStreakText(PerformanceTracker.Instance.Streak);
            PerformanceTracker.Instance.OnStreakChanged += UpdateStreakText;
        }

        private void OnDisable() => PerformanceTracker.Instance.OnStreakChanged -= UpdateStreakText;

        private void UpdateStreakText(int combo) => ReplaceTag(combo.ToString());
    }
}
