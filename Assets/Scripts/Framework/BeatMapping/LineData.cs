using UnityEngine;

namespace ProefExamen.Framework.BeatMapping
{
    /// <summary>
    /// Struct responsible for holding the necessary data for a gizmo line.
    /// </summary>
    [System.Serializable]
    public struct LineData
    {
        /// <summary>
        /// Start point of the line.
        /// </summary>
        public Vector2 startLinePoint;

        /// <summary>
        /// End point of the line.
        /// </summary>
        public Vector2 endLinePoint;
    }
}