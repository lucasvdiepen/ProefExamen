using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using ProefExamen.Framework.Utils;

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
    private GameObject _note;

    public void Start()
    {
        Settings.note = _note;
        StartCoroutine(delayFunction());
    }

    public IEnumerator delayFunction()
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
                Settings.currentLevel = level;
                StartCoroutine(PlayThroughLevel());
            }
        }
    }

    public IEnumerator PlayThroughLevel()
    {
        Debug.Log("starting playthrough");
        while (Settings.time < Settings.currentLevel.song.length)
        {
            if (!Settings.paused)
            {
                Debug.Log("burh");
                Settings.time += Time.deltaTime;

                QueueNotesForUpcomingSeconds();

                yield return null;
            }
            else
                yield return null;
        }
    }

    private void QueueNotesForUpcomingSeconds()
    {
        if (Settings.currentLevel.timestamps.Length <= _index) return;

        float upcomingTime = Settings.currentLevel.timestamps[_index];

        if (Settings.TimeStampReadyForQueue(upcomingTime))
        {
            int laneID = Settings.currentLevel.laneIDs[_index];

            _lanes[laneID].SpawnNote(upcomingTime);

            _index++;
            QueueNotesForUpcomingSeconds();
        }
    }

    public void SetNewPaused(bool paused)
    {
        Settings.paused = paused;
    }
}
