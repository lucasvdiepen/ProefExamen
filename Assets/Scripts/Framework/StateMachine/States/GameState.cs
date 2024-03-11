using System.Collections;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.StateMachine.States
{
    public class GameState : MenuState
    {
        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            PerformanceTracker.Instance.OnScoreCompletion += UpdateStateOnScoreCompletion;
            LaneManager.Instance.PlayLevel();
        }

        public override IEnumerator OnStateExit()
        {
            PerformanceTracker.Instance.OnScoreCompletion -= UpdateStateOnScoreCompletion;
            yield return base.OnStateExit();
        }

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