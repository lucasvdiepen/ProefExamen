using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class Settings
{
    public static float travelTime = 1;

    public static float time = travelTime * -1;

    public static bool paused = false;

    public static LevelData currentLevel;

    public static GameObject note;

    public static bool TimeStampReadyForQueue(float timeStamp) => timeStamp > time && timeStamp - (travelTime * 1.1) < time;
}
