using ProefExamen.Audio.TimeStamping;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for storing the time stamps of a song.
/// </summary>
public class TimeStampDataContainer : ScriptableObject
{
    public List<TimeStamper.LineData> songDebugLineData = new(); //debug data used for drawing gizmo lines
    public float[] timeStamps; //time stamps
}