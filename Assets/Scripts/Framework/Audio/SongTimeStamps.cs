using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for storing the time stamps of a song.
/// </summary>
public class SongTimeStamps : ScriptableObject
{
    public List<float> timeStamps = new();
}
