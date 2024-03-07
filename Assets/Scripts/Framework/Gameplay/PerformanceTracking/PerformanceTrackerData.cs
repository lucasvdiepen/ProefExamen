using System;
using System.Collections.Generic;

namespace ProefExamen.Framework.Gameplay.PerformanceTracking
{
    /// <summary>
    /// A struct that holds a list of the highscores for parsing to and from JSON.
    /// </summary>
    [Serializable]
    public struct PerformanceTrackerData
    {
        /// <summary>
        /// A list containing highscores for levels.
        /// </summary>
        public List<PerformanceResult> highscores;
    }
}