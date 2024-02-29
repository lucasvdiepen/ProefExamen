using ProefExamen.Framework.Gameplay.Values;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.MapData
{
    /// <summary>
    /// A struct holding all needed data for a level.
    /// </summary>
    [System.Serializable]
    public struct LevelData
    {
        public int levelID;
        public AudioClip song;
        public Level[] level;

        /// <summary>
        /// A function that returns the current levels mapData based on the current difficulty.
        /// </summary>
        /// <returns>The mapData of this level on the current difficulty.</returns>
        public Level GetLevel() => level[(int)SessionValues.difficulty];
    }

    /// <summary>
    /// An enum that is used to express the difficulty of a level.
    /// </summary>
    [System.Serializable]
    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    /// <summary>
    /// A struct holding mapping data seperatly so that it can be defined for each difficulty.
    /// </summary>
    [System.Serializable]
    public struct Level
    {
        public Difficulty difficulty;
        public float[] timestamps;
        public int[] laneIDs;
    }
}