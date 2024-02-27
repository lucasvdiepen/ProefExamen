using ProefExamen.Framework.Utils;
using UnityEngine;

using ProefExamen.Framework.Gameplay.MapData;

namespace ProefExamen.Framework.Gameplay.Values
{
    /// <summary>
    /// A class responsible for setting the default variables of the static class SessionValues. 
    /// Is also used to view values of SessionValues realtime which is usefull for debuggin.
    /// </summary>
    public class SessionValuesShortcut : AbstractSingleton<SessionValuesShortcut>
    {
        /// <summary>
        /// A bool that decides if the values here must be updated during a session to watch values.
        /// Used for debugging.
        /// </summary>
        public bool updateSettingsLive = true;

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
        /// The levels default.
        /// </summary>
        public Levels levels;

        public GameObject note;

        /// <summary>
        /// A function to quickly gather the settings from the static Settings class.
        /// </summary>
        [ContextMenu("Refresh Settings")]
        public void RefreshSettings()
        {
            travelTime = SessionValues.travelTime;
            time = SessionValues.time;

            score = SessionValues.score;
            scoreMultiplier = SessionValues.scoreMultiplier;

            paused = SessionValues.paused;
            levels = SessionValues.levels;
            currentLevelID = SessionValues.currentLevelID;
            currentLevel = SessionValues.currentLevel;
            difficulty = SessionValues.difficulty;

            note = SessionValues.note;
        }

        /// <summary>
        /// A function that pushes newly set settings to the static Settings class.
        /// </summary>
        [ContextMenu("Push New Settings")]
        public void PushSettings()
        {
            SessionValues.travelTime = travelTime;
            SessionValues.time = travelTime * -1;

            SessionValues.score = score;
            SessionValues.scoreMultiplier = scoreMultiplier;

            SessionValues.levels = levels;

            SessionValues.paused = paused;
            SessionValues.SelectDifficulty(difficulty);
            SessionValues.SelectLevel(currentLevelID);

            SessionValues.note = note;

            RefreshSettings();
        }

        private void FixedUpdate()
        {
            if (!updateSettingsLive) return;

            time = SessionValues.time;
            travelTime = SessionValues.travelTime;
            score = SessionValues.score;
            paused = SessionValues.paused;

            currentLevelID = SessionValues.currentLevelID;
            currentLevel = SessionValues.currentLevel;
            difficulty = SessionValues.difficulty;

            note = SessionValues.note;
        }
    }
}