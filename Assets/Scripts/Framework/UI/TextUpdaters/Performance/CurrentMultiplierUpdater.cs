using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.UI.TextUpdaters.Performance
{
    /// <summary>
    /// A TextUpdater class that is responsible for updating the Multiplier text.
    /// </summary>
    public class CurrentMultiplierUpdater : TextUpdater
    {
        private void OnEnable()
        {
            UpdateMultiplierText(PerformanceTracker.Instance.ScoreMultiplier);
            PerformanceTracker.Instance.OnMultiplierChanged += UpdateMultiplierText;
        }

        private void OnDisable() => PerformanceTracker.Instance.OnMultiplierChanged -= UpdateMultiplierText;

        private void UpdateMultiplierText(int multiplier) => ReplaceTag(multiplier.ToString());
    }
}