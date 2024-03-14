using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.Sharing;

namespace ProefExamen.Framework.Buttons.Share
{
    /// <summary>
    /// A class responsible for sharing the highscore of the player when the button is pressed.
    /// </summary>
    public class ShareButton : BasicButton
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private protected override void OnButtonPressed()
        {
            ShareScoreUpdater.Instance.SetTargetResult(PerformanceTracker.Instance.CurrentPerformanceResult);
            ShareHighscore.Instance.Share();
        }
    }
}