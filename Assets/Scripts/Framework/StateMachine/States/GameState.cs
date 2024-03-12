using System.Collections;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for game.
    /// </summary>
    public class GameState : MenuState
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            PerformanceTracker.Instance.OnScoreCompletion += UpdateStateOnScoreCompletion;

            PlayLevel();
        }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateExit()
        {
            PerformanceTracker.Instance.OnScoreCompletion -= UpdateStateOnScoreCompletion;

            yield return base.OnStateExit();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateEnteredFromChild()
        {
            yield return base.OnStateEnteredFromChild();

            PlayLevel();
        }

        private void PlayLevel() => LaneManager.Instance.PlayLevel();

        private void UpdateStateOnScoreCompletion(ScoreCompletionStatus status)
        {
            LaneManager.Instance.SetPaused(true);
            LaneManager.Instance.DestroyAllNotes();

            switch (status)
            {
                case ScoreCompletionStatus.Failed:
                    StateMachine.Instance.GoToState<AdState>();
                    break;
                case ScoreCompletionStatus.NotBeaten:
                    StateMachine.Instance.GoToState<WinState>();
                    break;
                case ScoreCompletionStatus.Beaten:
                    StateMachine.Instance.GoToState<WinState>();
                    break;
            }
        }
    }
}