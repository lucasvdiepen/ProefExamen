using ProefExamen.Audio.WaveFormDrawer;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProefExamen.Audio.TimeStamping
{
    public class TimeStamper : MonoBehaviour
    {
        [SerializeField]
        private Color _timeStampColor = Color.blue;

        [SerializeField]
        private Color _selectedTimeStampColor = Color.yellow;

        [SerializeField]
        private float _stampLineHeightReduction = 100;

        [SerializeField]
        private float _timeStampTweakAmount = .5f;

        [SerializeField]
        private float _gizmoSpacing = .25f;

        [Header("Input KeyCodes")]
        [SerializeField]
        private KeyCode _placeTimeStampKey;

        [SerializeField]
        private KeyCode _undoTimeStampKey;

        [SerializeField]
        private KeyCode _deleteCurrentTimeStampKey;

        [SerializeField]
        private KeyCode _exportTimeStampsKey;

        [SerializeField]
        private KeyCode _increaseTimeStampKey;

        [SerializeField]
        private KeyCode _decreaseTimeStampKey;

        [SerializeField]
        private List<TimeStampData> _timeStamps = new();

        private AudioWaveformDrawer _waveformDrawer = null;
        private TimeStampData _currentSelectedTimeStamp = null;

        private readonly GUIStyle _debugBoldGuiStyle = new();
        private readonly GUIStyle _debugItalicsGuiStyle = new();

        private string _assetPath => "Assets/TimeStampOutput" + $"{_waveformDrawer.currentSongTitle}.asset";

        /// <summary>
        /// Class responsible for holding the necessary data for a time stamp.
        /// </summary>
        [System.Serializable]
        public class TimeStampData
        {
            /// <summary>
            /// Start line point used for gizmo drawing.
            /// </summary>
            public Vector2 startPointPosition;

            /// <summary>
            /// End line point used for gizmo drawing.
            /// </summary>
            public Vector2 endPointPosition;

            /// <summary>
            /// Holds the actual song time of the time stamp.
            /// </summary>
            public float songTime;

            /// <summary>
            /// Returns if this time stamp is selected.
            /// </summary>
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
            if (!_waveformDrawer.hasActiveWaveform)
                return;

            HandleTimeStampControls();
            HandleTimeStampSelection();
        }

        /// <summary>
        /// Handles all input checks for time stamping.
        /// </summary>
        private void HandleTimeStampControls()
        {
            if (Input.GetKeyDown(_placeTimeStampKey))
            {
                float startYPos = _waveformDrawer.cursor.position.y - (_waveformDrawer.cursor.localScale.y * .5f);
                Vector2 startPosition = new Vector2(_waveformDrawer.cursor.position.x, startYPos);
                Vector2 endPosition = new Vector2(_waveformDrawer.cursor.position.x, -_stampLineHeightReduction);

                _timeStamps.Add(new TimeStampData(startPosition, endPosition, _waveformDrawer.currentSongTime));
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_undoTimeStampKey)) //undo last time stamp
            {
                if (_timeStamps.Count > 0)
                    _timeStamps.RemoveAt(_timeStamps.Count - 1);
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

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_exportTimeStampsKey))
                TryExportTimeStamps();
        }

        /// <summary>
        /// Handles logic for selecting specific time stamps.
        /// </summary>
        private void HandleTimeStampSelection()
        {
            if (Input.GetKey(KeyCode.LeftControl)) //try select closest time stamp to the mouse position
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

            //when releasing ctrl, remove current time stamp selection
            if (Input.GetKeyUp(KeyCode.LeftControl) && _currentSelectedTimeStamp != null)
            {
                _currentSelectedTimeStamp.isSelected = false;
                _currentSelectedTimeStamp = null;
            }

            //tweak current selected time stamp if it is not null
            if (_currentSelectedTimeStamp != null)
            {
                Vector2 newDirection = Vector2.zero;
                if (Input.mouseScrollDelta.magnitude > 0)
                    newDirection = Vector2.right * Input.mouseScrollDelta.y * _timeStampTweakAmount;
                else
                {
                    if (Input.GetKey(_decreaseTimeStampKey)) newDirection = Vector2.left * _timeStampTweakAmount;
                    if (Input.GetKey(_increaseTimeStampKey)) newDirection = Vector2.right * _timeStampTweakAmount;
                }

                _currentSelectedTimeStamp.startPointPosition += newDirection;
                _currentSelectedTimeStamp.endPointPosition += newDirection;

                _currentSelectedTimeStamp.songTime =
                    _waveformDrawer.CalculateSongTimeBasedOnPosition(_currentSelectedTimeStamp.startPointPosition);
            }
        }

        /// <summary>
        /// Returns the closest time stamp based on input origin position.
        /// </summary>
        /// <param name="originPosition">Origin of the distance check.</param>
        /// <returns>Data of the closest time stamp.</returns>
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
        /// <summary>
        /// Helper method for exporting all recorded time stamps to a scribtable object
        /// </summary>
        private void TryExportTimeStamps()
        {
            var obj = ScriptableObject.CreateInstance<SongTimeStamps>();
            List<float> sortedExportedTimeStamps = new(_timeStamps.Count);

            foreach (TimeStampData timeStamp in _timeStamps)
                sortedExportedTimeStamps.Add(timeStamp.songTime);

            sortedExportedTimeStamps = sortedExportedTimeStamps.OrderByDescending(songTime => songTime).ToList();
            sortedExportedTimeStamps.Reverse();

            obj.timeStamps = sortedExportedTimeStamps;

            UnityEditor.AssetDatabase.CreateAsset(obj, _assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            print("Exported timestamps");
        }
#endif

        //Draws all time stamps
        private void OnDrawGizmos()
        {
            for (int i = 0; i < _timeStamps.Count; i++)
            {
                //Little hacky but SOMETIME I HATE UNITY, you can't set the thickness of the gizmos line. 
                //This fixes the gizmo flickering when it's ony 1px wide.

                Gizmos.color = _timeStamps[i].isSelected ? _selectedTimeStampColor : _timeStampColor;
                Vector2 offset = new(_gizmoSpacing, 0);

                Gizmos.DrawLine(_timeStamps[i].startPointPosition, _timeStamps[i].endPointPosition); //center
                Gizmos.DrawLine(_timeStamps[i].startPointPosition - offset, _timeStamps[i].endPointPosition - offset); //left
                Gizmos.DrawLine(_timeStamps[i].startPointPosition + offset, _timeStamps[i].endPointPosition + offset); //right
            }
        }

#if UNITY_EDITOR
        //Draws debug info to the screen
        private void OnGUI()
        {
            GUI.color = Color.white;

            GUI.Label(new Rect(0, 0, 300, 100), $"Playback Speed: {_waveformDrawer.currentPlaybackSpeed}", _debugBoldGuiStyle);
            GUI.Label(new Rect(0, 48, 300, 100), $"Song Time: {_waveformDrawer.currentSongTime}", _debugBoldGuiStyle);

            if (_waveformDrawer.isPaused)
                GUI.Label(new Rect(1750, 0, 300, 100), "Paused", _debugItalicsGuiStyle);

            if (_currentSelectedTimeStamp != null)
                GUI.Label(new Rect(0, 96, 300, 100), $"TimeStamp Time: {_currentSelectedTimeStamp.songTime}", _debugItalicsGuiStyle);
        }
#endif
    }
}