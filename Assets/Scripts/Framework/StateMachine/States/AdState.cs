using System.Collections;

using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.StateMachine.Attributes;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for ads.
    /// </summary>
    [ParentState(typeof(GameState))]
    public class AdState : MenuState
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateExit()
        {
            if (!StateMachine.Instance.IsTargetState<PauseState>())
                PerformanceTracker.Instance.CompleteTracking(false);

            yield return base.OnStateExit();
        }
    }
}