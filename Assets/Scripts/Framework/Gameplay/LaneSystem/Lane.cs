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
        private Button _button;

        [SerializeField]
        private List<Note> _notes = new();

        [Header("Lerp positions")]
        [SerializeField]
        private Vector3 _initialNotePosition;

        [SerializeField]
        private Vector3 _targetNotePosition;

        /// <summary>
        /// Gets the private button component of the Lane.
        /// </summary>
        public Button Button => _button;

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (_laneID == -1)
            {
                Debug.LogError("Lane with button " + _button.name + " has no assigned ID!");
                return;
            }

            _button.onClick.AddListener(SendButtonPress);
        }

        private void SendButtonPress()
        {
            HitStatus hitResult;

            if (_notes.Count == 0)
                hitResult = HitStatus.Miss;

            else
            {
                Note nextNote = _notes[0];
                hitResult = LaneUtils.CalculateHitStatus(nextNote.LerpAlpha);

                Destroy(nextNote.gameObject);
                RemoveNote(nextNote);
            }

            LaneManager.Instance.NoteStatusUpdate?.Invoke(hitResult, _laneID);
        }

        /// <summary>
        /// Spawns a note on this Lane with the passed TimeStamp and data.
        /// </summary>
        /// <param name="timeStamp">The TimeStamp that the new note must be hit on.</param>
        public void SpawnNote(float timeStamp)
        {
            GameObject newNoteObject = Instantiate(SessionValues.note);

            Note newNote = newNoteObject.GetComponent<Note>();

            _notes.Add(newNote);

            newNote.SetNoteValues(_initialNotePosition, _targetNotePosition, _laneID, SessionValues.currentLevel.levelID, timeStamp);
            newNote.CallNoteRemoval += RemoveNote;
        }

        /// <summary>
        /// Remove a note from the tracked notes list.
        /// </summary>
        /// <param name="note">The targeted note.</param>
        public void RemoveNote(Note note) => _notes.Remove(note);

        private void OnDrawGizmos()
        {
            float total = _initialNotePosition.y + Mathf.Abs(_targetNotePosition.y);

            float targetHeight = total * SessionValuesShortcut.Instance._alphaLerpHitThreshold;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight, 0));
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0));

            targetHeight = total * (SessionValuesShortcut.Instance._alphaLerpHitThreshold * SessionValuesShortcut.Instance._okThreshold);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight, 0));
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0));

            targetHeight = total * (SessionValuesShortcut.Instance._alphaLerpHitThreshold * SessionValuesShortcut.Instance._alrightThreshold);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight, 0));
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0));

            targetHeight = total * (SessionValuesShortcut.Instance._alphaLerpHitThreshold * SessionValuesShortcut.Instance._niceThreshold);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight, 0));
            Gizmos.DrawLine(new Vector3(_initialNotePosition.x - .4f, targetHeight * -1, 0), new Vector3(_initialNotePosition.x + .4f, targetHeight * -1, 0));
        }
    }
}