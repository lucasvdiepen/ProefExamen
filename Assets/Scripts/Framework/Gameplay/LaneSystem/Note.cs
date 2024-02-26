using UnityEngine;
using System;

using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    public class Note : MonoBehaviour
    {
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

        [SerializeField]
        private float _lerpAlpha = 0;

        [SerializeField]
        private bool _calledRemoval;

        public Action<GameObject> CallNoteRemoval;

        public float LerpAlpha
        {
            get => _lerpAlpha;
        }

        public void SetNoteValues(Vector2 initialPosition, Vector2 targetPosition, int laneID, int levelID, float timeStamp)
        {
            _initialPosition = initialPosition;
            _targetPosition = targetPosition;
            _laneID = laneID;
            _levelID = levelID;
            _timeStamp = timeStamp;
        }

        public void Update()
        {
            if (SessionValues.paused || _laneID == -1)
                return;
            else if ((_timeStamp + (SessionValues.travelTime * .5) < SessionValues.time))
                Destroy(gameObject);

            _lerpAlpha = SessionValues.ReturnNoteLerpAlpha(_timeStamp);

            transform.position = Vector3.Lerp(_initialPosition, _targetPosition, _lerpAlpha);

            if (!_calledRemoval && _lerpAlpha > .65f)
            {
                _calledRemoval = true;

                SessionValues.score -= 5;

                CallNoteRemoval?.Invoke(gameObject);
            }
        }
    }
}