using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeStamper : MonoBehaviour
{
    [SerializeField] private Color _timeStampColor = Color.blue;
    [SerializeField] private Color _selectedTimeStampColor = Color.yellow;
    [SerializeField] private float _stampLineHeightReduction = 100;
    [SerializeField] private float _gizmoOffset = .25f;

    [Header("Input KeyCodes")]
    [SerializeField] private KeyCode _placeTimeStampKey;
    [SerializeField] private KeyCode _undoTimeStampKey;
    [SerializeField] private KeyCode _deleteCurrentTimeStampKey;
    [SerializeField] private KeyCode _exportTimeStampsKey;

    [SerializeField] private List<TimeStampData> _timeStamps = new List<TimeStampData>();
    
    private AudioWaveformDrawer _waveformDrawer = null;
    private TimeStampData _currentSelectedTimeStamp = null;
    private GUIStyle _debugBoldGuiStyle = new();
    private GUIStyle _debugItalicsGuiStyle = new();
    private string _assetPath => "Assets/" + $"{_waveformDrawer.currentSongTitle}.asset";

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
        
        _debugBoldGuiStyle.fontSize = 48;
        _debugBoldGuiStyle.fontStyle = FontStyle.Bold;
        _debugBoldGuiStyle.normal.textColor = Color.white;

        _debugItalicsGuiStyle.fontSize = 48;
        _debugItalicsGuiStyle.fontStyle = FontStyle.Italic;
        _debugItalicsGuiStyle.normal.textColor = Color.white;
    }

    private void Update()
    {
        if (!_waveformDrawer.HasActiveWaveform)
            return;

        HandleAudioControls();
        HandleTimeStampSelection();
    }
   
    private void HandleAudioControls()
    {
        if (Input.GetKeyDown(_placeTimeStampKey))
        {
            float startYPos = _waveformDrawer.cursor.position.y - (_waveformDrawer.cursor.localScale.y * .5f);
            Vector2 startPosition = new Vector2(_waveformDrawer.cursor.position.x, startYPos);
            Vector2 endPosition = new Vector2(_waveformDrawer.cursor.position.x, -_stampLineHeightReduction);

            _timeStamps.Add(new TimeStampData(startPosition, endPosition, _waveformDrawer.currentSongTime));
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_undoTimeStampKey))
        {
            if (_timeStamps.Count > 0)
                _timeStamps.RemoveAt(_timeStamps.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) //pausing song
        {
            if (_waveformDrawer.audioSource.isPlaying) _waveformDrawer.audioSource.Pause();
            else _waveformDrawer.audioSource.UnPause();
        }

        //deleting selected time stamp
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_deleteCurrentTimeStampKey))
        {
            if (_currentSelectedTimeStamp != null)
            {
                TimeStampData timeStampToDelete = _currentSelectedTimeStamp;
                _currentSelectedTimeStamp = null;
                _timeStamps.Remove(timeStampToDelete);
            }
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_exportTimeStampsKey))
            TryExportTimeStamps();
    }
#endif

    private void HandleTimeStampSelection()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TimeStampData closestStampToMouse = GetClosestTimeStamp(mousePosition);

            if (closestStampToMouse == null)
                return;

            if (closestStampToMouse != _currentSelectedTimeStamp)
            {
                closestStampToMouse.isSelected = true;
                if (_currentSelectedTimeStamp != null)
                    _currentSelectedTimeStamp.isSelected = false;
                _currentSelectedTimeStamp = closestStampToMouse;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && _currentSelectedTimeStamp != null)
        {
            _currentSelectedTimeStamp.isSelected = false;
            _currentSelectedTimeStamp = null;
        }

        if (_currentSelectedTimeStamp != null)
        {
            Vector2 newDirection = Vector2.right * Input.mouseScrollDelta.y * .5f;
            _currentSelectedTimeStamp.startPointPosition += newDirection;
            _currentSelectedTimeStamp.endPointPosition += newDirection;

            _currentSelectedTimeStamp.songTime =
                _waveformDrawer.CalculateSongTimeBasedOnPosition(_currentSelectedTimeStamp.startPointPosition);
        }
    }

    /// <summary>
    /// Returns the closest time stamp based on input origin position
    /// </summary>
    /// <param name="originPosition">Origin of the distance check</param>
    /// <returns></returns>
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

#if UNITY_EDITOR
    private void TryExportTimeStamps()
    {
        var obj = ScriptableObject.CreateInstance<SongTimeStamps>();
        float[] exportedTimeStamps = new float[_timeStamps.Count];

        for (int i = 0; i < exportedTimeStamps.Length; i++)
            exportedTimeStamps[i] = _timeStamps[i].songTime;

        obj.timeStamps = exportedTimeStamps;
        UnityEditor.AssetDatabase.CreateAsset(obj, _assetPath);

        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();

        print("Succesfully exported timestamps");
    }
#endif

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

    private void OnGUI()
    {
        GUI.color = Color.white;
        GUI.Label(new Rect(0, 0, 300, 100), $"Playback Speed: {_waveformDrawer.currentPlayBackSpeed}", _debugBoldGuiStyle);
        GUI.Label(new Rect(0, 48, 300, 100), $"Song Time: {_waveformDrawer.currentSongTime}", _debugBoldGuiStyle);

        if(!_waveformDrawer.audioSource.isPlaying && _waveformDrawer.HasActiveWaveform)
            GUI.Label(new Rect(1750, 0, 300, 100), "Paused", _debugItalicsGuiStyle);

        if (_currentSelectedTimeStamp != null)
            GUI.Label(new Rect(0, 96, 300, 100), $"TimeStamp Time: {_currentSelectedTimeStamp.songTime}", _debugItalicsGuiStyle);
    }
}
