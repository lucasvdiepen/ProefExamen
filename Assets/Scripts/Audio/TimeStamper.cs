using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TimeStamper : MonoBehaviour
{
    [SerializeField] private Color _timeStampColor = Color.blue;
    [SerializeField] private Color _selectedTimeStampColor = Color.yellow;
    [SerializeField] private float _stampLineHeightReduction = 100;
    [SerializeField] private float _gizmoOffset = .25f;

    [Header("Input KeyCodes")]
    [SerializeField] private KeyCode _timeStampInputKey;
    [SerializeField] private KeyCode _undoTimeStampKey;

    [SerializeField] private List<TimeStampData> _timeStamps = new List<TimeStampData>();
    
    private AudioWaveformDrawer _waveformDrawer = null;
    private TimeStampData _currentSelectedTimeStamp;

    [System.Serializable]
    public class TimeStampData
    {
        public Vector2 startPointPosition;
        public Vector2 endPointPosition;
        public float songTime;
        public bool isSelected = false;

        public TimeStampData(Vector2 start, Vector2 end, float time)
        {
            startPointPosition = start;
            endPointPosition = end;
            songTime = time;
        }
    }

    private void Awake()
    {
        _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();   
    }

    private void Update()
    {
        if (!_waveformDrawer.isCurrentlyPlaying)
            return;

        if (Input.GetKeyDown(_timeStampInputKey))
        {
            float startYPos = _waveformDrawer.cursor.position.y - (_waveformDrawer.cursor.localScale.y * .5f);
            Vector2 startPosition = new Vector2(_waveformDrawer.cursor.position.x, startYPos);
            Vector2 endPosition = new Vector2(_waveformDrawer.cursor.position.x, -_stampLineHeightReduction);

            _timeStamps.Add(new TimeStampData(startPosition, endPosition, _waveformDrawer.currentSongTime));
        }
        
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_undoTimeStampKey))
        {
            if(_timeStamps.Count > 0)
                _timeStamps.RemoveAt(_timeStamps.Count - 1);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TimeStampData closestStampToMouse = GetClosestTimeStamp(mousePosition);

            if(closestStampToMouse != _currentSelectedTimeStamp)
            {
                closestStampToMouse.isSelected = true;
                if(_currentSelectedTimeStamp != null)
                    _currentSelectedTimeStamp.isSelected = false;
                _currentSelectedTimeStamp = closestStampToMouse;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _currentSelectedTimeStamp.isSelected = false;
            _currentSelectedTimeStamp = null;
        }
    }

    TimeStampData GetClosestTimeStamp(Vector2 originPosition)
    {
        TimeStampData bestTarget = default;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (TimeStampData timeStampData in _timeStamps)
        {
            Vector3 directionToTarget = timeStampData.startPointPosition - originPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = timeStampData;
            }
        }

        return bestTarget;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _timeStamps.Count; i++)
        {
            //Little hacky but SOMETIME I HATE UNITY, you can't set the thickness of the gizmos line. 
            //This fixes the gizmo flickering when it's ony 1px wide.

            Gizmos.color = _timeStamps[i].isSelected ? _selectedTimeStampColor : _timeStampColor;
            Vector2 offset = new Vector2(_gizmoOffset, 0);

            Gizmos.DrawLine(_timeStamps[i].startPointPosition, _timeStamps[i].endPointPosition); //center
            Gizmos.DrawLine(_timeStamps[i].startPointPosition - offset, _timeStamps[i].endPointPosition - offset); //left
            Gizmos.DrawLine(_timeStamps[i].startPointPosition + offset, _timeStamps[i].endPointPosition + offset); //right
        }
    }
}
