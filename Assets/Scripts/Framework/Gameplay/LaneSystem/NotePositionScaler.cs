using UnityEngine;

using ProefExamen.Framework.Utils;
using Unity.VisualScripting;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    /// <summary>
    /// A class that is responsible for getting the correct scales and returning these to their targeted lanes.
    /// </summary>
    public class NotePositionScaler : AbstractSingleton<NotePositionScaler>
    {
        [Header("Scaling")]
        [SerializeField]
        private float _baseHeight = 1920f;

        [SerializeField]
        private float _baseWidth = 1080f;

        [Header("Lane starts")]
        [SerializeField]
        private Transform[] _startPoints;

        [Header("Lane end")]
        [SerializeField]
        private Transform[] _endPoints;

        [SerializeField]
        private GameObject _lineVisualiser;

        /// <summary>
        /// Updates a Lane's InitialNotePosition and TargetNotePosition.
        /// </summary>
        /// <param name="laneID">The Lane's Lane ID to update.</param>
        public void UpdateLaneNotePositions(int laneID)
        {
            Vector3 startPos = GetScaledStartPosition(laneID);
            Vector3 endPos = GetScaledEndPosition(laneID);

            LaneManager.Instance.Lanes[laneID].SetNotePositions(startPos, endPos);
        }

        /// <summary>
        /// Returns the lerp position of a lane ID.
        /// </summary>
        /// <param name="laneID">The lane from which the positions must be calculated.</param>
        /// <param name="lerp">The lerp value from 0-1 to use when returning the lerped position.</param>
        /// <returns>The lerped position from the targeted lane.</returns>
        public Vector3 GetLaneLerpPosition(int laneID, float lerp)
        {
            Vector3 startPos = GetScaledStartPosition(laneID);
            Vector3 endPos = GetScaledEndPosition(laneID);

            return Vector3.Lerp(startPos, endPos, lerp);
        }

        private void Start()
        {
            Vector3 startPos = GetScaledStartPosition(0);
            Vector3 endPos = GetScaledEndPosition(0);

            _lineVisualiser.transform.position = Vector3.Lerp(startPos, endPos, .55f);
        }

        private Vector3 GetScaledStartPosition(int laneID)
        {
            Vector3 startPos = _startPoints[laneID].position;
            startPos.x *= Screen.width / _baseWidth;
            startPos.y *= Screen.height / _baseHeight;

            return startPos;
        }

        private Vector3 GetScaledEndPosition(int laneID)
        {
            Vector3 endPos = _endPoints[laneID].position;
            endPos.x *= Screen.width / _baseWidth;
            endPos.y *= Screen.height / _baseHeight;

            return endPos;
        }
    }
}