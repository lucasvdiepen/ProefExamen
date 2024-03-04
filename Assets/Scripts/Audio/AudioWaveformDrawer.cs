using UnityEngine;
using System;

namespace ProefExamen.Audio.WaveFormDrawer
{
    /// <summary>
    /// Class responsible for drawing the waveform of an audioclip to a texture.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioWaveformDrawer : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        private int _textureWidth = 2048;

        [SerializeField]
        private int _textureHeight = 512;

        [SerializeField]
        private float _heightScaleModifier = 100f;

        [SerializeField]
        private int _renderDownScaleModifier = 4;

        [Header("Other")]
        [SerializeField]
        private Color _renderColor = Color.white;

        [SerializeField]
        private GameObject _drawerPrefab = null;

        [field: SerializeField]
        /// <summary>
        /// Cursor object used to display the current time in the song.
        /// </summary>
        public Transform cursor { get; private set; }

        [SerializeField]
        private float _curserYPosition = -290;

        [Header("Input Keycodes")]
        [SerializeField]
        private KeyCode _pauseKey = KeyCode.DownArrow;

        [SerializeField]
        private KeyCode _forwardKey = KeyCode.RightArrow;

        [SerializeField]
        private KeyCode _backwardKey = KeyCode.LeftArrow;

        [SerializeField]
        private KeyCode _speedUpKey = KeyCode.RightBracket;

        [SerializeField]
        private KeyCode _speedDownKey = KeyCode.LeftBracket;

        [SerializeField]
        private KeyCode _homeKey = KeyCode.Home;
             
        [SerializeField]
        private KeyCode _endKey = KeyCode.End;

        /// <summary>
        /// Returns the current time in the song. Returns -1 if audioSource is empty.
        /// </summary>
        public float currentSongTime => audioSource != null ? audioSource.time : -1;

        /// <summary>
        /// Returns the current playback speed.
        /// </summary>
        public float currentPlaybackSpeed => _playbackSpeed / 10f;

        /// <summary>
        /// Returns the current active song title.
        /// </summary>
        public string currentSongTitle => audioSource.clip.name;

        /// <summary>
        /// Checks if a audio waveform texture has been made.
        /// </summary>
        public bool hasActiveWaveform => _waveformTexture != null;

        /// <summary>
        /// Returns if audio is paused.
        /// </summary>
        public bool isPaused { get; private set; }

        /// <summary>
        /// Time amount for scrubbing through the audio track.
        /// </summary>
        public float timeScrubAmount { get; set; } = 2.5f;

        /// <summary>
        /// Audio source used by the waveform drawer.
        /// </summary>
        public AudioSource audioSource { get; private set; }

        /// <summary>
        /// Event for when the song has been changed. Returns the new song title and the old song title.
        /// </summary>
        public Action<string, string> onSongChanged { get; set; }

        /// <summary>
        /// Event for when the keybinds are shown.
        /// </summary>
        public Action onShowKeyBinds { get; set; }  

        private float _playbackSpeed = 10;
        private float _songWidth = 0;

        private float _audioClipDuration = 0;
        private float[] _dataSamples = null;

        private Vector2 _waveformPositionOffset = Vector2.zero;
        private Color[] _textureColors = null;

        private Texture2D _waveformTexture = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Calculates the correct local song time based on a point along the waveform.
        /// </summary>
        /// <param name="position">Position to calculate the song time.</param>
        /// <returns>Song time (float).</returns>
        public float CalculateSongTimeBasedOnPosition(Vector2 position)
        {
            float xPos = position.x - _waveformPositionOffset.x;
            xPos = Mathf.Clamp(xPos, 0, _songWidth);

            return xPos * _audioClipDuration / _songWidth; ;
        }

        /// <summary>
        /// Log all available keybinds to the console.
        /// </summary>
        public void LogKeybinds()
        {
            Debug.Log("<b>Available Keybinds:</b>");
            Debug.Log("Pause song key: " + _pauseKey);
            Debug.Log(" "); //empty line

            Debug.Log("Scrub forward key: " + _forwardKey);
            Debug.Log("Scrub backward Key: " + _backwardKey);
            Debug.Log(" "); //empty line

            Debug.Log("Speed up Key: " + _speedUpKey);
            Debug.Log("Speed down Key: " + _speedDownKey);
            Debug.Log(" "); //empty line

            Debug.Log("Go to start Key: " + _homeKey);
            Debug.Log("Go to end Key: " + _endKey);

            Debug.Log(" "); //empty line
            Debug.Log(" "); //empty line
            onShowKeyBinds?.Invoke();
        }

        /// <summary>
        /// Method for initialzing the waveform drawer for a specific AudioClip.
        /// </summary>
        /// <param name="audioClip">Audio clip to draw.</param>
        public void InitializeDrawer(AudioClip audioClip)
        {
            Destroy(_waveformTexture);

            AudioClip lastClip = audioSource.clip;
            audioSource.clip = audioClip;

            onSongChanged?.Invoke(audioClip.name, lastClip != null ? lastClip.name : "Null");

            _dataSamples = new float[audioSource.clip.samples * audioSource.clip.channels]; // create an array to store the audio data
            audioSource.clip.GetData(_dataSamples, 0); //get audio data from the audio clip

            _waveformTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false); //prepare the waveform texture
            _textureColors = new Color[_waveformTexture.width * _waveformTexture.height]; //create an array to store the texture pixel data

            GenerateWaveformTexture();
            Renderer renderer = GetComponent<Renderer>();

            //set the offset for the texture
            _waveformPositionOffset = new Vector2(-(_textureWidth + _textureWidth / _renderDownScaleModifier), _curserYPosition);
            _songWidth = Mathf.Abs(_waveformPositionOffset.x * 2);

            if (renderer != null)
                renderer.material.mainTexture = _waveformTexture;

            _audioClipDuration = audioSource.clip.length;

            if (isPaused) 
                audioSource.Pause();
            else 
                audioSource.Play();
        }

        /// <summary>
        /// Generates a texture with the audio clip's audio waveform data displayed.
        /// </summary>
        private void GenerateWaveformTexture()
        {
            for (int x = 0; x < _waveformTexture.width; x++)
            {
                //calculate the average sample value for the current x position
                int startSample = Mathf.FloorToInt(x * (_dataSamples.Length / (float)_waveformTexture.width));
                int endSample = Mathf.Min(startSample + (_dataSamples.Length / _waveformTexture.width), _dataSamples.Length);
                float sum = 0;

                for (int j = startSample; j < endSample; j++)
                    sum += Mathf.Abs(_dataSamples[j]); //get the absolute value of the sample

                float averageSample = sum / (_dataSamples.Length / _waveformTexture.width);
                float scaledAverage = averageSample * _heightScaleModifier; //scale the average sample value

                for (int y = 0; y < _waveformTexture.height; y++) //set the pixel color based on the average sample value
                    _textureColors[x + y * _waveformTexture.width] = (y < scaledAverage) ? _renderColor : Color.clear;
            }

            _waveformTexture.SetPixels(_textureColors);
            _waveformTexture.filterMode = FilterMode.Point; //set the filter mode to point for a pixelated look
            _waveformTexture.Apply();

            GameObject drawerObject = Instantiate(_drawerPrefab, transform.position, _drawerPrefab.transform.rotation);
            drawerObject.transform.SetParent(transform);

            drawerObject.transform.localScale = new Vector3(_textureWidth / _renderDownScaleModifier, 1, _textureHeight / _renderDownScaleModifier);
            drawerObject.GetComponent<Renderer>().material.mainTexture = _waveformTexture;
        }

        private void Update()
        {
            _waveformPositionOffset.y = _curserYPosition; //update yOffset for possible live tweaking

            CheckAudioControls(); //update controls for manipulating the audio track
            UpdateCursorPosition(); //move cursor along audio waveform texture
        }

        /// <summary>
        /// Handles all input checks related to manipulating the audio track.
        /// </summary>
        private void CheckAudioControls()
        {
            if (!Input.GetKey(KeyCode.LeftControl)) //can't hold left ctrl, messes with other keybinds
            {
                if (Input.GetKey(_forwardKey)) //scrub forward in song
                    audioSource.time = Mathf.Clamp(audioSource.time + timeScrubAmount, 0, _audioClipDuration - 1);

                if (Input.GetKey(_backwardKey)) //scrub backward in song
                    audioSource.time = Mathf.Clamp(audioSource.time - timeScrubAmount, 0, _audioClipDuration);
            }

            if (Input.GetKeyDown(_pauseKey)) //pausing song
            {
                if (!isPaused)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                }

                isPaused = !isPaused; //toggle pause state
            }

            //mouse playback speed control
            if (Input.mouseScrollDelta.magnitude > 0)
            {
                if (Input.GetKey(KeyCode.LeftControl)) //can't hold ctrl, messes with other keybinds
                    return;

                IncreasePlaybackSpeed((int)Input.mouseScrollDelta.y);
            }

            //keyboard playback speed controls
            if (Input.GetKeyDown(_speedUpKey)) 
                IncreasePlaybackSpeed(1);
            
            if (Input.GetKeyDown(_speedDownKey)) 
                IncreasePlaybackSpeed(-1);

            //reset song time
            if(Input.GetKeyDown(_homeKey)) 
                audioSource.time = 0;

            //set song time to end of song
            if (Input.GetKeyDown(_endKey))
                audioSource.time = _audioClipDuration - 1;
        }

        /// <summary>
        /// Increases the playback speed by an arbitrary amount.
        /// </summary>
        /// <param name="amount">Added amount to the playback speed.</param>
        private void IncreasePlaybackSpeed(int amount)
        {
            _playbackSpeed = Mathf.Clamp(_playbackSpeed + amount, 1f, 30);
            audioSource.pitch = currentPlaybackSpeed;
        }

        /// <summary>
        /// Updates the cursor position based on the current time in the song.
        /// </summary>
        private void UpdateCursorPosition()
        {
            if (_audioClipDuration == 0) //check if a song is selected
                return;

            float x = _songWidth / _audioClipDuration * audioSource.time;
            cursor.transform.position = new Vector2(x, 0) + _waveformPositionOffset;
        }
    }
}
