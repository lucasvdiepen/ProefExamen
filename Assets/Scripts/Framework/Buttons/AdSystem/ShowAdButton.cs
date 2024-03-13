using ProefExamen.Framework.AdSystem;
using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.StateMachine.States;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    /// <summary>
    /// A class responsible for showing an ad when the button is pressed.
    /// </summary>
    public class ShowAdButton : BasicButton
    {
        private protected override void OnButtonPressed()
        {
            RewardedAdSystem.Instance.ShowAd((success) =>
            {
                if (!success)
                    return;

                Gameplay.LaneSystem.LaneManager.Instance.HasWatchedAd = true;
                PerformanceTracker.Instance.SetHealth(PerformanceTracker.Instance.MaxHealth / 2);

                Framework.StateMachine.StateMachine.Instance.GoToState<PauseState>();
            });
        }
    }
}
