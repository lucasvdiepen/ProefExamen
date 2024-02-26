using UnityEngine;
using Unity.Mathematics;

using ProefExamen.Framework.Gameplay.MapData;

namespace ProefExamen.Framework.Gameplay.Values
{
    public static class SessionValues
    {
        public static float travelTime = SessionValuesShortcut.Instance.travelTime;

        public static float time = SessionValuesShortcut.Instance.travelTime * -1;

        public static int score = SessionValuesShortcut.Instance.score;

        public static int scoreMultiplier = SessionValuesShortcut.Instance.scoreMultiplier;

        public static bool paused = SessionValuesShortcut.Instance.paused;

        public static Difficulty difficulty = SessionValuesShortcut.Instance.difficulty;

        public static LevelData currentLevel = SessionValuesShortcut.Instance.currentLevel;

        public static GameObject note = SessionValuesShortcut.Instance.note;

        public static KeyCode[] inputs = SessionValuesShortcut.Instance.inputs;

        public static bool TimeStampReadyForQueue(float timeStamp) => timeStamp > time && timeStamp - (travelTime * 1.1) < time;

        public static float ReturnNoteLerpAlpha(float targetTime)
        {
            float halfTravelTime = travelTime * .5f;

            float range = time - (targetTime - halfTravelTime);

            float alpha = range / travelTime;

            return math.clamp(alpha, 0, 1);
        }
    }
}
