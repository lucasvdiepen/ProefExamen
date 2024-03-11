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
        [Header("Player's performance")]
        [SerializeField]
        private PerformanceResult _newResult;

        [SerializeField]
        private ScoreCompletionStatus _lastCompletionStatus;

        [SerializeField]
        private int _score = 0;

        [Header("Score multipliers and streaks")]
        [SerializeField]
        private int _scoreMultiplier = 1;

        [SerializeField]
        private int _maxMultiplier = 8;

        [SerializeField]
        private int _multiplierStreak = 0;

        [SerializeField]
        private int _totalStreak = 0;

        [Header("Health")]
        [SerializeField]
        private float _maxHealth = 1000f;

        [SerializeField]
        private float _health;

        [SerializeField]
        private float _healthLossMultiplier = 3;

        [SerializeField]
        private float _healthGainMultiplier = 1;

        [Header("Highscores object")]
        [SerializeField]
        private List<PerformanceResult> _highscores = null;

        /// <summary>
        /// Getter for max health stat.
        /// </summary>
        public float MaxHealth => _maxHealth;

        /// <summary>
        /// An action that broadcasts the new amount of points when they change.
        /// </summary>
        public Action<int> OnPointsChanged;

        /// <summary>
        /// An action that broadcasts the change of the current multiplier.
        /// </summary>
        public Action<int> OnMultiplierChanged;

        /// <summary>
        /// An action that broadcasts the change of the current streak.
        /// </summary>
        public Action<int> OnStreakChanged;

        /// <summary>
        /// An action that broadcasts the performance on a level.
        /// </summary>
        public Action<ScoreCompletionStatus> OnScoreCompletion;

        /// <summary>
        /// An action that broadcasts the new health value when its changed.
        /// </summary>
        public Action<float> OnHealthChanged;

        /// <summary>
        /// The last ScoreCompletionStatus that was achieved.
        /// </summary>
        public ScoreCompletionStatus LastCompletionStatus => _lastCompletionStatus;

        /// <summary>
        /// A getter that retrieves the current score.
        /// </summary>
        public int Score => _score;

        /// <summary>
        /// A getter that retrieves the current Streak.
        /// </summary>
        public int Streak => _totalStreak;

        /// <summary>
        /// A getter that retrieves the current score multiplier.
        /// </summary>
        public int ScoreMultiplier => _scoreMultiplier;

        private void Awake() => LoadData();

        private void Start() => LaneManager.Instance.OnNoteHit += ProcessNewHit;

        /// <summary>
        /// Gets the highscore from the curretnly selected level.
        /// </summary>
        /// <returns>The highscore of this level, is 0 when there is no highscore set.</returns>
        public PerformanceResult GetHighScoreFromLevel() => GetHighScoreFromLevel(SessionValues.Instance.currentLevelID);

        /// <summary>
        /// Gets the highscore from the passed level ID.
        /// </summary>
        /// <param name="levelID">The level ID to get the highscore from.</param>
        /// <returns>The highscore of the passed level ID, will return 0 if no highscore is set.</returns>
        public PerformanceResult GetHighScoreFromLevel(int levelID)
        {
            int listLength = _highscores.Count;

            for(int i = 0; i < listLength; i++)
            {
                if (_highscores[i].levelID == SessionValues.Instance.currentLevelID &&
                    _highscores[i].difficulty == SessionValues.Instance.difficulty)
                {
                    return _highscores[i];
                }
            }

            return new PerformanceResult
            (
                levelID, 
                SessionValues.Instance.difficulty
            );
        }

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
        public void ProcessNewHit(HitStatus hit, int laneID)
        {
            bool isComboBroken = UpdateStreak(hit);

            int pointsToAdd = (int)hit * _scoreMultiplier;

            _score += pointsToAdd;

            float chosenMultiplier = isComboBroken
                ? _healthLossMultiplier
                : _healthGainMultiplier;

            float healthToAdd = pointsToAdd * chosenMultiplier;
            if(_health < _maxHealth || isComboBroken)
            {
                _health = Math.Clamp(_health + healthToAdd, 0, _maxHealth);
                OnHealthChanged?.Invoke(_health);
            }

            _newResult.AddHit(hit);
            OnPointsChanged?.Invoke(_score);
        }

        /// <summary>
        /// Updates the streak and combo with a newly passed hit.
        /// </summary>
        /// <param name="hit">The hit to update the streak with.</param>
        /// <returns>Returns true if the combo is broken, otherwise false</returns>
        private bool UpdateStreak(HitStatus hit)
        {
            if(hit == HitStatus.Miss || hit == HitStatus.MissClick)
            {
                _multiplierStreak = 0;
                _totalStreak = 0;
                _scoreMultiplier = 1;
                OnMultiplierChanged?.Invoke(_scoreMultiplier);
                OnStreakChanged?.Invoke(_totalStreak);
                return true;
            }

            int nextMultiplier = _scoreMultiplier * 2 < _maxMultiplier
                ? _scoreMultiplier * 2
                : _maxMultiplier;

            _multiplierStreak++;
            _totalStreak++;
            OnStreakChanged?.Invoke(_totalStreak);

            if (_multiplierStreak == nextMultiplier)
            {
                _multiplierStreak = 0;
                _scoreMultiplier = nextMultiplier;
                OnStreakChanged?.Invoke(_scoreMultiplier);
            }

            return false;
        }

        private void CheckIfPlayerFailed(float health)
        {
            if (health > 0f)
                return;

            CompleteTracking(false);
        }

        /// <summary>
        /// Initiates a new PerformanceResult with the currentLevelID and current difficulty.
        /// </summary>
        public void StartTracking()
        {
            _score = 0;
            _totalStreak = 0;
            _multiplierStreak = 0;
            _scoreMultiplier = 1;
            _health = 1000f;

            _newResult = new PerformanceResult
            (
                SessionValues.Instance.currentLevelID,
                SessionValues.Instance.difficulty
            );

            OnHealthChanged += CheckIfPlayerFailed;
            LaneManager.Instance.OnNoteHit += ProcessNewHit;
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

            _lastCompletionStatus = levelBeaten
                ? ProcessNewScoreResult()
                : ScoreCompletionStatus.Failed;

            if(levelBeaten)
                SaveData();

            OnHealthChanged -= CheckIfPlayerFailed;
            LaneManager.Instance.OnNoteHit -= ProcessNewHit;

            OnScoreCompletion?.Invoke(_lastCompletionStatus);
        }

        private ScoreCompletionStatus ProcessNewScoreResult()
        {
            if (_highscores == null)
            {
                Debug.LogError("No reference set for 'highscores' on PerformanceTracker!");

                return ScoreCompletionStatus.NotBeaten;
            }

            int listLength = _highscores.Count;

            _newResult.totalScore = _score;

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
