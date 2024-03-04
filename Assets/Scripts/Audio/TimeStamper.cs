using ProefExamen.Audio.WaveFormDrawer;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace ProefExamen.Audio.TimeStamping
{
    /// <summary>
    /// Class responsible for adding time stamps to a song.
    /// </summary>
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

        public List<TimeStampData> timeStamps => _timeStamps;

        private AudioWaveformDrawer _waveformDrawer = null;
        private TimeStampData _currentSelectedTimeStamp = null;

        private readonly GUIStyle _debugBoldGuiStyle = new();
        private readonly GUIStyle _debugItalicsGuiStyle = new();

        /// <summary>
        /// Returns the raw asset path for the time stamp data container.
        /// </summary>
        public string rawAssetPath => "Assets/SongTimeStampData/";

        /// <summary>
        /// Struct responsible for holding the necessary data for a gizmo line.
        /// </summary>
        [System.Serializable]
        public struct LineData
        {
            public Vector2 startLinePoint;
            public Vector2 endLinePoint;
        }

        /// <summary>
        /// Class responsible for holding the necessary data for a time stamp.
        /// </summary>
        [System.Serializable]
        public class TimeStampData
        {
            /// <summary>
            /// Holds the start and end point of the time stamp.
            /// </summary>
            public LineData lineData;

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
                lineData.startLinePoint = start;
                lineData.endLinePoint = end;
                songTime = time;
            }
        }

        private void Awake()
        {
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();
            _waveformDrawer.onShowKeyBinds += HandleShowKeybinds;

            //set up the debug gui styles
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
            //placing a time stamp
            if (Input.GetKeyDown(_placeTimeStampKey))
            {
                float startYPos = _waveformDrawer.cursor.position.y - (_waveformDrawer.cursor.localScale.y * .5f);
                Vector2 startPosition = new(_waveformDrawer.cursor.position.x, startYPos);
                Vector2 endPosition = new(_waveformDrawer.cursor.position.x, -_stampLineHeightReduction);

                _timeStamps.Add(new TimeStampData(startPosition, endPosition, _waveformDrawer.currentSongTime));
            }

            //undo last time stamp
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

            //exporting time stamps
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
                TimeStampData closestStampToMouse = GetClosestTimeStamp(mousePosition); //get closest time stamp to mouse position

                if (closestStampToMouse == null)
                    return;

                //if closest time stamp is not the current selected time stamp, select it
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
                if (Input.mouseScrollDelta.magnitude > 0) //tweak time stamp position based on mouse scroll
                    newDirection = _timeStampTweakAmount * Input.mouseScrollDelta.y * Vector2.right;
                else
                {
                    if (Input.GetKey(_decreaseTimeStampKey)) //decrease
                        newDirection = Vector2.left * _timeStampTweakAmount;

                    if (Input.GetKey(_increaseTimeStampKey)) //increase
                        newDirection = Vector2.right * _timeStampTweakAmount;
                }

                _currentSelectedTimeStamp.lineData.startLinePoint += newDirection;
                _currentSelectedTimeStamp.lineData.endLinePoint += newDirection;

                //update song time of the current selected time stamp
                _currentSelectedTimeStamp.songTime =
                    _waveformDrawer.CalculateSongTimeBasedOnPosition(_currentSelectedTimeStamp.lineData.startLinePoint);
            }
        }

        /// <summary>
        /// Returns the closest time stamp based on input origin position.
        /// </summary>
        /// <param name="originPosition">Origin of the distance check.</param>
        /// <returns>Data of the closest time stamp.</returns>
        private TimeStampData GetClosestTimeStamp(Vector2 originPosition)
        {
            TimeStampData bestTarget = default;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (TimeStampData timeStampData in _timeStamps)
            {
                Vector3 directionToTarget = timeStampData.lineData.startLinePoint - originPosition; //get the direction to the target
                float dSqrToTarget = directionToTarget.sqrMagnitude; //get the squared distance to the target
                
                if (dSqrToTarget < closestDistanceSqr) //check if the current time stamp is closer than the previous closest time stamp
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = timeStampData;
                }
            }

            return bestTarget;
        }

        /// <summary>
        /// Method for showing the keybinds in the console.
        /// </summary>
        private void HandleShowKeybinds()
        {
            Debug.Log("Place TimeStamp Key: " + _placeTimeStampKey);
            Debug.Log("Undo TimeStamp Key: CTRL + " + _undoTimeStampKey);
            Debug.Log(" "); //empty line

            Debug.Log("Select TimeStamp Key: CTRL");
            Debug.Log("Delete selected TimeStamp Key: CTRL + " + _deleteCurrentTimeStampKey);
            Debug.Log(" "); //empty line

            Debug.Log("Export TimeStamps Key: CTRL + " + _exportTimeStampsKey);
            Debug.Log(" "); //empty line

            Debug.Log("Increase TimeStamp Key: CTRL + " + _increaseTimeStampKey);
            Debug.Log("Decrease TimeStamp Key: CTRL + " + _decreaseTimeStampKey);
            Debug.Log(" "); //empty line
        }

        /// <summary>
        /// Helper method for importing time stamps from a scriptable object (TimeStampDataContainer)
        /// </summary>
        public void TryImportTimeStamps()
        {
            //open file panel to select the TimeStampDataContainer to import
            string path = EditorUtility.OpenFilePanel("Select TimeStampDataContainer to import", "Assets", "asset");

            if(string.IsNullOrEmpty(path)) 
            {
                Debug.LogError($"Specified path: {path} is empty/null!");
                return;
            }

            path = "Assets" + path[Application.dataPath.Length..]; //get the relative path
            var timeStampDataContainer = AssetDatabase.LoadAssetAtPath<TimeStampDataContainer>(path);

            //if the time stamp data container is null, log a warning
            if (timeStampDataContainer == null)
            {
                Debug.LogError($"Failed to load TimeStampDataContainer at path: {path}");
                return;
            }

            if (timeStampDataContainer.songDebugLineData.Count == 0)
            {
                Debug.LogError("No line data found in the TimeStampDataContainer (count == 0), returning...", timeStampDataContainer);
                return;
            }

            if (timeStampDataContainer.timeStamps.Length == 0)
            {
                Debug.LogError("No time stamp data found in the TimeStampDataContainer (lenght == 0), returning...");
                return;
            }

            _timeStamps.Clear(); //clear the current time stamps

            //import the specified time stamps
            for (int i = 0; i < timeStampDataContainer.timeStamps.Length; i++)
            {
                Vector2 startPoint = timeStampDataContainer.songDebugLineData[i].startLinePoint;
                Vector2 endPoint = timeStampDataContainer.songDebugLineData[i].endLinePoint;

                float songTime = timeStampDataContainer.timeStamps[i];

                _timeStamps.Add(new TimeStampData(startPoint, endPoint, songTime)); //add the time stamp to current list
            }

            Debug.Log("Succesfully imported time stamp data");
        }

#if UNITY_EDITOR
        /// <summary>
        /// Helper method for exporting all recorded time stamps to a scribtable object
        /// </summary>
        public void TryExportTimeStamps(string overrideSongTitle = "")
        {
            TimeStampDataContainer obj = ScriptableObject.CreateInstance<TimeStampDataContainer>();
            
            List<LineData> exportedLineData = new(_timeStamps.Count);
            List<float> sortedExportedTimeStamps = new(_timeStamps.Count); 

            foreach (TimeStampData timeStamp in _timeStamps)
            {
                exportedLineData.Add(timeStamp.lineData); //add the line data to the exported line data
                sortedExportedTimeStamps.Add(timeStamp.songTime); //add the song time to the exported time stamps
            }

            //sort the exported time stamps
            sortedExportedTimeStamps = sortedExportedTimeStamps.OrderByDescending(songTime => songTime).ToList();
            sortedExportedTimeStamps.Reverse();

            obj.songDebugLineData = exportedLineData;
            obj.timeStamps = sortedExportedTimeStamps.ToArray();

            //get the song title for the asset name
            string assetName = overrideSongTitle == string.Empty ? _waveformDrawer.currentSongTitle : overrideSongTitle;

            AssetDatabase.CreateAsset(obj, rawAssetPath + $"{assetName}.asset"); //create the asset
            AssetDatabase.Refresh();

            print("Exported timestamps");
        }
#endif

        /// <summary>
        /// Draws gizmos for the time stamps
        /// </summary>
        private void OnDrawGizmos()
        {
            for (int i = 0; i < _timeStamps.Count; i++)
            {
                //Little hacky but SOMETIME I HATE UNITY, you can't set the thickness of the gizmos line. 
                //This fixes the gizmo flickering when it's ony 1px wide.

                Vector2 offset = new(_gizmoSpacing, 0);
                Gizmos.color = _timeStamps[i].isSelected ? _selectedTimeStampColor : _timeStampColor;

                Gizmos.DrawLine(_timeStamps[i].lineData.startLinePoint, _timeStamps[i].lineData.endLinePoint); //center
                Gizmos.DrawLine(_timeStamps[i].lineData.startLinePoint - offset, _timeStamps[i].lineData.endLinePoint - offset); //left
                Gizmos.DrawLine(_timeStamps[i].lineData.startLinePoint + offset, _timeStamps[i].lineData.endLinePoint + offset); //right
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws debug information on the screen.
        /// </summary>
        private void OnGUI()
        {
            GUI.color = Color.white;

            GUI.Label(new Rect(0, 0, 300, 100), $"Playback Speed: {_waveformDrawer.currentPlaybackSpeed}", _debugBoldGuiStyle);
            GUI.Label(new Rect(0, 48, 300, 100), $"Song Time: {_waveformDrawer.currentSongTime}", _debugBoldGuiStyle);

            if (_waveformDrawer.isPaused) //draw paused text
                GUI.Label(new Rect(1750, 0, 300, 100), "Paused", _debugItalicsGuiStyle);

            if (_currentSelectedTimeStamp != null) //draw selected time stamp information
                GUI.Label(new Rect(0, 96, 300, 100), $"TimeStamp Time: {_currentSelectedTimeStamp.songTime}", _debugItalicsGuiStyle);
        }
#endif

        private void OnDestroy()
        {
            //remove event listener
            _waveformDrawer.onShowKeyBinds -= HandleShowKeybinds;
        }
    }
}