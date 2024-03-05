using UnityEditor;
using UnityEngine;
using System;

namespace ProefExamen.Framework.BeatMapping
{
    /// <summary>
    /// Class for showing a warning popup when trying to exit the application with unsaved time stamps.
    /// </summary>
    [RequireComponent(typeof(TimeStamper))]
    public class TimeStampPopupWarning : MonoBehaviour
    {
        private AudioWaveformDrawer _waveformDrawer;
        private TimeStamper _timeStamper;

        private const string _unexportedDataTitle = "You have unexported time stamp data!";
        private const string _unsavedDataTitle = "You have unsaved time stamp data!";
        
        private const string _noContainerMessage = "No TimeStampDataContainer found at the path: {0} " +
            "\nDo you wish to save and export all new changes before exiting?";
        private const string _mismatchDataMessage = "Saved TimeStampDataContainer found at the path: {0} " +
            "\nDoes not match with current time stamp data. Do you wish to save and update all new changes before exiting?";
        
        private const string _exportDataButton = "Yes, export data now";
        private const string _updateDataButton = "Yes, update data now";

        private void Awake()
        {
            // Find the required components.
            _timeStamper = GetComponent<TimeStamper>();
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();

            _waveformDrawer.OnSongChanged += HandleSongChanged;
        }

        /// <summary>
        /// Method for handling the song changed event.
        /// </summary>
        /// <param name="newSongTitle">New song title.</param>
        /// <param name="oldSongTitle">Old song title.</param>
        private void HandleSongChanged(string newSongTitle, string oldSongTitle)
        {
            CheckForUnsavedData(oldSongTitle);
            _timeStamper.timeStamps.Clear();
        }

        /// <summary>
        /// Method for showing a warning popup when trying to exit the application with unsaved/unexported time stamps.
        /// </summary>
        private void CheckForUnsavedData(string songTitle)
        {
            //Return if no time stamps are present.
            if (_timeStamper.timeStamps.Count == 0)
                return;

            string pathToCheck = _timeStamper.rawAssetPath + $"{songTitle}.asset";

            //Check if there is a time stamp data container at the path.
            var existingContainer = AssetDatabase.LoadAssetAtPath<TimeStampDataContainer>(pathToCheck);
            if (existingContainer == null)
            {
                //If no time stamp data container is found, show a warning dialog.
                string message = string.Format(_noContainerMessage, _timeStamper.rawAssetPath + $"{songTitle}.asset");
                ShowWarningDialog(_unexportedDataTitle, message, _exportDataButton, null, songTitle);
            }
            else if (existingContainer.timeStamps.Length != _timeStamper.timeStamps.Count)
            {
                //if the time stamp data container does not match with the current time stamp data, show a warning dialog.
                string message = string.Format(_mismatchDataMessage, _timeStamper.rawAssetPath + $"{songTitle}.asset");
                ShowWarningDialog(_unsavedDataTitle, message, _updateDataButton, null, songTitle);
            }
        }

        /// <summary>
        /// Method for showing a warning popup when trying to exit the application with unsaved time stamps.
        /// </summary>
        /// <param name="title">Title of dialog screen.</param>
        /// <param name="message">Text message of dialog box.</param>
        /// <param name="ok">Ok button text.</param>
        private void ShowWarningDialog(string title, string message, string ok, Action<bool> onSubmit, string overrideSongTitle = "")
        {
            string cancel = "No, I know what I'm doing";
            if (EditorUtility.DisplayDialog(title, message, ok, cancel))
                _timeStamper.TryExportTimeStamps(overrideSongTitle);
        }

        private void OnDestroy()
            => _waveformDrawer.OnSongChanged -= HandleSongChanged;

        private void OnApplicationQuit()
        {
            // Check for unsaved data before quitting the application.
            CheckForUnsavedData(_waveformDrawer.CurrentSongTitle);
            
            AssetDatabase.SaveAssets();
        }
    }
}