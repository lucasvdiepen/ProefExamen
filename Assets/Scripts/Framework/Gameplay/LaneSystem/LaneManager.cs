using System.Collections;
using UnityEngine;
using System;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.MapData;

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

        [SerializeField]
        private AudioSource _audio;

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

        private void Start()
        {
            OnNoteHit += RemoveNoteFromLane;

            SessionValues.Instance.SelectLevel(2);
            StartCoroutine(PlayThroughLevel());
        }

        private void RemoveNoteFromLane(HitStatus hitStatus, int laneID)
        {
            if (hitStatus != HitStatus.Miss)
                _lanes[laneID].Notes.RemoveAt(0);

            SessionValues.Instance.score += (int)hitStatus;
        }

        /// <summary>
        /// Starts the level that is currently selected on the selected difficulty and song.
        /// </summary>
        public IEnumerator PlayThroughLevel()
        {
            _index = 0;

            _audio.clip = SessionValues.Instance.currentLevel.song;
            _audio.Play();
            Debug.Log("starting level");
            while (SessionValues.Instance.time < SessionValues.Instance.currentLevel.song.length)
            {
                if (SessionValues.Instance.paused)
                {
                    yield return null;
                    continue;
                }
                Debug.Log("playing");
                SessionValues.Instance.time += Time.deltaTime;

                QueueUpcomingNotes();

                yield return null;
            }

            yield return null;
        }

        private void QueueUpcomingNotes()
        {
            Level currentLevel = SessionValues.Instance.currentLevel.GetLevel();

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

            if (paused && _audio.isPlaying)
                _audio.Pause();
            else if (!paused && !_audio.isPlaying)
                _audio.UnPause();
        }

        private void Update()
        {
            int inputsLength = _inputs.Length;

            for (int i = 0; i < inputsLength; i++)
                if (Input.GetKeyDown(_inputs[i]))
                    _lanes[i].Button.onClick?.Invoke();
        }
    }
}