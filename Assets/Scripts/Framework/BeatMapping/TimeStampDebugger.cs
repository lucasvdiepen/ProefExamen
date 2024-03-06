using UnityEngine;

namespace ProefExamen.Framework.BeatMapping
{
    /// <summary>
    /// Class for debugging the time stamps.
    /// </summary>
    [RequireComponent(typeof(TimeStamper))]
    public class TimeStampDebugger : MonoBehaviour
    {
        [SerializeField]
        private Color[] _timeStampLaneColor;

        [SerializeField]
        private Color _selectedTimeStampColor = Color.yellow;

        [SerializeField]
        private float _stampLineHeightReduction = 190;

        [SerializeField]
        private float _gizmoSpacing = .6f;

        [SerializeField]
        private Texture selectedGizmoTexture = null;

        [SerializeField, Space]
        private Vector2 _arrowOffsetPosition;

        [SerializeField]
        private Vector2 _arrowScale = Vector2.one;

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

            _debugItalicsGuiStyle.fontSize = 24;
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

            for (int i = 0; i < _timeStamper.TimeStamps.Count; i++)
            {
                // Little hacky but SOMETIME I HATE UNITY, you can't set the thickness of the gizmos line. 
                // This fixes the gizmo flickering when it's ony 1px wide.

                Vector2 offset = new(_gizmoSpacing, 0);

                bool isSelected = _timeStamper.TimeStamps[i].isSelected;
                Color normalGizmoColor = _timeStampLaneColor[_timeStamper.TimeStamps[i].laneID];

                Gizmos.color = isSelected ? _selectedTimeStampColor : normalGizmoColor;

                if (isSelected)
                {
                    float startRect = _timeStamper.TimeStamps[i].lineData.startLinePoint.x + _arrowOffsetPosition.x;
                    float endRect = _timeStamper.TimeStamps[i].lineData.endLinePoint.y + _arrowOffsetPosition.y;

                    endRect += 5 * Mathf.Sin(Mathf.PI * 2 * Time.time);

                    Rect rect = new(startRect, endRect, _arrowScale.x, _arrowScale.y);
                    Gizmos.DrawGUITexture(rect, selectedGizmoTexture);
                }

                Vector2 startPoint = _timeStamper.TimeStamps[i].lineData.startLinePoint;
                Vector2 endPoint = _timeStamper.TimeStamps[i].lineData.endLinePoint;

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
                $"Playback Speed: {_waveformDrawer.CurrentPlaybackSpeed}",
                _debugBoldGuiStyle
            );
            GUI.Label(new Rect(0, 48, 300, 100), $"Song Time: {_waveformDrawer.CurrentSongTime}", _debugBoldGuiStyle);

            // Draw paused text.
            if (_waveformDrawer.IsPaused)
                GUI.Label(new Rect(1750, 0, 300, 100), "Paused", _debugItalicsGuiStyle);

            // Draw selected time stamp information.
            if (_timeStamper.CurrentSelectedTimeStamp != null)
            {
                GUI.Label(
                    new Rect(0, 105, 300, 100),
                    $"TimeStamp Time: {_timeStamper.CurrentSelectedTimeStamp.songTime}",
                    _debugItalicsGuiStyle
                );

                GUI.Label(
                    new Rect(0, 129, 300, 100),
                    $"TimeStamp LaneID: {_timeStamper.CurrentSelectedTimeStamp.laneID}", 
                    _debugItalicsGuiStyle
                );
            }

            // Move down on screen
            GUILayout.BeginArea(new Rect(0, 200, 200, 200));
            GUILayout.Box("Visible Gizmos", GUILayout.Height(50), GUILayout.Width(50));

            bool isLane1Visible = GUILayout.Toggle(false, $"Lane 1");
            bool isLane2Visible = GUILayout.Toggle(false, $"Lane 2");
            bool isLane3Visible = GUILayout.Toggle(false, $"Lane 3");
            bool isLane4Visible = GUILayout.Toggle(false, $"Lane 4");

            GUILayout.EndArea();
        }
#endif
    }
}
