using UnityEngine;

using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.Gameplay.Level
{
    /// <summary>
    /// A struct holding all needed data for a level.
    /// </summary>
    [System.Serializable]
    public struct LevelData
    {
        public int levelID;
        public AudioClip song;
        public MappingData[] mappingData;

        /// <summary>
        /// A function that returns the current levels mapData based on the current difficulty.
        /// </summary>
        /// <returns>The mapData of this level on the current difficulty.</returns>
        public MappingData GetLevel() => mappingData[(int)SessionValues.Instance.difficulty];
    }
}