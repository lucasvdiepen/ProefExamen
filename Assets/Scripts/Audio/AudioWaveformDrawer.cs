using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioWaveformDrawer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _textureWidth = 2048;
    [SerializeField] private int _textureHeight = 512;

    [SerializeField] private float _heightScaleModifier = 100f;
    [SerializeField] private int _renderDownScaleModifier = 4;

    [Header("Debugging")]
    [SerializeField] private float _teleportTimePos;
    [SerializeField] private float _timeScrubAmount;

    [Header("Other")]
    [SerializeField] private Color _renderColor = Color.white;
    [SerializeField] private GameObject _drawerPrefab;
    [field: SerializeField] public Transform cursor { get; private set; }
    [SerializeField] private float curserYPosition = -290;

    /// <summary>
    /// Returns the current time in the song. Returns -1 if audioSource is empty
    /// </summary>
    public float currentSongTime => audioSource != null ? audioSource.time : -1;

    private float _songWidth;
    private float _audioClipDuration;
    private float _playBackSpeed = 10;
    private float[] _dataSamples;

    private Vector2 _waveformPositionOffset;
    private Color[] _textureColors;
    
    public AudioSource audioSource { get; private set; }
    private Texture2D _waveformTexture;
    private GUIStyle _debugGuiStyle = new();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void InitializeDrawer(AudioClip audioClip)
    {
        Destroy(_waveformTexture);

        _debugGuiStyle.normal.textColor = Color.white;
        _debugGuiStyle.fontSize = 48;
        _debugGuiStyle.fontStyle = FontStyle.Bold;

        audioSource.clip = audioClip;

        _dataSamples = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.clip.GetData(_dataSamples, 0);

        _waveformTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
        _textureColors = new Color[_waveformTexture.width * _waveformTexture.height];

        GenerateWaveformTexture();
        Renderer renderer = GetComponent<Renderer>();

        _waveformPositionOffset = new Vector2(-(_textureWidth + _textureWidth / _renderDownScaleModifier), curserYPosition);
        _songWidth = Mathf.Abs(_waveformPositionOffset.x * 2);

        if (renderer != null)
            renderer.material.mainTexture = _waveformTexture;

        _audioClipDuration = audioSource.clip.length;
        audioSource.Play();
    }

    private void GenerateWaveformTexture()
    {
        for (int x = 0; x < _waveformTexture.width; x++)
        {
            int startSample = Mathf.FloorToInt(x * (_dataSamples.Length / (float)_waveformTexture.width));
            int endSample = Mathf.Min(startSample + (_dataSamples.Length / _waveformTexture.width), _dataSamples.Length);
            float sum = 0;

            for (int j = startSample; j < endSample; j++) 
                sum += Mathf.Abs(_dataSamples[j]);

            float averageSample = sum / (_dataSamples.Length / _waveformTexture.width);
            float scaledAverage = averageSample * _heightScaleModifier;

            for (int y = 0; y < _waveformTexture.height; y++)
                _textureColors[x + y * _waveformTexture.width] = (y < scaledAverage) ? _renderColor : Color.clear;
        }

        _waveformTexture.SetPixels(_textureColors);
        _waveformTexture.filterMode = FilterMode.Point;
        _waveformTexture.Apply();

        GameObject drawerObject = Instantiate(_drawerPrefab, transform.position, _drawerPrefab.transform.rotation);
        drawerObject.transform.SetParent(transform);

        drawerObject.transform.localScale = new Vector3(_textureWidth / _renderDownScaleModifier, 1, _textureHeight / _renderDownScaleModifier);
        drawerObject.GetComponent<Renderer>().material.mainTexture = _waveformTexture;
    }

    private void Update()
    {
        _waveformPositionOffset.y = curserYPosition; //update yOffset for possible live tweaking

        CheckAudioControls();
        UpdateCursorPosition();
    }


    private void CheckAudioControls()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow)) //pausing song
        {
            if (audioSource.isPlaying) audioSource.Pause();
            else audioSource.UnPause();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) //scrub forward in song
            audioSource.time = Mathf.Clamp(audioSource.time + _timeScrubAmount, 0, _audioClipDuration);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) //scrub backwards in song
            audioSource.time = Mathf.Clamp(audioSource.time - _timeScrubAmount, 0, _audioClipDuration);

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            _playBackSpeed = Mathf.Clamp(_playBackSpeed - 1, 1f, 30);

        //mouse playback speed control
        if(Input.mouseScrollDelta.magnitude != 0)
            IncreasePlayBackSpeed((int)Input.mouseScrollDelta.y);
      
        //keyboard playback speed controls
        if (Input.GetKeyDown(KeyCode.LeftBracket)) IncreasePlayBackSpeed(-1);
        if (Input.GetKeyDown(KeyCode.RightBracket)) IncreasePlayBackSpeed(1);
    }

    private void IncreasePlayBackSpeed(int amount)
    {
        _playBackSpeed = Mathf.Clamp(_playBackSpeed + amount, 1f, 30);
        audioSource.pitch = _playBackSpeed / 10f;
    }

    private void UpdateCursorPosition()
    {
        if (_audioClipDuration == 0) //check if a song is selected
            return;

        float x = _songWidth / _audioClipDuration * audioSource.time;
        cursor.transform.position = new Vector2(x, 0) + _waveformPositionOffset;
    }

    private void OnGUI()
    {
        GUI.color = Color.white;
        GUI.Label(new Rect(0, 0, 300, 100), $"Playback Speed: {_playBackSpeed / 10f}", _debugGuiStyle);
    }
}
