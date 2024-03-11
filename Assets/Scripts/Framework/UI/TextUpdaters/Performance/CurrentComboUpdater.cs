using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.UI.TextUpdaters.Performance
{
    /// <summary>
    /// A TextUpdater class that is responsible for updating the current combo.
    /// </summary>
    public class CurrentComboUpdater : TextUpdater
    {
        private void OnEnable()
        {
            UpdateComboText(PerformanceTracker.Instance.Combo);
            PerformanceTracker.Instance.OnComboChanged += UpdateComboText;
        }

        private void OnDisable() => PerformanceTracker.Instance.OnComboChanged -= UpdateComboText;

        private void UpdateComboText(int combo) => ReplaceTag(combo.ToString());
    }
}
