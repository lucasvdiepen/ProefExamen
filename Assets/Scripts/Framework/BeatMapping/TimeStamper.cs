using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace ProefExamen.Framework.BeatMapping
{
    /// <summary>
    /// Class responsible for adding time stamps to a song.
    /// </summary>
    public class TimeStamper : MonoBehaviour
    {
        [SerializeField]
        private float _stampLineHeightReduction = 100;

        [SerializeField]
        private float _timeStampTweakAmount = .5f;

        [SerializeField]
        private float _gizmoSpacing = .25f;

        [Header("Input KeyCodes")]
        [SerializeField]
        private KeyCode[] _placeTimeStampKeys;

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

        /// <summary>
        /// Points to the list of time stamps.
        /// </summary>
        public List<TimeStampData> TimeStamps => _timeStamps;

        /// <summary>
        /// Returns the current selected time stamp.
        /// </summary>
        public TimeStampData CurrentSelectedTimeStamp { get; private set;}

        /// <summary>
        /// Returns the raw asset path for the time stamp data container.
        /// </summary>
        public string RawAssetPath => "Assets/SongTimeStampData/";

        private AudioWaveformDrawer _waveformDrawer = null;

        private void Awake()
        {
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();
            _waveformDrawer.OnShowKeyBinds += HandleShowKeybinds;
        }

        private void Update()
        {
            if (!_waveformDrawer.HasActiveWaveform)
                return;

            HandleTimeStampControls();
            HandleTimeStampSelection();
        }

        /// <summary>
        /// Handles all input checks for time stamping.
        /// </summary>
        private void HandleTimeStampControls()
        {
            for (int i = 0; i < _placeTimeStampKeys.Length; i++)
            {
                //Placing a time stamp.
                if (Input.GetKeyDown(_placeTimeStampKeys[i]))
                {
                    float startYPos = _waveformDrawer.Cursor.position.y - (_waveformDrawer.Cursor.localScale.y * .5f);
                    Vector2 startPosition = new(_waveformDrawer.Cursor.position.x, startYPos);
                    Vector2 endPosition = new(_waveformDrawer.Cursor.position.x, -_stampLineHeightReduction);

                    _timeStamps.Add(new TimeStampData(startPosition, endPosition, _waveformDrawer.CurrentSongTime, i));
                }
            }

            //Undo last time stamp.
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_undoTimeStampKey))
            {
                if (_timeStamps.Count > 0)
                    _timeStamps.RemoveAt(_timeStamps.Count - 1);
            }

            //Deleting selected time stamp.
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_deleteCurrentTimeStampKey))
            {
                if (CurrentSelectedTimeStamp != null)
                {
                    TimeStampData timeStampToDelete = CurrentSelectedTimeStamp;
                    CurrentSelectedTimeStamp = null;
                    _timeStamps.Remove(timeStampToDelete);
                }
            }

            //Exporting time stamps.
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_exportTimeStampsKey))
                TryExportTimeStamps();
        }

        /// <summary>
        /// Handles logic for selecting specific time stamps.
        /// </summary>
        private void HandleTimeStampSelection()
        {
            // Try select closest time stamp to the mouse position.
            if (Input.GetKey(KeyCode.LeftControl)) 
            {
                //Get closest time stamp to mouse position.
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TimeStampData closestStampToMouse = GetClosestTimeStamp(mousePosition);

                if (closestStampToMouse == null)
                    return;

                //If closest time stamp is not the current selected time stamp, select it.
                if (closestStampToMouse != CurrentSelectedTimeStamp)
                {
                    closestStampToMouse.isSelected = true;
                    if (CurrentSelectedTimeStamp != null)
                        CurrentSelectedTimeStamp.isSelected = false;

                    CurrentSelectedTimeStamp = closestStampToMouse;
                }
            }

            //When releasing ctrl, remove current time stamp selection.
            if (Input.GetKeyUp(KeyCode.LeftControl) && CurrentSelectedTimeStamp != null)
            {
                CurrentSelectedTimeStamp.isSelected = false;
                CurrentSelectedTimeStamp = null;
            }

            //Tweak current selected time stamp if it is not null.
            if (CurrentSelectedTimeStamp != null)
            {
                Vector2 newDirection = Vector2.zero;
                if (Input.mouseScrollDelta.magnitude > 0) 
                {
                    newDirection = _timeStampTweakAmount * Input.mouseScrollDelta.y * Vector2.right;
                }
                else
                {
                    if (Input.GetKey(_decreaseTimeStampKey))
                        newDirection = Vector2.left * _timeStampTweakAmount;

                    if (Input.GetKey(_increaseTimeStampKey))
                        newDirection = Vector2.right * _timeStampTweakAmount;
                }

                CurrentSelectedTimeStamp.lineData.startLinePoint += newDirection;
                CurrentSelectedTimeStamp.lineData.endLinePoint += newDirection;

                //Update song time of the current selected time stamp.
                CurrentSelectedTimeStamp.songTime =
                    _waveformDrawer.CalculateSongTimeBasedOnPosition(CurrentSelectedTimeStamp.lineData.startLinePoint);
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
                // Get the direction to the target.
                Vector3 directionToTarget = timeStampData.lineData.startLinePoint - originPosition;
                
                // Get the squared distance to the target.
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                
                // Check if the current time stamp is closer than the previous closest time stamp.
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = timeStampData;
                }
            }

            return bestTarget;
        }

        /// <summary>
        /// Helper method for importing time stamps from a scriptable object (TimeStampDataContainer).
        /// </summary>
        public void TryImportTimeStamps()
        {
            //Open file panel to select the TimeStampDataContainer to import.
            string path = EditorUtility.OpenFilePanel("Select TimeStampDataContainer to import", "Assets", "asset");

            if(string.IsNullOrEmpty(path)) 
            {
                Debug.LogError($"Specified path: {path} is empty/null!");
                return;
            }

            //Get the relative path.
            path = "Assets" + path[Application.dataPath.Length..];
            var timeStampDataContainer = AssetDatabase.LoadAssetAtPath<TimeStampDataContainer>(path);

            //If the time stamp data container is null, log a warning.
            if (timeStampDataContainer == null)
            {
                Debug.LogError($"Failed to load TimeStampDataContainer at path: {path}");
                return;
            }

            if (timeStampDataContainer.songDebugLineData.Count == 0)
            {
                Debug.LogError("No line data found in the TimeStampDataContainer (count == 0), returning...", 
                    timeStampDataContainer);
                return;
            }

            if (timeStampDataContainer.timeStamps.Length == 0)
            {
                Debug.LogError("No time stamp data found in the TimeStampDataContainer (lenght == 0), returning...", 
                    timeStampDataContainer);
                return;
            }

            //Clear the current time stamps.
            _timeStamps.Clear(); 

            //Import the specified time stamps.
            for (int i = 0; i < timeStampDataContainer.timeStamps.Length; i++)
            {
                Vector2 startPoint = timeStampDataContainer.songDebugLineData[i].startLinePoint;
                Vector2 endPoint = timeStampDataContainer.songDebugLineData[i].endLinePoint;

                float songTime = timeStampDataContainer.timeStamps[i];

                //Add the time stamp to current list.
                _timeStamps.Add(new TimeStampData(startPoint, endPoint, songTime, timeStampDataContainer.laneIDs[i])); 
            }

            Debug.Log("Succesfully imported time stamp data");
        }

#if UNITY_EDITOR
        /// <summary>
        /// Helper method for exporting all recorded time stamps to a scribtable object.
        /// </summary>
        public void TryExportTimeStamps(string overrideSongTitle = "")
        {
            TimeStampDataContainer obj = ScriptableObject.CreateInstance<TimeStampDataContainer>();
            
            List<LineData> exportedLineData = new(_timeStamps.Count);
            List<float> sortedExportedTimeStamps = new(_timeStamps.Count);
            List<int> exportedLaneIDs = new(_timeStamps.Count);

            foreach (TimeStampData timeStamp in _timeStamps)
            {
                exportedLineData.Add(timeStamp.lineData);
                sortedExportedTimeStamps.Add(timeStamp.songTime);
                exportedLaneIDs.Add(timeStamp.laneID);
            }

            //Sort the exported time stamps.
            sortedExportedTimeStamps = sortedExportedTimeStamps.OrderByDescending(songTime => songTime).ToList();
            sortedExportedTimeStamps.Reverse();

            obj.songDebugLineData = exportedLineData;
            obj.timeStamps = sortedExportedTimeStamps.ToArray();
            obj.laneIDs = exportedLaneIDs.ToArray(); 

            //Get the song title for the asset name.
            string assetName = overrideSongTitle == string.Empty ? _waveformDrawer.CurrentSongTitle : overrideSongTitle;

            AssetDatabase.CreateAsset(obj, RawAssetPath + $"{assetName}.asset");
            AssetDatabase.Refresh();

            Debug.Log("Exported timestamps");
        }
#endif

        /// <summary>
        /// Method for showing the keybinds in the console. Gets called from custom editor button "Show Keybinds".
        /// </summary>
        private void HandleShowKeybinds()
        {
            Debug.Log("Place TimeStamp Key: " + _placeTimeStampKeys[0]);
            Debug.Log("Undo TimeStamp Key: CTRL + " + _undoTimeStampKey);
            Debug.Log(" ");

            Debug.Log("Select TimeStamp Key: CTRL");
            Debug.Log("Delete selected TimeStamp Key: CTRL + " + _deleteCurrentTimeStampKey);
            Debug.Log(" ");

            Debug.Log("Export TimeStamps Key: CTRL + " + _exportTimeStampsKey);
            Debug.Log(" ");

            Debug.Log("Increase TimeStamp Key: CTRL + " + _increaseTimeStampKey);
            Debug.Log("Decrease TimeStamp Key: CTRL + " + _decreaseTimeStampKey);
            Debug.Log(" ");
        }

        private void OnDestroy()
        {
            //Remove event listener.
            _waveformDrawer.OnShowKeyBinds -= HandleShowKeybinds;
        }
    }
}