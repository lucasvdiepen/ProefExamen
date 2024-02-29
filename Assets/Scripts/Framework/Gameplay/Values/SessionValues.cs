using UnityEngine;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Level;

namespace ProefExamen.Framework.Gameplay.Values
{
    /// <summary>
    /// A class responsible for setting the default variables of the static class SessionValues. 
    /// Is also used to view values of SessionValues realtime which is usefull for debuggin.
    /// </summary>
    public class SessionValues : AbstractSingleton<SessionValues>
    {
        /// <summary>
        /// A bool that decides if the values here must be updated during a session to watch values.
        /// Used for debugging.
        /// </summary>
        public bool shouldUpdateLive = true;

        /// <summary>
        /// The travelTime default.
        /// </summary>
        public float travelTime;

        /// <summary>
        /// The time through a song default.
        /// </summary>
        public float time;

        /// <summary>
        /// The score default.
        /// </summary>
        public int score;

        /// <summary>
        /// The score multiplier default.
        /// </summary>
        public int scoreMultiplier;

        /// <summary>
        /// The paused bool default.
        /// </summary>
        public bool paused;

        /// <summary>
        /// The currentLevelID default.
        /// </summary>
        public int currentLevelID;

        /// <summary>
        /// The currentLevel default.
        /// </summary>
        public LevelData currentLevel;

        /// <summary>
        /// The default difficulty.
        /// </summary>
        public Difficulty difficulty;

        /// <summary>
        /// The threshold between 1 and the okThreshold will be the range for getting an Ok.
        /// </summary>
        public float okThreshold = .75f;

        /// <summary>
        /// The threshold between okThreshold and alrightThreshold will be the range for getting an Alright.
        /// </summary>
        public float alrightThreshold = .5f;

        /// <summary>
        /// The threshold between alrightThreshold and niceThreshold will be the range for getting an Nice, 
        /// everything below that is Perfect.
        /// </summary>
        public float niceThreshold = .25f;

        /// <summary>
        /// The max difference in the lerpAlpha value that a note can have before not registering a hit anymore.
        /// </summary>
        public float lerpAlphaHitThreshold = .2f;

        /// <summary>
        /// The levels default.
        /// </summary>
        public Levels levels;

        /// <summary>
        /// The Note default.
        /// </summary>
        public GameObject note;

        /// <summary>
        /// Returns a bool for if the upcoming timestamp should be queued yet.
        /// </summary>
        /// <param name="timeStamp">The timestamp that is being checked.</param>
        /// <returns>The status for if we want to queue the passed timestamp.</returns>
        public bool IsTimeStampReadyForQueue(float timeStamp) =>
            timeStamp > time && timeStamp - (travelTime * 1.1) < time;

        /// <summary>
        /// Will loop over levels and update SessionValues currentLevel and currentLevelID.
        /// </summary>
        /// <param name="levelID">The new Level ID that is from the target level.</param>
        public void SelectLevel(int levelID)
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
        public float CalculateNoteLerpAlpha(float targetTime)
        {
            float halfTravelTime = travelTime * .5f;

            float range = time - (targetTime - halfTravelTime);

            float alpha = range / travelTime;

            return Mathf.Clamp01(alpha);
        }
    }
}