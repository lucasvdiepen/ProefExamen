using ProefExamen.Framework.Gameplay.Values;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.MapData
{
    [System.Serializable]
    public struct LevelData
    {
        public int levelID;

        public AudioClip song;

        public Level[] level;

        public Level Level()
        {
            return level[(int)SessionValues.difficulty];
        }
    }

    [System.Serializable]
    public enum Difficulty
    {
        EASY = 0,
        NORMAL = 1,
        HARD = 2
    }

    [System.Serializable]
    public struct Level
    {
        public Difficulty difficulty;

        public float[] timestamps;

        public int[] laneIDs;
    }
}