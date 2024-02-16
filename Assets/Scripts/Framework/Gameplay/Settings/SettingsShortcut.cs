using UnityEngine;

#if UNITY_EDITOR
namespace ProefExamen.Framework.Gameplay.Settings
{
    public class SettingsShortcut : MonoBehaviour
    {
        public bool updateSettingsLive = true;

        public float travelTime = Settings.travelTime;

        public float time = Settings.time;

        public int score = Settings.score;

        public bool paused = Settings.paused;

        public LevelData currentLevel = Settings.currentLevel;

        public GameObject note = Settings.note;

        /// <summary>
        /// A function to quickly gather the settings from the static Settings class.
        /// </summary>
        [ContextMenu("Refresh Settings")]
        public void RefreshSettings()
        {
            travelTime = Settings.travelTime;
            time = Settings.time;
            score = Settings.score;
            paused = Settings.paused;
            currentLevel = Settings.currentLevel;
            note = Settings.note;
        }

        /// <summary>
        /// A function that pushes newly set settings to the statis Settings class.
        /// </summary>
        [ContextMenu("Push New Settings")]
        public void PushSettings()
        {
            Settings.travelTime = travelTime;
            Settings.time = travelTime * -1;
            Settings.score = score;
            Settings.paused = paused;
            Settings.currentLevel = currentLevel;
            Settings.note = note;

            RefreshSettings();
        }

        /// <summary>
        /// Used to keep settings up to date if updateSettingsLive is true.
        /// </summary>
        public void FixedUpdate()
        {
            if (!updateSettingsLive) return;

            time = Settings.time;
            travelTime = Settings.travelTime;
            score = Settings.score;
            paused = Settings.paused;
            currentLevel = Settings.currentLevel;
            note = Settings.note;
        }
    }
}
#endif