using System.Collections;
using UnityEngine;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.MapData;
using System;

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
        public Action<HitStatus, int> NoteStatusUpdate;

        private int _index;

        /// <summary>
        /// Starts the level that is currently selected on the selected difficulty and song.
        /// </summary>
        /// <returns></returns>

        public void Start()
        {
            SessionValues.SelectLevel(2);
            StartCoroutine(PlayThroughLevel());
        }

        public IEnumerator PlayThroughLevel()
        {
            _index = 0;

            _audio.clip = SessionValues.currentLevel.song;
            _audio.Play();
            Debug.Log("starting level");
            while (SessionValues.time < SessionValues.currentLevel.song.length)
            {
                if (SessionValues.paused)
                {
                    yield return null;
                    continue;
                }
                Debug.Log("playing");
                SessionValues.time += Time.deltaTime;

                QueueUpcomingNotes();

                yield return null;
            }

            yield return null;
        }

        private void QueueUpcomingNotes()
        {
            Level currentLevel = SessionValues.currentLevel.GetLevel();

            if (currentLevel.timestamps.Length <= _index)
                return;

            float upcomingTime = currentLevel.timestamps[_index];

            if (!SessionValues.IsTimeStampReadyForQueue(upcomingTime))
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
        public void SetNewPaused(bool paused)
        {
            SessionValues.paused = paused;

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