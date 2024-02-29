using UnityEngine;

using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    /// <summary>
    /// A class that handles the logic for the Note that travels on a lane.
    /// </summary>
    public class Note : MonoBehaviour
    {
        [Header("Note Data")]
        [SerializeField]
        private float _timeStamp = 0;

        [SerializeField]
        private int _laneID = -1;

        [SerializeField]
        private int _levelID = -1;
        
        [SerializeField]
        private Vector2 _initialPosition;

        [SerializeField]
        private Vector2 _targetPosition;

        [Header("Lerping")]
        [SerializeField]
        private float _lerpAlpha = 0;

        [SerializeField]
        private bool _isRemovalCalled;

        /// <summary>
        /// The LerpAlpha 0-1 float value that is used to place the note on the lane.
        /// </summary>
        public float LerpAlpha => _lerpAlpha;

        /// <summary>
        /// A function that is used to set the Note's initial values correctly.
        /// </summary>
        /// <param name="initialPosition">The initial position for the Note.</param>
        /// <param name="targetPosition">The target position the Note needs to move to.</param>
        /// <param name="laneID">The Lane ID of the Lane that the Note is on.</param>
        /// <param name="levelID">The Level ID of the level that the Note originates from.</param>
        /// <param name="timeStamp">The timestamp that the Note needs to be hit on.</param>
        public void SetNoteValues(Vector2 initialPosition, Vector2 targetPosition, int laneID, int levelID, float timeStamp)
        {
            _initialPosition = initialPosition;
            _targetPosition = targetPosition;
            _laneID = laneID;
            _levelID = levelID;
            _timeStamp = timeStamp;

            transform.position = _initialPosition;
        }

        private void Update()
        {
            if (SessionValues.Instance.paused || _laneID == -1)
                return;

            if (_lerpAlpha > .99f)
                Destroy(gameObject);

            _lerpAlpha = SessionValues.Instance.CalculateNoteLerpAlpha(_timeStamp);

            transform.position = Vector3.Lerp(_initialPosition, _targetPosition, _lerpAlpha);

            if (_isRemovalCalled || _lerpAlpha <= (SessionValues.Instance.lerpAlphaHitThreshold + .5f)) 
                return;

            _isRemovalCalled = true;
            LaneManager.Instance.OnNoteHit?.Invoke(HitStatus.Miss, _laneID);
        }
    }
}