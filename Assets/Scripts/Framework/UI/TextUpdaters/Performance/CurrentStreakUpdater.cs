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
            UpdateComboText(PerformanceTracker.Instance.Streak);
            PerformanceTracker.Instance.OnStreakChanged += UpdateComboText;
        }

        private void OnDisable() => PerformanceTracker.Instance.OnStreakChanged -= UpdateComboText;

        private void UpdateComboText(int combo) => ReplaceTag(combo.ToString());
    }
}
