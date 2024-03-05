using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.BeatMapping
{
    /// <summary>
    /// Class responsible for storing the time stamps of a song.
    /// </summary>
    public class TimeStampDataContainer : ScriptableObject
    {
        /// <summary>
        /// List of line data used for drawing gizmo lines.
        /// </summary>
        public List<LineData> songDebugLineData = new();

        /// <summary>
        /// Array of time stamps.
        /// </summary>
        public float[] timeStamps;

        /// <summary>
        /// Array of lane IDs.
        /// </summary>
        public int[] laneIDs;
    }
}