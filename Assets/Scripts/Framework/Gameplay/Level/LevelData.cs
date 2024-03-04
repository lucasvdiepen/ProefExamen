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
        /// <summary>
        /// The ID of a level.
        /// </summary>
        public int levelID;

        /// <summary>
        /// The playable sound of this level.
        /// </summary>
        public AudioClip song;

        /// <summary>
        /// All the mapping data of this level needs to contain Easy, Normal and Hard
        /// even if not used.
        /// </summary>
        public MappingData[] mappingData;

        /// <summary>
        /// A function that returns the current levels mapData based on the current difficulty.
        /// </summary>
        /// <returns>The mapData of this level on the current difficulty.</returns>
        public MappingData GetLevel() => mappingData[(int)SessionValues.Instance.difficulty];
    }
}