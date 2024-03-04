using ProefExamen.Framework.Gameplay.LaneSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.PerformanceTracker
{
    /// <summary>
    /// A struct that holds a players performance from playing a level.
    /// </summary>
    public struct PerformanceResult
    {
        public PerformanceResult(int levelID)
        {
            this.levelID = levelID;
            misses = 0;
            missClicks = 0;
            okHits = 0;
            alrightHits = 0;
            niceHits = 0;
            perfectHits = 0;
        }
        /// <summary>
        /// The Level ID of this result.
        /// </summary>
        public int levelID;

        /// <summary>
        /// The amount of Misses in this result.
        /// </summary>
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
        /// Adds the hit to the result.
        /// </summary>
        /// <param name="hit">The new hit.</param>
        public void CountHit(HitStatus hit)
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
