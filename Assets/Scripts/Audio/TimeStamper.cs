using System.Collections.Generic;
using UnityEngine;

public class TimeStamper : MonoBehaviour
{
    [SerializeField] private Color _timeStampColor = Color.blue;
    [SerializeField] private float _stampLineHeightReduction = 100;

    [Header("Input KeyCodes")]
    [SerializeField] private KeyCode _timeStampInputKey;
    [SerializeField] private KeyCode _undoTimeStampKey;

    [SerializeField] private List<TimeStampData> _timeStamps = new List<TimeStampData>();
    
    private AudioWaveformDrawer _waveformDrawer = null;

    [System.Serializable]
    public struct TimeStampData
    {
        public Vector2 startPointPosition;
        public Vector2 endPointPosition;
        public float songTime;

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _timeStampColor;
        foreach (TimeStampData timeStamp in _timeStamps)
        {
            Gizmos.DrawLine(timeStamp.startPointPosition, timeStamp.endPointPosition);

        }
    }
}
