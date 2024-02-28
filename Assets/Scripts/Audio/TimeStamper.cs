using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStamper : MonoBehaviour
{
    [SerializeField] private Color _timeStampColor = Color.blue;
    [SerializeField] private KeyCode _timeStampInputKey;
    [SerializeField] private float _stampPineLenght = 100;

    private AudioWaveformDrawer _waveformDrawer = null;
    private List<TimeStampData> _timeStamps = new List<TimeStampData>();
    public List<TimeStampData> timeStamps => _timeStamps;
    
    private void Awake()
    {
        _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();   
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(_timeStampInputKey))
        {
            float startYPos = _waveformDrawer.cursor.position.y - (_waveformDrawer.cursor.localScale.y * .5f);
            Vector2 startPosition = new Vector2(_waveformDrawer.cursor.position.x, startYPos);
            Vector2 endPosition = new Vector2(_waveformDrawer.cursor.position.x, -_stampPineLenght);

            timeStamps.Add(new TimeStampData(startPosition, endPosition, _waveformDrawer.currentSongTime));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _timeStampColor;
        foreach (TimeStampData timeStamp in timeStamps)
        {
            Gizmos.DrawLine(timeStamp.startPointPosition, timeStamp.endPointPosition);
        }
    }
}
