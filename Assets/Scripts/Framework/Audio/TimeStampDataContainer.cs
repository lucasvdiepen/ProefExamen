using ProefExamen.Audio.TimeStamping;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for storing the time stamps of a song.
/// </summary>
public class TimeStampDataContainer : ScriptableObject
{
    public List<TimeStamper.LineData> songDebugLineData = new();
    public float[] timeStamps;
}