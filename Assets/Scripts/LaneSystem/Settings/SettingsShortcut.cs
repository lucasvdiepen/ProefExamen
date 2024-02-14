using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class SettingsShortcut : MonoBehaviour
{
    public float time = Settings.time;

    public float travelTime = Settings.travelTime;

    public bool paused = Settings.paused;

    public LevelData currentLevel = Settings.currentLevel;

    public GameObject note = Settings.note;

    
    /*private void OnValidate()
    {
        Settings.time = time;
        Settings.travelTime = travelTime;
        Settings.paused = paused;
        Settings.note = note;
    }*/

    public void FixedUpdate()
    {
        time = Settings.time;
        travelTime = Settings.travelTime;
        paused = Settings.paused;
        currentLevel = Settings.currentLevel;
        note = Settings.note;
    }
}
#endif
