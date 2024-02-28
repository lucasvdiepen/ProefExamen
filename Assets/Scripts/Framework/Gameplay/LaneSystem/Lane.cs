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
        private int _id = -1;

        [Header("References")]
        [SerializeField]
        private Button _button;

        [SerializeField]
        private List<GameObject> _notes = new();

        [Header("The minimum and maximum the lerp alpha can be to hit the note.")]
        [SerializeField]
        private float _lerpAlphaRangeMinimum = .4f;

        [SerializeField]
        private float _lerpAlphaRangeMaximum = .7f;

        [Header("Lerp positions")]
        [SerializeField]
        private Vector3 _initialNotePosition;

        [SerializeField]
        private Vector3 _targetNotePosition;

        /// <summary>
        /// Gets the private button variable of the Lane.
        /// </summary>
        public Button Button => _button;

        private void Start()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            if (_id != -1)
                _button.onClick.AddListener(SendButtonPress);
            else
                Debug.LogError("Lane with button " + _button.name + " has no assigned ID!");
        }

        private void SendButtonPress()
        {
            if (!(_notes.Count > 0))
            {
                SessionValues.score -= (int)HitStatus.Alright;
                return;
            }

            GameObject nextNote = _notes[0];
            Note nextNoteScript = nextNote.GetComponent<Note>();

            if (nextNoteScript.LerpAlpha < _lerpAlphaRangeMinimum && nextNoteScript.LerpAlpha > _lerpAlphaRangeMaximum)
                return;

            float differenceAlpha = Mathf.Abs(nextNoteScript.LerpAlpha - .5f) / .2f;
            HitStatus hitResult = LaneUtils.ReturnHitStatus(differenceAlpha);

            SessionValues.score += (int)hitResult;

            Destroy(nextNote);
            RemoveNote(nextNote);
        }

        /// <summary>
        /// Spawns a note on this Lane with the passed TimeStamp and data.
        /// </summary>
        /// <param name="timeStamp">The TimeStamp that the new note must be hit on.</param>
        public void SpawnNote(float timeStamp)
        {
            GameObject newNote = Instantiate(SessionValues.note);
            _notes.Add(newNote);

            Note newNoteScript = newNote.GetComponent<Note>();

            newNoteScript.SetNoteValues(_initialNotePosition, _targetNotePosition, _id, SessionValues.currentLevel.levelID, timeStamp);
            newNoteScript.CallNoteRemoval += RemoveNote;
        }

        /// <summary>
        /// Remove a note from the tracked notes list.
        /// </summary>
        /// <param name="note">The targeted note.</param>
        public void RemoveNote(GameObject note) => _notes.Remove(note);
    }
}