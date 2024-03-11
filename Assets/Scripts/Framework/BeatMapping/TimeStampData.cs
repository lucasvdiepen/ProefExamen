using UnityEngine;

namespace ProefExamen.Framework.BeatMapping
{
    /// <summary>
    /// Class responsible for holding the necessary data for a time stamp.
    /// </summary>
    [System.Serializable]
    public class TimeStampData
    {
        /// <summary>
        /// Holds the start and end point of the time stamp.
        /// </summary>
        public LineData lineData;

        /// <summary>
        /// Holds the actual song time of the time stamp.
        /// </summary>
        public float songTime;

        /// <summary>
        /// Holds the lane ID of the time stamp.
        /// </summary>
        public int laneID;

        /// <summary>
        /// Returns if this time stamp is selected.
        /// </summary>
        public bool isSelected;

        /// <summary>
        /// Constructor for the time stamp data.
        /// </summary>
        /// <param name="start">Start point of verticle line.</param>
        /// <param name="end">End point of vertical line.</param>
        /// <param name="time">Associated song time.</param>
        public TimeStampData(Vector2 start, Vector2 end, float time, int laneID)
        {
            lineData.startLinePoint = start;
            lineData.endLinePoint = end;

            songTime = time;
            this.laneID = laneID;
        }
    }
}