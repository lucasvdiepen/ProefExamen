using System;
using System.Collections;
using UnityEngine;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.PerformanceTracking;

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
        /// A bool that checks if the level is being beatmapped or not.
        /// </summary>
        [field: SerializeField]
        public bool IsBeatMapping { get; set; }

        /// <summary>
        /// A status update for notes that are either being hit or have been missed.
        /// Hitstatus giving an idea of what happend and the integer being the lane ID of origin.
        /// </summary>
        public Action<HitStatus, int> OnNoteHit;

        /// <summary>
        /// The index of the current shown time stamp.
        /// </summary>
        public int Index { get; set; }

        private void Awake() => Application.targetFrameRate = 60;

        private void Start()
        {
            OnNoteHit += RemoveNoteFromLane;

            SessionValues.Instance.SelectLevel(SessionValues.Instance.currentLevelID);

            // Clear the liveTimeStamps list.
            var level = SessionValues.Instance.currentLevel.GetLevel();
            level.liveTimeStamps = new();
            SessionValues.Instance.currentLevel.mappingData[0] = level;

            StartCoroutine(PlayThroughLevel());
        }

        private void RemoveNoteFromLane(HitStatus hitStatus, int laneID)
        {
            if (hitStatus == HitStatus.Miss)
                return;

            Note target = _lanes[laneID].Notes[0];
            _lanes[laneID].Notes.Remove(target);

            target.HitNote();
        }

        /// <summary>
        /// Starts the level that is currently selected on the selected difficulty and song.
        /// </summary>
        public IEnumerator PlayThroughLevel()
        {
            Index = 0;

            PerformanceTracker.Instance.StartTracking();

            if (!IsBeatMapping)
            {
                SessionValues.Instance.audioSource.clip = SessionValues.Instance.currentLevel.song;
                SessionValues.Instance.audioSource.Play();
            }

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

            if (currentLevel.liveTimeStamps.Count <= Index)
                return;

            if (currentLevel.liveTimeStamps.Count == 0)
                return;

            if (Index == -1)
                Index = 0;

            float upcomingTime = currentLevel.liveTimeStamps[Index].Item1;
            if (!SessionValues.Instance.IsLiveTimeStampReadyForQueue(upcomingTime))
                return;

            int laneID = currentLevel.liveTimeStamps[Index].Item2;

            _lanes[laneID].SpawnNote(upcomingTime);
            Index++;

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
            int inputsLength = _inputs.Length;

            for (int i = 0; i < inputsLength; i++)
                if (Input.GetKeyDown(_inputs[i]))
                    _lanes[i].OnButtonPressed();
        }
    }
}
