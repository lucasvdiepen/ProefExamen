using System.Collections;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.Gameplay.Values;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for game.
    /// </summary>
    public class GameState : MenuState
    {
        [SerializeField]
        private GameObject _lineVisualizer;

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

            LaneManager.Instance.DestroyAllNotes();

            ExitLevel();

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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerator OnStateExitedToChild()
        {
            ExitLevel();

            yield return base.OnStateExitedToChild();
        }

        private void PlayLevel()
        {
            _lineVisualizer.SetActive(true);

            if (StateMachine.Instance.PreviousState.GetType() == typeof(PauseState))
                return;

            LaneManager.Instance.PlayLevel();
        }

        private void ExitLevel() => _lineVisualizer.SetActive(false);

        private void UpdateStateOnScoreCompletion(ScoreCompletionStatus status)
        {
            LaneManager.Instance.SetPaused(true);
            LaneManager.Instance.DestroyAllNotes();

            switch (status)
            {
                case ScoreCompletionStatus.Failed:
                    if(LaneManager.Instance.HasWatchedAd)
                    {
                        StateMachine.Instance.GoToState<LoseState>();
                        break;
                    }

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