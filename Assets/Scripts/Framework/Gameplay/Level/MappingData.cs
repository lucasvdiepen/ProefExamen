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
        /// The timestamps for notes.
        /// </summary>
        public float[] timestamps;

        /// <summary>
        /// The lane ID for each live timestamp that the notes have to be spawned on.
        /// </summary>
        public List<Tuple<float, int>> liveTimeStamps;

        /// <summary>
        /// The lane ID for each timestamp that the notes have to be spawned on.
        /// </summary>
        public int[] laneIDs;

        /// <summary>
        /// The amount of lanes that the notes have to be spawned on.
        /// </summary>
        public void SortLiveTimeStamps()
        {
            if (liveTimeStamps.Count <= 1)
                return;

            liveTimeStamps.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        }
    }
}
