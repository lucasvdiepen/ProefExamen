using System;
using System.Collections.Generic;
using UnityEngine;

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
        [Header("The current performance")]
        [SerializeField]
        private PerformanceResult _newResult;

        /// <summary>
        /// A list containing HighScores for levels.
        /// </summary>
        private List<PerformanceResult> _highscores = null;

        /// <summary>
        /// An action that shares the performance on a level.
        /// </summary>
        public Action<ScoreCompletionStatus> OnScoreCompletion;

        private void Awake() => LoadData();

        private void Start() => LaneManager.Instance.OnNoteHit += RegisterNewHit;

        private void LoadData()
        {
            string jsonData = PlayerPrefs.GetString("highscores");

            if (!string.IsNullOrEmpty(jsonData))
            {
                PerformanceTrackerData performanceTrackerData =
                JsonUtility.FromJson<PerformanceTrackerData>(jsonData);

                if (performanceTrackerData.highscores != null)
                {
                    _highscores = performanceTrackerData.highscores;
                    return;
                }
            }

            _highscores = new List<PerformanceResult>();
        }

        /// <summary>
        /// Counts an extra hit onto the current result.
        /// </summary>
        /// <param name="hit">The new hit to count.</param>
        public void RegisterNewHit(HitStatus hit, int laneID) => _newResult.AddHit(hit);

        /// <summary>
        /// Initiates a new PerformanceResult with the currentLevelID and current difficulty.
        /// </summary>
        public void StartTracking()
        {
            _newResult = new PerformanceResult(
                SessionValues.Instance.currentLevelID,
                SessionValues.Instance.difficulty
            );
        }

        /// <summary>
        /// Completes the currently tracking level and overwrites the old high score if the level score was beaten
        /// then shares the result of the level through an action.
        /// </summary>
        /// <param name="levelBeaten">If the level was beaten.</param>
        public void CompleteTracking(bool levelBeaten = true)
        {
            if(_newResult.levelID != SessionValues.Instance.currentLevelID)
            {
                Debug.LogError("Current level is not equal to the _newResult level! Check PerformanceTracker!");
                return;
            }

            ScoreCompletionStatus scoreCompletionStatus = levelBeaten
                ? ProcessNewScoreResult()
                : ScoreCompletionStatus.Failed;

            SaveData();

            OnScoreCompletion?.Invoke(scoreCompletionStatus);
        }

        private ScoreCompletionStatus ProcessNewScoreResult()
        {
            if (_highscores == null)
            {
                Debug.LogError("No reference set for 'highscores' on PerformanceTracker!");

                return ScoreCompletionStatus.NotBeaten;
            }

            int listLength = _highscores.Count;

            _newResult.totalScore = SessionValues.Instance.score;

            for (int i = 0; i < listLength; i++)
            {
                if (!_highscores[i].CompareLevels(_newResult))
                    continue;

                if (_highscores[i].totalScore >= _newResult.totalScore)
                    return ScoreCompletionStatus.NotBeaten;

                _highscores[i] = _newResult;
                return ScoreCompletionStatus.Beaten;
            }

            _highscores.Add(_newResult);
            return ScoreCompletionStatus.Beaten;
        }

        private void SaveData()
        {
            string data = JsonUtility.ToJson(new PerformanceTrackerData { highscores = _highscores});
            PlayerPrefs.SetString("highscores", data);
        }
    }
}
