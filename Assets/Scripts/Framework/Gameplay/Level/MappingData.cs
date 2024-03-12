using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.Level
{
    /// <summary>
    /// A struct holding mapping data seperatly so that it can be defined for each difficulty.
    /// </summary>
    [System.Serializable]
    public struct MappingData
    {
        /// <summary>
        /// The difficulty for this mapping data.
        /// </summary>
        public Difficulty difficulty;

        /// <summary>
        /// The timeStamps for notes.
        /// </summary>
        public float[] timeStamps;

        /// <summary>
        /// The lane ID for each timestamp that the notes have to be spawned on.
        /// </summary>
        public int[] laneIDs;
    }
}
