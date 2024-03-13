using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Utils.Libraries.LaneUtils;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    /// <summary>
    /// A class that manages a lane.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class Lane : MonoBehaviour
    {
        [SerializeField]
        private int _laneID = -1;

        [Header("References")]

        [SerializeField]
        private List<Note> _notes = new();

        [Header("Lerp positions")]
        [SerializeField]
        private Vector3 _initialNotePosition;

        [SerializeField]
        private Vector3 _targetNotePosition;

        [SerializeField]
        private GameObject _notePrefab;

        /// <summary>
        /// Gets the registered Notes of this lane.
        /// </summary>
        public List<Note> Notes => _notes;

        private void Start()
        {
            if (_laneID == -1)
            {
                Debug.LogError("Lane has no assigned ID!");
                return;
            }

            NotePositionScaler.Instance.UpdateLaneNotePositions(_laneID);
        }

        /// <summary>
        /// Sets the InitialNotePosition and TargetNotePosition to the newly passed values.
        /// </summary>
        /// <param name="initialPosition">The new InitialNotePosition.</param>
        /// <param name="targetPosition">The new TargetNotePosition.</param>
        public void SetNotePositions(Vector3 initialPosition, Vector3 targetPosition)
        {
            _initialNotePosition = initialPosition;
            _targetNotePosition = targetPosition;
        }

        /// <summary>
        /// Called when the lane button is pressed.
        /// </summary>
        public void OnButtonPressed()
        {
            HitStatus hitResult = _notes.Count == 0
                ? HitStatus.Miss
                : LaneUtils.CalculateHitStatus(_notes[0].LerpAlpha);

            LaneManager.Instance.OnNoteHit?.Invoke(hitResult, _laneID);
        }

        /// <summary>
        /// Instantiates a DeadNote and destroys this Note.
        /// </summary>
        public void HitNote(HitStatus hitStatus, Sprite hitSprite)
        {
            if (LaneManager.Instance.IsBeatMapping)
                return;

            bool miss = hitStatus == HitStatus.Miss || hitStatus == HitStatus.MissClick;

            Transform deadNoteTransform = _notePrefab.transform;

            if (!miss)
                deadNoteTransform = _notes[0].transform;
            else
                deadNoteTransform.position = NotePositionScaler.Instance.GetLaneLerpPosition(_laneID, SessionValues.Instance.lerpAlphaHitThreshold + .5f);

            GameObject deadNote = Instantiate(SessionValues.Instance.deadNote);

            Sprite deathSprite = miss ? null : _notes[0].DeathSprite;
            deadNote.GetComponent<DeadNote>().SetDeadNoteValues(deathSprite, hitSprite, deadNoteTransform);

            if (miss)
                return;

            Note targetNote = _notes[0];
            _notes.Remove(targetNote);

            Destroy(targetNote.gameObject);
        }

        /// <summary>
        /// Spawns a note on this Lane with the passed TimeStamp and data.
        /// </summary>
        /// <param name="timeStamp">The TimeStamp that the new note must be hit on.</param>
        public void SpawnNote(float timeStamp)
        {
            GameObject newNoteObject = Instantiate(_notePrefab);

            Note newNote = newNoteObject.GetComponent<Note>();

            _notes.Add(newNote);

            newNote.SetNoteValues(
                this,
                _initialNotePosition, 
                _targetNotePosition, 
                _laneID, 
                SessionValues.Instance.currentLevel.levelID, 
                timeStamp
            );
        }

        /// <summary>
        /// Remove a note from the tracked notes list.
        /// </summary>
        /// <param name="note">The targeted note.</param>
        public void RemoveNote(Note note)
        {
            if(_notes.Contains(note))
                _notes.Remove(note);
        }

        private void OnDrawGizmos()
        {
            float total = _initialNotePosition.y + Mathf.Abs(_targetNotePosition.y);

            float targetHeight = total * SessionValues.Instance.lerpAlphaHitThreshold;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight, 0)
            );
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0)
            );

            targetHeight = total
                * SessionValues.Instance.lerpAlphaHitThreshold 
                * SessionValues.Instance.okThreshold;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight, 0)
            );
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0)
            );

            targetHeight = total
                * SessionValues.Instance.lerpAlphaHitThreshold 
                * SessionValues.Instance.alrightThreshold;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight, 0)
            );
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0)
            );

            targetHeight = total 
                * SessionValues.Instance.lerpAlphaHitThreshold 
                * SessionValues.Instance.niceThreshold;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight, 0)
            );
            Gizmos.DrawLine(
                new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), 
                new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0)
            );
        }
    }
}