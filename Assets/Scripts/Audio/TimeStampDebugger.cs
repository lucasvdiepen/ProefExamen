using UnityEngine;

using ProefExamen.Audio.WaveFormDrawer;

namespace ProefExamen.Audio.TimeStamping.Debugger
{
    /// <summary>
    /// Class for debugging the time stamps.
    /// </summary>
    [RequireComponent(typeof(TimeStamper))]
    public class TimeStampDebugger : MonoBehaviour
    {
        [SerializeField]
        private Color _timeStampColor = Color.blue;

        [SerializeField]
        private Color _selectedTimeStampColor = Color.yellow;

        [SerializeField]
        private float _stampLineHeightReduction = 190;

        [SerializeField]
        private float _gizmoSpacing = .6f;

        private TimeStamper _timeStamper;
        private AudioWaveformDrawer _waveformDrawer;

        private readonly GUIStyle _debugBoldGuiStyle = new();
        private readonly GUIStyle _debugItalicsGuiStyle = new();

        private void Awake()
        {
            _timeStamper = GetComponent<TimeStamper>();
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();

            // Set up the debug gui styles.
            _debugBoldGuiStyle.fontSize = 48;
            _debugBoldGuiStyle.fontStyle = FontStyle.Bold;
            _debugBoldGuiStyle.normal.textColor = Color.white;

            _debugItalicsGuiStyle.fontSize = 48;
            _debugItalicsGuiStyle.fontStyle = FontStyle.Italic;
            _debugItalicsGuiStyle.normal.textColor = Color.white;
        }

        /// <summary>
        /// Draws gizmos for the time stamps.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Return if the application is not playing.
            if (!Application.isPlaying)
                return;

            for (int i = 0; i < _timeStamper.timeStamps.Count; i++)
            {
                // Little hacky but SOMETIME I HATE UNITY, you can't set the thickness of the gizmos line. 
                // This fixes the gizmo flickering when it's ony 1px wide.

                Vector2 offset = new(_gizmoSpacing, 0);
                Gizmos.color = _timeStamper.timeStamps[i].isSelected ? _selectedTimeStampColor : _timeStampColor;

                Vector2 startPoint = _timeStamper.timeStamps[i].lineData.startLinePoint;
                Vector2 endPoint = _timeStamper.timeStamps[i].lineData.endLinePoint;

                // Center.
                Gizmos.DrawLine(startPoint, endPoint);
                
                // Left.
                Gizmos.DrawLine(startPoint - offset, endPoint - offset);
                
                // Right.
                Gizmos.DrawLine(startPoint + offset, endPoint + offset);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws debug information on the screen.
        /// </summary>
        private void OnGUI()
        {
            GUI.color = Color.white;

            GUI.Label(
                new Rect(0, 0, 300, 100),
                $"Playback Speed: {_waveformDrawer.currentPlaybackSpeed}",
                _debugBoldGuiStyle
            );
            GUI.Label(new Rect(0, 48, 300, 100), $"Song Time: {_waveformDrawer.currentSongTime}", _debugBoldGuiStyle);

            // Draw paused text.
            if (_waveformDrawer.isPaused)
                GUI.Label(new Rect(1750, 0, 300, 100), "Paused", _debugItalicsGuiStyle);

            // Draw selected time stamp information.
            if (_timeStamper.currentSelectedTimeStamp != null)
            {
                GUI.Label(
                    new Rect(0, 96, 300, 100),
                    $"TimeStamp Time: {_timeStamper.currentSelectedTimeStamp.songTime}",
                    _debugItalicsGuiStyle
                );
            }
        }
#endif
    }
}
