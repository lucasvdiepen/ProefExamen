using UnityEngine;
using System;

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
        private Lane _lane;

        [SerializeField]
        private int _levelID = -1;
        
        [SerializeField]
        private Vector2 _initialPosition;

        [SerializeField]
        private Vector2 _targetPosition;

        [Header("Visuals")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private float _animationSpeed = 4;

        [SerializeField]
        private Sprite _deathSprite;

        [Header("Lerping")]
        [SerializeField]
        private float _lerpAlpha = 0;

        [SerializeField]
        private bool _isRemovalCalled;

        /// <summary>
        /// The LerpAlpha 0-1 float value that is used to place the note on the lane.
        /// </summary>
        public float LerpAlpha => _lerpAlpha;

        private void Awake()
        {
            if(_animator == null)
                _animator = gameObject.GetComponent<Animator>();

            if (_spriteRenderer == null)
                _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// A function that is used to set the Note's initial values correctly.
        /// </summary>
        /// <param name="initialPosition">The initial position for the Note.</param>
        /// <param name="targetPosition">The target position the Note needs to move to.</param>
        /// <param name="laneID">The Lane ID of the Lane that the Note is on.</param>
        /// <param name="levelID">The Level ID of the level that the Note originates from.</param>
        /// <param name="timeStamp">The timestamp that the Note needs to be hit on.</param>
        public void SetNoteValues(Lane lane,Vector2 initialPosition, Vector2 targetPosition, int laneID, int levelID, float timeStamp)
        {
            _lane = lane;
            _initialPosition = initialPosition;
            _targetPosition = targetPosition;
            _laneID = laneID;
            _levelID = levelID;
            _timeStamp = timeStamp;

            transform.position = _initialPosition;
        }

        /// <summary>
        /// Instantiates a DeadNote and destroys this Note.
        /// </summary>
        public void HitNote(HitStatus hitStatus, Sprite hitSprite)
        {
            if (LaneManager.Instance.IsBeatMapping)
                return;

            bool miss = hitStatus == HitStatus.Miss || hitStatus == HitStatus.MissClick;

            GameObject deadNote = Instantiate(SessionValues.Instance.deadNote);

            Sprite sprite = miss ? null : _deathSprite;
            deadNote.GetComponent<DeadNote>().SetDeadNoteValues(sprite, hitSprite, gameObject.transform);
            
            if (miss)
                return;

            Destroy(gameObject);
        }

        /// <summary>
        /// Checks if the note should exist when called during BeatMapping, when it shouldn't it will destroy itself.
        /// </summary>
        public void CheckIfNoteShouldExist()
        {
            if (!LaneManager.Instance.IsBeatMapping)
                return;

            if (LaneManager.Instance.liveTimeStamps.Contains(new Tuple<float, int>(_timeStamp, _laneID)))
                return;

            Destroy(gameObject);
        }

        private void Update()
        {
            if (SessionValues.Instance.paused || _laneID == -1)
            {
                if (!LaneManager.Instance.IsBeatMapping)
                    _animator.speed = 0;
                return;
            }

            if (!LaneManager.Instance.IsBeatMapping)
                _animator.speed = _animationSpeed / SessionValues.Instance.travelTime;

            if (_lerpAlpha > .99f)
                Destroy(gameObject);

            _lerpAlpha = SessionValues.Instance.CalculateNoteLerpAlpha(_timeStamp);

            transform.position = Vector3.Lerp(_initialPosition, _targetPosition, _lerpAlpha);

            if (_isRemovalCalled || _lerpAlpha <= (SessionValues.Instance.lerpAlphaHitThreshold + .5f))
                return;

            _isRemovalCalled = true;
            _lane.RemoveNote(this);
            LaneManager.Instance.OnNoteHit?.Invoke(HitStatus.Miss, _laneID);
        }
    }
}