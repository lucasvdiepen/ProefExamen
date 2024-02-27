using System.Collections;
using UnityEngine;

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
        [SerializeField]
        private Lane[] _lanes;

        [SerializeField]
        private AudioSource _audio;

        [Header("PC Controls")]
        [SerializeField]
        private bool _usingInputs;

        [SerializeField]
        private KeyCode[] _inputs;

        private int _index;

        /// <summary>
        /// Starts the level that is currently selected on the selected difficulty and song.
        /// </summary>
        /// <returns></returns>
        public IEnumerator PlayThroughLevel()
        {
            _index = 0;

            _audio.clip = SessionValues.currentLevel.song;
            _audio.Play();

            while (SessionValues.time < SessionValues.currentLevel.song.length)
            {
                if (!SessionValues.paused)
                {
                    SessionValues.time += Time.deltaTime;

                    QueueUpcomingNotes();

                    yield return null;
                }
                else
                    yield return null;
            }
            yield return null;
        }

        private void QueueUpcomingNotes()
        {
            Level currentLevel = SessionValues.currentLevel.Level();

            if (currentLevel.timestamps.Length <= _index)
                return;

            float upcomingTime = currentLevel.timestamps[_index];

            if (SessionValues.TimeStampReadyForQueue(upcomingTime))
            {
                int laneID = currentLevel.laneIDs[_index];

                _lanes[laneID].SpawnNote(upcomingTime);

                _index++;
                QueueUpcomingNotes();
            }
        }

        /// <summary>
        /// Pauses the game based on the passed bool, 
        /// this will be passed to the SessionValues and pause any playing audio.
        /// </summary>
        /// <param name="paused"></param>
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