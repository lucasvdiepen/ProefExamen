using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public int levelID;

    public AudioSource song;

    public float[] timestamps;

    public int[] laneIDs;
}