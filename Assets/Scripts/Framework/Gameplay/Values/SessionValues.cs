using UnityEngine;
using Unity.Mathematics;

using ProefExamen.Framework.Gameplay.MapData;

namespace ProefExamen.Framework.Gameplay.Values
{
    /// <summary>
    /// The class that holds the value for a session, these are widely used and easily accessible through this static class.
    /// </summary>
    public static class SessionValues
    {
        /// <summary>
        /// The amount of time it takes for a note to enter the screen and exit the screen.
        /// </summary>
        public static float travelTime = SessionValuesShortcut.Instance.travelTime;
        /// <summary>
        /// The current time we are in the song, keeps being updated unless we are paused.
        /// </summary>
        public static float time = SessionValuesShortcut.Instance.travelTime * -1;

        /// <summary>
        /// The score that the player has.
        /// </summary>
        public static int score = SessionValuesShortcut.Instance.score;
        /// <summary>
        /// Multiplier for the score that is applied before adding to score.
        /// </summary>
        public static int scoreMultiplier = SessionValuesShortcut.Instance.scoreMultiplier;

        /// <summary>
        /// A bool that holds status for if the game is paused.
        /// </summary>
        public static bool paused = SessionValuesShortcut.Instance.paused;
        /// <summary>
        /// The currently selected Level ID.
        /// </summary>
        public static int currentLevelID = SessionValuesShortcut.Instance.currentLevelID;
        /// <summary>
        /// The currently selected Level, gets set when the ID is updated.
        /// </summary>
        public static LevelData currentLevel = SessionValuesShortcut.Instance.currentLevel;
        /// <summary>
        /// The selected difficulty, this is one value and if will be applied on the currentLevel when we start.
        /// </summary>
        public static Difficulty difficulty = SessionValuesShortcut.Instance.difficulty;
        /// <summary>
        /// The scriptableObject for all the levels, can be swapped out if necessary.
        /// </summary>
        public static Levels levels = SessionValuesShortcut.Instance.levels;

        /// <summary>
        /// The prefab for a note, can be any prefab if it has the Note.cs script.
        /// </summary>
        public static GameObject note = SessionValuesShortcut.Instance.note;

        /// <summary>
        /// A bool that decides if we are using PC Inputs.
        /// </summary>
        public static bool usingInputs = SessionValuesShortcut.Instance.usingInputs;
        /// <summary>
        /// The inputs from 0 - 3 for each lane when on PC.
        /// </summary>
        public static KeyCode[] inputs = SessionValuesShortcut.Instance.inputs;

        /// <summary>
        /// Returns a bool for if the upcoming timestamp should be queued yet.
        /// </summary>
        /// <param name="timeStamp">The timestamp that is being checked.</param>
        /// <returns>The status for if we want to queue the passed timestamp.</returns>
        public static bool TimeStampReadyForQueue(float timeStamp) => timeStamp > time && timeStamp - (travelTime * 1.1) < time;

        /// <summary>
        /// Will update the difficulty through a newly passed difficulty.
        /// </summary>
        /// <param name="newDifficulty">The new difficulty.</param>
        public static void SelectDifficulty(Difficulty newDifficulty) => difficulty = newDifficulty;

        /// <summary>
        /// Will loop over levels and update SessionValues currentLevel and currentLevelID.
        /// </summary>
        /// <param name="levelID">The new Level ID that is from the target level.</param>
        public static void SelectLevel(int levelID)
        {
            int listLenght = levels.levels.Count;
            currentLevelID = levelID;

            for (int i = 0; i < listLenght; i++)
                if (levels.levels[i].levelID == currentLevelID)
                    currentLevel = levels.levels[i];
        }

        /// <summary>
        /// Calculates the alpha 0-1 value for a note through a given timestamp.
        /// </summary>
        /// <param name="targetTime">The give timestamp to base the calculation on.</param>
        /// <returns>The 0-1 alpha value. When it is currently at the given timestamp alpha will be 0.5.</returns>
        public static float ReturnNoteLerpAlpha(float targetTime)
        {
            float halfTravelTime = travelTime * .5f;

            float range = time - (targetTime - halfTravelTime);

            float alpha = range / travelTime;

            return math.clamp(alpha, 0, 1);
        }
    }
}
