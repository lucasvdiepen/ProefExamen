using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField]
    private float _timeStamp = 0;

    [SerializeField]
    private int _laneID = -1;

    [SerializeField]
    private int _levelID = -1;

    [SerializeField]
    private Vector2 _initialPosition;

    [SerializeField]
    private Vector2 _targetPosition;

    public void SetNoteValues(Vector2 initialPosition, Vector2 targetPosition, int laneID, int levelID, float timeStamp)
    {
        _initialPosition = initialPosition;
        _targetPosition = targetPosition;
        _laneID = laneID;
        _levelID = levelID;
        _timeStamp = timeStamp;
    }

    public void Update()
    {
        if(!Settings.paused && _laneID != -1)
        {
            float diff = (_timeStamp - Settings.time) / Settings.travelTime;

            if (diff <= 1)
            {
                transform.position = Vector3.Lerp(_initialPosition, _targetPosition, diff);
            }
            else
                transform.position = _initialPosition;
        }
    }
}
