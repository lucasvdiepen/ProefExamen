using UnityEngine;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.Gameplay.PerformanceTracking
{
    /// <summary>
    /// A class that holds a players performance from playing a level.
    /// </summary>
    [System.Serializable]
    public class PerformanceResult
    {
        /// <summary>
        /// Create a new initial Performance Result with assigned Level ID and difficulty.
        /// </summary>
        /// <param name="levelID">The Level ID of this result.</param>
        /// <param name="difficulty">The difficulty of this result.</param>
        public PerformanceResult(int levelID, Difficulty difficulty)
        {
            this.levelID = levelID;
            this.difficulty = difficulty;

            misses = 0;
            missClicks = 0;
            okHits = 0;
            alrightHits = 0;
            niceHits = 0;
            perfectHits = 0;
            totalScore = 0;

            PerformanceTracker.Instance.OnPointsChanged += UpdateTotalScore;
        }

        ~PerformanceResult()
        {
            PerformanceTracker.Instance.OnPointsChanged -= UpdateTotalScore;
        }

        /// <summary>
        /// The Level ID of this result.
        /// </summary>
        [Header("Level Data")]
        public int levelID;

        /// <summary>
        /// The difficulty of this result.
        /// </summary>
        public Difficulty difficulty;

        /// <summary>
        /// The amount of Misses in this result.
        /// </summary>
        [Header("Performance")]
        public int misses;

        /// <summary>
        /// The amount of MissClicks in this result.
        /// </summary>
        public int missClicks;

        /// <summary>
        /// The amount of Ok hits in this result.
        /// </summary>
        public int okHits;

        /// <summary>
        /// The amount of Alright hits in this result.
        /// </summary>
        public int alrightHits;

        /// <summary>
        /// The amount of Nice hits in this result.
        /// </summary>
        public int niceHits;

        /// <summary>
        /// The amount of Perfect hits in this result.
        /// </summary>
        public int perfectHits;

        /// <summary>
        /// The total score in this result.
        /// </summary>
        [Header("Score")]
        public int totalScore;

        private void UpdateTotalScore(int newScore) => totalScore = newScore;

        /// <summary>
        /// Returns the result of if a passed level is the same as this one.
        /// </summary>
        /// <param name="newResult">The level to compare this level to.</param>
        /// <returns>The result for if these levels are the same.</returns>
        public bool CompareLevels(PerformanceResult newResult) => 
            newResult.levelID == levelID && newResult.difficulty == difficulty;

        /// <summary>
        /// Adds the hit to the result.
        /// </summary>
        /// <param name="hit">The new hit.</param>
        public void AddHit(HitStatus hit)
        {
            switch (hit)
            {
                case HitStatus.Miss:
                    misses++;
                    break;
                case HitStatus.MissClick:
                    missClicks++;
                    break;
                case HitStatus.Ok:
                    okHits++;
                    break;
                case HitStatus.Alright:
                    alrightHits++;
                    break;
                case HitStatus.Nice:
                    niceHits++;
                    break;
                case HitStatus.Perfect:
                    perfectHits++;
                    break;
            }
        }
    }
}
