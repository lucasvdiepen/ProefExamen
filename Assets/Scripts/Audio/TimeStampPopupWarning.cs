using UnityEditor;
using UnityEngine;
using System;

using ProefExamen.Audio.WaveFormDrawer;

namespace ProefExamen.Audio.TimeStamping.PopupWarning
{
    /// <summary>
    /// Class for showing a warning popup when trying to exit the application with unsaved time stamps.
    /// </summary>
    [RequireComponent(typeof(TimeStamper))]
    public class TimeStampPopupWarning : MonoBehaviour
    {
        private AudioWaveformDrawer _waveformDrawer;
        private TimeStamper _timeStamper;

        private void Awake()
        {
            // Find the required components.
            _timeStamper = GetComponent<TimeStamper>();
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();

            _waveformDrawer.onSongChanged += HandleSongChanged;
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
            if (_timeStamper.timeStamps.Count == 0) //return if no time stamps are present
                return;

            //check if there is a time stamp data container at the path
            TimeStampDataContainer existingContainer = AssetDatabase.LoadAssetAtPath<TimeStampDataContainer>(_timeStamper.rawAssetPath + $"{songTitle}.asset");
            if (existingContainer == null)
            {
                //if no time stamp data container is found, show a warning dialog
                string title = "You have unexported time stamp data!";
                string message = $"No TimeStampDataContainer found at the path: {_timeStamper.rawAssetPath + $"{songTitle}.asset"} \nDo you wish to save and export all new changes before exiting?";
                string ok = "Yes, export data now";

                ShowWarningDialog(title, message, ok, null, songTitle);
            }
            else if (existingContainer.timeStamps.Length != _timeStamper.timeStamps.Count)
            {
                //if the time stamp data container does not match with the current time stamp data, show a warning dialog
                string title = "You have unsaved time stamp data!";
                string message = $"Saved TimeStampDataContainer found at the path: {_timeStamper.rawAssetPath + $"{songTitle}.asset"} \nDoes not match with current time stamp data. Do you wish to save and update all new changes before exiting?";
                string ok = "Yes, update data now";

                ShowWarningDialog(title, message, ok, null, songTitle);
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
            => _waveformDrawer.onSongChanged -= HandleSongChanged;

        private void OnApplicationQuit()
        {
            // Check for unsaved data before quitting the application.
            CheckForUnsavedData(_waveformDrawer.currentSongTitle);
            
            AssetDatabase.SaveAssets();
        }
    }
}