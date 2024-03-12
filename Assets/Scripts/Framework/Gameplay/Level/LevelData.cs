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
        /// The cover of the song that gets displayed.
        /// </summary>
        public Sprite songCover;

        /// <summary>
        /// The title of the song.
        /// </summary>
        public string title;

        /// <summary>
        /// The artists of the song.
        /// </summary>
        public string artists;

        /// <summary>
        /// The name of the album this song is from.
        /// </summary>
        public string album;

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
        public MappingData GetLevel()
        {
            if (mappingData.Length == 0)
                Debug.LogError("No MappingData for level with levelID " + levelID + "!");

            for (int i = 0; i < mappingData.Length; i++)
            {
                int difficultyInt = (int)SessionValues.Instance.difficulty;
                if ((int)mappingData[i].difficulty == difficultyInt)
                    return mappingData[i];
            }

            return mappingData[0];
        }

        /// <summary>
        /// Checks all the mapping data to see if a given difficulty is available.
        /// </summary>
        /// <param name="difficulty">The difficulty to check.</param>
        /// <returns>Returns true if there is a map for this difficulty, otherwise false.</returns>
        public bool LevelHasDifficulty(Difficulty difficulty)
        {
            for (int i = 0; i < mappingData.Length; i++)
                if (mappingData[i].difficulty == difficulty)
                    return true;

            return false;
        }
    }
}