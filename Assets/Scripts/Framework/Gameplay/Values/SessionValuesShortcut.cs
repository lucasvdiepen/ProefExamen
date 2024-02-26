using ProefExamen.Framework.Utils;
using UnityEngine;

using ProefExamen.Framework.Gameplay.MapData
    ;
namespace ProefExamen.Framework.Gameplay.Values
{
    public class SessionValuesShortcut : AbstractSingleton<SessionValuesShortcut>
    {
        public bool updateSettingsLive = true;

        public float travelTime;

        public float time;

        public int score;

        public int scoreMultiplier;

        public bool paused;

        public LevelData currentLevel;

        public Difficulty difficulty;

        public GameObject note;

        public KeyCode[] inputs;

        /// <summary>
        /// A function to quickly gather the settings from the static Settings class.
        /// </summary>
        [ContextMenu("Refresh Settings")]
        public void RefreshSettings()
        {
            travelTime = SessionValues.travelTime;
            time = SessionValues.time;
            score = SessionValues.score;
            paused = SessionValues.paused;
            currentLevel = SessionValues.currentLevel;
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
            SessionValues.paused = paused;
            SessionValues.currentLevel = currentLevel;
            SessionValues.note = note;

            RefreshSettings();
        }

        /// <summary>
        /// Used to keep settings up to date if updateSettingsLive is true.
        /// </summary>
        public void FixedUpdate()
        {
            if (!updateSettingsLive) return;

            time = SessionValues.time;
            travelTime = SessionValues.travelTime;
            score = SessionValues.score;
            paused = SessionValues.paused;
            currentLevel = SessionValues.currentLevel;
            note = SessionValues.note;
        }
    }
}