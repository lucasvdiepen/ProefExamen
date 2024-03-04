using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.PerformanceTracker
{
    /// <summary>
    /// A scriptable object holding a list of results for levels.
    /// </summary>
    [CreateAssetMenu(fileName = "ScoreResults", menuName = "ScriptableObjects/ScoreResults")]
    public class ScoreResults : ScriptableObject
    {
        /// <summary>
        /// A list containing HighScores for levels.
        /// </summary>
        public List<PerformanceResult> highScores = new();
    }
}
