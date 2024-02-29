using UnityEngine;

using ProefExamen.Framework.Gameplay.MapData;

namespace ProefExamen.Framework.Gameplay.Values
{
    /// <summary>
    /// The class that holds the value for a session, these are widely used and easily accessible through 
    /// this static class.
    /// </summary>
    public static class SessionValues
    {
        /// <summary>
        /// The amount of time it takes for a note to enter the screen and exit the screen.
        /// </summary>
        public static float travelTime = SessionValuesEditor.Instance.travelTime;

        /// <summary>
        /// The current time we are in the song, keeps being updated unless we are paused.
        /// </summary>
        public static float time = SessionValuesEditor.Instance.travelTime * -1;

        /// <summary>
        /// The score that the player has.
        /// </summary>
        public static int score = SessionValuesEditor.Instance.score;

        /// <summary>
        /// Multiplier for the score that is applied before adding to score.
        /// </summary>
        public static int scoreMultiplier = SessionValuesEditor.Instance.scoreMultiplier;

        /// <summary>
        /// The max difference in the lerpAlpha value that a note can have before not registering a hit anymore.
        /// </summary>
        public static float lerpAlphaHitThreshold = SessionValuesEditor.Instance._alphaLerpHitThreshold;

        /// <summary>
        /// A bool that holds status for if the game is paused.
        /// </summary>
        public static bool paused = SessionValuesEditor.Instance.paused;

        /// <summary>
        /// The currently selected Level ID.
        /// </summary>
        public static int currentLevelID = SessionValuesEditor.Instance.currentLevelID;

        /// <summary>
        /// The currently selected Level, gets set when the ID is updated.
        /// </summary>
        public static LevelData currentLevel = SessionValuesEditor.Instance.currentLevel;

        /// <summary>
        /// The selected difficulty, this is one value and if will be applied on the currentLevel when we start.
        /// </summary>
        public static Difficulty difficulty = SessionValuesEditor.Instance.difficulty;

        /// <summary>
        /// The scriptableObject for all the levels, can be swapped out if necessary.
        /// </summary>
        public static Levels levels = SessionValuesEditor.Instance.levels;

        /// <summary>
        /// The prefab for a note, can be any prefab if it has the Note.cs script.
        /// </summary>
        public static GameObject note = SessionValuesEditor.Instance.note;

        /// <summary>
        /// Returns a bool for if the upcoming timestamp should be queued yet.
        /// </summary>
        /// <param name="timeStamp">The timestamp that is being checked.</param>
        /// <returns>The status for if we want to queue the passed timestamp.</returns>
        public static bool IsTimeStampReadyForQueue(float timeStamp) => 
            timeStamp > time && timeStamp - (travelTime * 1.1) < time;

        /// <summary>
        /// Will loop over levels and update SessionValues currentLevel and currentLevelID.
        /// </summary>
        /// <param name="levelID">The new Level ID that is from the target level.</param>
        public static void SelectLevel(int levelID)
        {
            int listLength = levels.levels.Count;
            currentLevelID = levelID;

            for (int i = 0; i < listLength; i++)
                if (levels.levels[i].levelID == currentLevelID)
                    currentLevel = levels.levels[i];
        }

        /// <summary>
        /// Calculates the alpha 0-1 value for a note through a given timestamp.
        /// </summary>
        /// <param name="targetTime">The given timestamp to base the calculation on.</param>
        /// <returns>The 0-1 alpha value. When it is currently at the given timestamp alpha will be 0.5.</returns>
        public static float CalculateNoteLerpAlpha(float targetTime)
        {
            float halfTravelTime = travelTime * .5f;

            float range = time - (targetTime - halfTravelTime);

            float alpha = range / travelTime;

            return Mathf.Clamp01(alpha);
        }
    }
}