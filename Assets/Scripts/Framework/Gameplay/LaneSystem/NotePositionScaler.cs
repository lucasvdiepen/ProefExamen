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
            Vector3 startPos = _startPoints[laneID].position;
            startPos.x *= Screen.width / _baseWidth;
            startPos.y *= Screen.height / _baseHeight;

            Vector3 endPos = _endPoints[laneID].position;
            endPos.x *= Screen.width / _baseWidth;
            endPos.y *= Screen.height / _baseHeight;

            LaneManager.Instance.Lanes[laneID].SetNotePositions(startPos, endPos);
        }

        private void Start()
        {
            Vector3 startPos = _startPoints[0].position;
            startPos.x *= Screen.width / _baseWidth;
            startPos.y *= Screen.height / _baseHeight;

            Vector3 endPos = _endPoints[0].position;
            endPos.x *= Screen.width / _baseWidth;
            endPos.y *= Screen.height / _baseHeight;

            _lineVisualiser.transform.position = Vector3.Lerp(startPos, endPos, .55f);
        }
    }
}