using System.Collections;
using UnityEngine;

using ProefExamen.Framework.Utils;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.MapData;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    public class LaneManager : AbstractSingleton<LaneManager>
    {
        [SerializeField]
        private Lane[] _lanes;

        [SerializeField]
        private int _index;

        [SerializeField]
        private Levels _levels;

        [SerializeField]
        private int _selectedLevelID;

        [SerializeField]
        private AudioSource _currentSong;

        public void Start()
        {
            SessionValues.difficulty = Difficulty.EASY;
            //temp code
            StartCoroutine(delayFunction());
        }

        public IEnumerator delayFunction() // temp function
        {
            yield return new WaitForSeconds(2f);
            SelectLevel();
        }
        public void SelectLevel()
        {
            Debug.Log("SelectLevel called");
            foreach (LevelData level in _levels.levels)
            {
                if (level.levelID == _selectedLevelID)
                {
                    SessionValues.currentLevel = level;
                    StartCoroutine(PlayThroughLevel());
                }
            }
        }

        public IEnumerator PlayThroughLevel()
        {
            _index = 0;

            _currentSong.clip = SessionValues.currentLevel.song;
            _currentSong.Play();

            Debug.Log("starting playthrough");
            while (SessionValues.time < SessionValues.currentLevel.song.length)
            {
                if (!SessionValues.paused)
                {
                    SessionValues.time += Time.deltaTime;

                    QueueNotesForUpcomingSeconds();

                    yield return null;
                }
                else
                    yield return null;
            }
            yield return null;
        }

        private void QueueNotesForUpcomingSeconds()
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
                QueueNotesForUpcomingSeconds();
            }
        }

        public void SetNewPaused(bool paused)
        {
            SessionValues.paused = paused;

            if (paused && _currentSong.isPlaying)
                _currentSong.Pause();
            else if (!paused && !_currentSong.isPlaying)
                _currentSong.UnPause();
        }

        private void Update()
        {
            for (int i = 0; i < SessionValues.inputs.Length; i++)
            {
                if (Input.GetKeyDown(SessionValues.inputs[i]))
                    _lanes[i].Button.onClick?.Invoke();
            }
        }
    }
}