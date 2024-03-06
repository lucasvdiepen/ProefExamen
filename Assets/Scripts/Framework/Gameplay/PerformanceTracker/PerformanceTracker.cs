using UnityEngine;
using System;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Utils;

namespace ProefExamen.Framework.Gameplay.PerformanceTracking
{
    /// <summary>
    /// A class responsible for tracking the performance of the player.
    /// </summary>
    public class PerformanceTracker : AbstractSingleton<PerformanceTracker>
    {
        [Header("The high scores")]
        [SerializeField]
        private ScoreResults scoreResults;

        [Header("The current performance")]
        [SerializeField]
        private PerformanceResult newResult;

        /// <summary>
        /// An action that shares the performance on a level.
        /// </summary>
        public Action<ScoreCompletionStatus> OnScoreCompletion;

        private void Start() => LaneManager.Instance.OnNoteHit += RegisterNewHit;

        /// <summary>
        /// Counts an extra hit onto the current result.
        /// </summary>
        /// <param name="hit">The new hit to count.</param>
        public void RegisterNewHit(HitStatus hit, int laneID) => newResult.CountHit(hit);

        /// <summary>
        /// Initiates a new PerformanceResult with the currentLevelID and current difficulty.
        /// </summary>
        public void StartNewLevelTracking() =>
            newResult = new PerformanceResult(SessionValues.Instance.currentLevelID, SessionValues.Instance.difficulty);

        /// <summary>
        /// Completes the currently tracking level and overwrites the old high score if the level score was beaten
        /// then shares the result of the level through an action.
        /// </summary>
        /// <param name="levelBeaten">If the level was beaten.</param>
        public void CompleteCurrentScore(bool levelBeaten = true)
        {
            ScoreCompletionStatus scoreCompletionStatus = levelBeaten 
                ? CheckCurrentScoreResult() 
                : ScoreCompletionStatus.Failed;

            OnScoreCompletion(scoreCompletionStatus);
        }

        private ScoreCompletionStatus CheckCurrentScoreResult()
        {
            int listLength = scoreResults.highScores.Count;

            newResult.totalScore = SessionValues.Instance.score;

            for (int i = 0; i < listLength; i++)
            {
                if (scoreResults.highScores[i].CompareLevels(newResult))
                {
                    if (scoreResults.highScores[i].totalScore >= newResult.totalScore)
                        return ScoreCompletionStatus.NotBeaten;

                    scoreResults.highScores[i] = newResult;
                    return ScoreCompletionStatus.Beaten;
                }
            }

            if (scoreResults == null)
            {
                Debug.LogError("No reference set for 'scoreResults' on PerformanceTracker!");

                return ScoreCompletionStatus.NotBeaten;
            }

            scoreResults.highScores.Add(newResult);
            return ScoreCompletionStatus.Beaten;
        }
    }
}
