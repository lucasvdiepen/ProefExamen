using UnityEngine;
using Unity.Mathematics;

namespace ProefExamen.Framework.Gameplay.Settings
{
    public static class Settings
    {
        public static float travelTime = 1;

        public static float time = travelTime * -1;

        public static int score = 0;

        public static bool paused = false;

        public static LevelData currentLevel;

        public static GameObject note;

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
