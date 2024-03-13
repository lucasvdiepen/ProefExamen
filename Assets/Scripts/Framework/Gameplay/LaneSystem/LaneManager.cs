using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.PerformanceTracking;
using UnityEditor.Search;
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

        [Header("Note Hit Sprites")]
        [SerializeField]
        private Sprite _perfectHitSprite;

        [SerializeField]
        private Sprite _niceHitSprite;

        [SerializeField]
        private Sprite _alrightHitSprite;

        [SerializeField]
        private Sprite _okHitSprite;

        [SerializeField]
        private Sprite _missSprite;

        [Header("PC Controls")]
        [SerializeField]
        private bool _usingInputs;

        [SerializeField]
        private KeyCode[] _inputs;

        [SerializeField]
        private float _minimumPauseTime;

        /// <summary>
        /// A bool that checks if the level is being beatmapped or not.
        /// </summary>
        [field: SerializeField]
        public bool IsBeatMapping { get; set; }

        /// <summary>
        /// The lane ID for each live timestamp that the notes have to be spawned on.
        /// </summary>
        public List<Tuple<float, int>> liveTimeStamps;

        /// <summary>
        /// A status update for notes that are either being hit or have been missed.
        /// Hitstatus giving an idea of what happend and the integer being the lane ID of origin.
        /// </summary>
        public Action<HitStatus, int> OnNoteHit;

        /// <summary>
        /// The index of the current shown time stamp.
        /// </summary>
        [field:SerializeField]
        public int Index { get; set; }
        
        /// <summary>
        /// Return the LaneManager's managed lanes.
        /// </summary>
        public Lane[] Lanes => _lanes;

        /// <summary>
        /// Whether or not the player has watched an ad.
        /// </summary>
        public bool HasWatchedAd { get; set; }

        private Note[] _lastPausedNotes;
        private DeadNote[] _lastPausedDeadNotes;

        private void Awake() => Application.targetFrameRate = 60;

        private void Start()
        {
            OnNoteHit += RemoveNoteFromLane;

            if (!IsBeatMapping)
                return;

            liveTimeStamps = new();

            StartCoroutine(PlayThroughLevel());
        }

        private void RemoveNoteFromLane(HitStatus hitStatus, int laneID)
        {
            Sprite targetSprite = null;

            switch (hitStatus)
            {
                case HitStatus.Miss:
                    targetSprite = _missSprite;
                    break;
                case HitStatus.MissClick:
                    targetSprite = _missSprite;
                    break;
                case HitStatus.Ok:
                    targetSprite = _okHitSprite;
                    break;
                case HitStatus.Alright:
                    targetSprite = _alrightHitSprite;
                    break;
                case HitStatus.Nice:
                    targetSprite = _niceHitSprite;
                    break;
                case HitStatus.Perfect:
                    targetSprite = _perfectHitSprite;
                    break;
            }

            _lanes[laneID].HitNote(hitStatus, targetSprite);
        }

        /// <summary>
        /// Destroys all notes in the scene.
        /// </summary>
        public void DestroyAllNotes()
        {
            foreach(Lane lane in _lanes)
                lane.Notes.Clear();

            Note[] notes = FindObjectsOfType<Note>();

            foreach (Note note in notes)
                Destroy(note.gameObject);

            DeadNote[] deadNotes = FindObjectsOfType<DeadNote>();

            foreach (DeadNote deadNote in deadNotes)
                Destroy(deadNote.gameObject);
        }

        /// <summary>
        /// Starts the level that is currently selected on the selected difficulty and song.
        /// </summary>
        public void PlayLevel()
        {
            HasWatchedAd = false;

            if(SessionValues.Instance.audioSource.clip != null)
            {
                SessionValues.Instance.audioSource.time = 0;
                SessionValues.Instance.paused = false;
            }

            StartCoroutine(PlayThroughLevel());
        }

        private IEnumerator PlayThroughLevel()
        {
            SessionValues sessionValues = SessionValues.Instance;

            Index = 0;
            sessionValues.startTimer = -1 * sessionValues.travelTime;
            _minimumPauseTime = sessionValues.startTimer;

            bool hasStartedSong = false;

            PerformanceTracker.Instance.StartTracking();

            if (!IsBeatMapping)
                sessionValues.audioSource.clip = sessionValues.currentLevel.song;

            while (IsBeatMapping || sessionValues.time < sessionValues.currentLevel.song.length)
            {
                if (sessionValues.paused)
                {
                    yield return null;
                    continue;
                }

                if (sessionValues.startTimer < 0)
                {
                    sessionValues.startTimer = Math.Clamp
                    (
                        sessionValues.startTimer + Time.deltaTime,
                        -1 * sessionValues.travelTime,
                        0
                    );
                }
                else if (!hasStartedSong && !sessionValues.audioSource.isPlaying)
                {
                    hasStartedSong = true;
                    sessionValues.audioSource.Play();
                }

                QueueUpcomingNotes();

                yield return null;
            }

            PerformanceTracker.Instance.CompleteTracking();
            yield return null;
        }

        private void QueueUpcomingNotes()
        {
            if (IsBeatMapping)
            {
                QueueUpcomingBeatMappingNotes();
                return;
            }

            MappingData currentLevel = SessionValues.Instance.currentLevel.GetLevel();

            if (currentLevel.timeStamps.Length <= Index)
                return;

            float upcomingTime = currentLevel.timeStamps[Index];

            if (!SessionValues.Instance.IsTimeStampReadyForQueue(upcomingTime))
                return;

            int laneID = currentLevel.laneIDs[Index];

            _lanes[laneID].SpawnNote(upcomingTime);
            Index++;

            QueueUpcomingNotes();
        }

        private void QueueUpcomingBeatMappingNotes()
        {
            if (liveTimeStamps.Count <= Index)
                return;

            if (liveTimeStamps.Count == 0)
                return;

            if (Index == -1)
                Index = 0;

            float upcomingTime = liveTimeStamps[Index].Item1;

            if (!SessionValues.Instance.IsLiveTimeStampReadyForQueue(upcomingTime))
                return;

            int laneID = liveTimeStamps[Index].Item2;

            _lanes[laneID].SpawnNote(upcomingTime);
            Index++;

            QueueUpcomingBeatMappingNotes();
        }

        /// <summary>
        /// Pauses the game based on the passed bool,
        /// this will be passed to the SessionValues and pause any playing audio.
        /// </summary>
        /// <param name="paused">The new paused value.</param>
        public void SetPaused(bool paused)
        {
            SessionValues.Instance.paused = paused;
            SetNotesActive(!paused);

            if (paused && SessionValues.Instance.audioSource.isPlaying)
                SessionValues.Instance.audioSource.Pause();
            else if (!paused && !SessionValues.Instance.audioSource.isPlaying)
            {
                _minimumPauseTime = Math.Clamp
                (
                    SessionValues.Instance.time - SessionValues.Instance.travelTime, 
                    _minimumPauseTime, 
                    SessionValues.Instance.audioSource.clip.length
                );

                if (_minimumPauseTime < 0)
                    SessionValues.Instance.startTimer = _minimumPauseTime;
                else
                    SessionValues.Instance.audioSource.time = _minimumPauseTime;

                SessionValues.Instance.audioSource.UnPause();
            }
        }

        private void SetNotesActive(bool notesActive)
        {
            Note[] allNotes = notesActive
                ? FindObjectsOfType<Note>().Concat(_lastPausedNotes).ToArray()
                : FindObjectsOfType<Note>();

            foreach (Note note in allNotes)
            {
                if (note == null)
                    continue;

                note.gameObject.SetActive(notesActive);
            }

            DeadNote[] allDeadNotes = notesActive
                ? FindObjectsOfType<DeadNote>().Concat(_lastPausedDeadNotes).ToArray()
                : FindObjectsOfType<DeadNote>();
            foreach (DeadNote deadNote in allDeadNotes)
            {
                if (deadNote == null)
                    continue;

                deadNote.gameObject.SetActive(notesActive);
            }

            if (notesActive)
                return;

            _lastPausedDeadNotes = allDeadNotes;
            _lastPausedNotes = allNotes;
        }

        private void Update()
        {
            if (!_usingInputs || IsBeatMapping)
                return;

            int inputsLength = _inputs.Length;

            for (int i = 0; i < inputsLength; i++)
                if (Input.GetKeyDown(_inputs[i]))
                    _lanes[i].OnButtonPressed();
        }
    }
}
