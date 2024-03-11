using System;
using System.Collections;
using UnityEngine;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.PerformanceTracking;
using System.Linq;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    /// <summary>
    /// A class responsible for managing and sending relevant info to all lanes.
    /// </summary>
    public class LaneManager : AbstractSingleton<LaneManager>
    {
        [Header("References")]
        [SerializeField]
        private Lane[] _lanes;

        [Header("PC Controls")]
        [SerializeField]
        private bool _usingInputs;

        [SerializeField]
        private KeyCode[] _inputs;

        /// <summary>
        /// A status update for notes that are either being hit or have been missed.
        /// Hitstatus giving an idea of what happend and the integer being the lane ID of origin.
        /// </summary>
        public Action<HitStatus, int> OnNoteHit;

        private int _index;

        private void Awake() => Application.targetFrameRate = 60;

        private void Start() => OnNoteHit += RemoveNoteFromLane;

        private void RemoveNoteFromLane(HitStatus hitStatus, int laneID)
        {
            if (hitStatus == HitStatus.Miss || hitStatus == HitStatus.MissClick)
                return;

            Note target = _lanes[laneID].Notes[0];
            _lanes[laneID].Notes.Remove(target);

            target.HitNote();
        }

        public void DestroyAllNotes()
        {
            foreach(Lane lane in _lanes)
                lane.Notes.Clear();

            Note[] notes = FindObjectsOfType<Note>();

            foreach (Note note in notes)
                Destroy(note.gameObject);
        }

        /// <summary>
        /// Starts the level that is currently selected on the selected difficulty and song.
        /// </summary>
        public void PlayLevel() => StartCoroutine(PlayThroughLevel());

        private IEnumerator PlayThroughLevel()
        {
            _index = 0;

            PerformanceTracker.Instance.StartTracking();

            SessionValues.Instance.audioSource.clip = SessionValues.Instance.currentLevel.song;
            SessionValues.Instance.audioSource.Play();

            while (SessionValues.Instance.time < SessionValues.Instance.currentLevel.song.length)
            {
                if (SessionValues.Instance.paused)
                {
                    yield return null;
                    continue;
                }

                QueueUpcomingNotes();

                yield return null;
            }

            PerformanceTracker.Instance.CompleteTracking();
            yield return null;
        }

        private void QueueUpcomingNotes()
        {
            MappingData currentLevel = SessionValues.Instance.currentLevel.GetLevel();

            if (currentLevel.timestamps.Length <= _index)
                return;

            float upcomingTime = currentLevel.timestamps[_index];

            if (!SessionValues.Instance.IsTimeStampReadyForQueue(upcomingTime))
                return;

            int laneID = currentLevel.laneIDs[_index];
            _lanes[laneID].SpawnNote(upcomingTime);
            _index++;

            QueueUpcomingNotes();
        }

        /// <summary>
        /// Pauses the game based on the passed bool,
        /// this will be passed to the SessionValues and pause any playing audio.
        /// </summary>
        /// <param name="paused">The new paused value.</param>
        public void SetPaused(bool paused)
        {
            SessionValues.Instance.paused = paused;

            if (paused && SessionValues.Instance.audioSource.isPlaying)
                SessionValues.Instance.audioSource.Pause();
            else if (!paused && !SessionValues.Instance.audioSource.isPlaying)
                SessionValues.Instance.audioSource.UnPause();
        }

        private void Update()
        {
            if (!_usingInputs)
                return;

            int inputsLength = _inputs.Length;

            for (int i = 0; i < inputsLength; i++)
                if (Input.GetKeyDown(_inputs[i]))
                    _lanes[i].OnButtonPressed();
        }
    }
}
