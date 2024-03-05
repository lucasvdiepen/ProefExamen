using UnityEngine;
using System;

namespace ProefExamen.Framework.BeatMapping
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

        /// <summary>
        /// Cursor object used to display the current time in the song.
        /// </summary>
        [field: SerializeField]
        public Transform Cursor { get; private set; }

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
        public float CurrentSongTime => AudioSource != null ? AudioSource.time : -1;

        /// <summary>
        /// Returns the current playback speed.
        /// </summary>
        public float CurrentPlaybackSpeed => _playbackSpeed / 10f;

        /// <summary>
        /// Returns the current active song title.
        /// </summary>
        public string CurrentSongTitle => AudioSource.clip.name;

        /// <summary>
        /// Checks if a audio waveform texture has been made.
        /// </summary>
        public bool HasActiveWaveform => _waveformTexture != null;

        /// <summary>
        /// Returns if audio is paused.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Time amount for scrubbing through the audio track.
        /// </summary>
        public float TimeScrubAmount { get; set; } = 2.5f;

        /// <summary>
        /// Audio source used by the waveform drawer.
        /// </summary>
        public AudioSource AudioSource { get; private set; }

        /// <summary>
        /// Event for when the song has been changed. Returns the new song title and the old song title.
        /// </summary>
        public Action<string, string> OnSongChanged { get; set; }

        /// <summary>
        /// Event for when the keybinds are shown.
        /// </summary>
        public Action OnShowKeyBinds { get; set; }  

        private float _playbackSpeed = 10;
        private float _songWidth = 0;

        private float _audioClipDuration = 0;
        private float[] _dataSamples = null;

        private Vector2 _waveformPositionOffset = Vector2.zero;
        private Color[] _textureColors = null;

        private Texture2D _waveformTexture = null;

        private void Awake() => AudioSource = GetComponent<AudioSource>();

        /// <summary>
        /// Calculates the correct local song time based on a point along the waveform.
        /// </summary>
        /// <param name="position">Position to calculate the song time.</param>
        /// <returns>Song time (float).</returns>
        public float CalculateSongTimeBasedOnPosition(Vector2 position)
        {
            float xPos = position.x - _waveformPositionOffset.x;
            xPos = Mathf.Clamp(xPos, 0, _songWidth);

            return xPos * _audioClipDuration / _songWidth;
        }

        /// <summary>
        /// Method for initialzing the waveform drawer for a specific AudioClip.
        /// </summary>
        /// <param name="audioClip">Audio clip to draw.</param>
        public void InitializeDrawer(AudioClip audioClip)
        {
            Destroy(_waveformTexture);

            AudioClip lastClip = AudioSource.clip;
            AudioSource.clip = audioClip;

            OnSongChanged?.Invoke(audioClip.name, lastClip != null ? lastClip.name : "Null");

            // Create an array to store the audio data.
            _dataSamples = new float[AudioSource.clip.samples * AudioSource.clip.channels];
            
            // Get audio data from the audio clip.
            AudioSource.clip.GetData(_dataSamples, 0);

            // Prepare the waveform texture.
            _waveformTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
            
            // Create an array to store the texture pixel data.
            _textureColors = new Color[_waveformTexture.width * _waveformTexture.height]; 

            GenerateWaveformTexture();
            Renderer renderer = GetComponent<Renderer>();

            // Set the offset for the texture.
            _waveformPositionOffset = new Vector2(
                -(_textureWidth + _textureWidth / _renderDownScaleModifier),
                _curserYPosition
             );
            _songWidth = Mathf.Abs(_waveformPositionOffset.x * 2);

            if (renderer != null)
                renderer.material.mainTexture = _waveformTexture;

            _audioClipDuration = AudioSource.clip.length;

            if (IsPaused) 
                AudioSource.Pause();
            else 
                AudioSource.Play();
        }

        /// <summary>
        /// Generates a texture with the audio clip's audio waveform data displayed.
        /// </summary>
        private void GenerateWaveformTexture()
        {
            for (int x = 0; x < _waveformTexture.width; x++)
            {
                // Calculate the average sample value for the current x position.
                int startSample = Mathf.FloorToInt(x * (_dataSamples.Length / (float)_waveformTexture.width));
                int endSample = Mathf.Min(
                    startSample + (_dataSamples.Length / _waveformTexture.width),
                    _dataSamples.Length
                );
                float sum = 0;

                for (int j = startSample; j < endSample; j++)
                {
                    // Get the absolute value of the sample.
                    sum += Mathf.Abs(_dataSamples[j]);
                }

                float averageSample = sum / (_dataSamples.Length / _waveformTexture.width);
                
                // Scale the average sample value.
                float scaledAverage = averageSample * _heightScaleModifier;

                // Set the pixel color based on the average sample value.
                for (int y = 0; y < _waveformTexture.height; y++)
                    _textureColors[x + y * _waveformTexture.width] = (y < scaledAverage) ? _renderColor : Color.clear;
            }

            _waveformTexture.SetPixels(_textureColors);
            
            // Set the filter mode to point for a pixelated look.
            _waveformTexture.filterMode = FilterMode.Point;
            
            _waveformTexture.Apply();

            GameObject drawerObject = Instantiate(_drawerPrefab, transform.position, _drawerPrefab.transform.rotation);
            drawerObject.transform.SetParent(transform);

            drawerObject.transform.localScale = new Vector3(
                _textureWidth / _renderDownScaleModifier,
                1,
                _textureHeight / _renderDownScaleModifier
            );
            drawerObject.GetComponent<Renderer>().material.mainTexture = _waveformTexture;
        }

        private void Update()
        {
            // Update yOffset for possible live tweaking.
            _waveformPositionOffset.y = _curserYPosition;

            // Update controls for manipulating the audio track.
            CheckAudioControls();
            
            // Move cursor along audio waveform texture.
            UpdateCursorPosition();
        }

        /// <summary>
        /// Handles all input checks related to manipulating the audio track.
        /// </summary>
        private void CheckAudioControls()
        {
            //Can't hold left ctrl, messes with other keybinds.
            if (!Input.GetKey(KeyCode.LeftControl)) 
            {
                //Scrub forward in song.
                if (Input.GetKey(_forwardKey)) 
                    AudioSource.time = Mathf.Clamp(AudioSource.time + TimeScrubAmount, 0, _audioClipDuration - 1);

                //Scrub backward in song.
                if (Input.GetKey(_backwardKey))
                    AudioSource.time = Mathf.Clamp(AudioSource.time - TimeScrubAmount, 0, _audioClipDuration);
            }

            //Pause toggle.
            if (Input.GetKeyDown(_pauseKey))
            {
                if (!IsPaused && AudioSource.isPlaying)
                {
                    AudioSource.Pause();
                }
                else
                {
                    AudioSource.UnPause();
                    if (!AudioSource.isPlaying)
                        AudioSource.Play();
                }

                //Toggle pause state.
                IsPaused = !IsPaused;
            }

            // Mouse playback speed control.
            if (Input.mouseScrollDelta.magnitude > 0)
            {
                //Can't hold ctrl, messes with other keybinds.
                if (Input.GetKey(KeyCode.LeftControl))
                    return;

                IncreasePlaybackSpeed((int)Input.mouseScrollDelta.y);
            }

            // Keyboard playback speed controls.
            if (Input.GetKeyDown(_speedUpKey)) 
                IncreasePlaybackSpeed(1);
            
            if (Input.GetKeyDown(_speedDownKey)) 
                IncreasePlaybackSpeed(-1);

            // Reset song time.
            if(Input.GetKeyDown(_homeKey)) 
                AudioSource.time = 0;

            // Set song time to end of song.
            if (Input.GetKeyDown(_endKey))
                AudioSource.time = _audioClipDuration - 1;
        }

        /// <summary>
        /// Increases the playback speed by an arbitrary amount.
        /// </summary>
        /// <param name="amount">Added amount to the playback speed.</param>
        private void IncreasePlaybackSpeed(int amount)
        {
            _playbackSpeed = Mathf.Clamp(_playbackSpeed + amount, 1f, 30);
            AudioSource.pitch = CurrentPlaybackSpeed;
        }

        /// <summary>
        /// Updates the cursor position based on the current time in the song.
        /// </summary>
        private void UpdateCursorPosition()
        {
            //Check if a song is selected.
            if (_audioClipDuration == 0)
                return;

            float x = _songWidth / _audioClipDuration * AudioSource.time;
            Cursor.transform.position = new Vector2(x, 0) + _waveformPositionOffset;
        }

        /// <summary>
        /// Log all available keybinds to the console. Gets called from custom editor button "Show Keybinds".
        /// </summary>
        public void LogKeybinds()
        {
            Debug.Log("<b>Available Keybinds:</b>");
            Debug.Log("Pause song key: " + _pauseKey);
            Debug.Log(" ");

            Debug.Log("Scrub forward key: " + _forwardKey);
            Debug.Log("Scrub backward Key: " + _backwardKey);
            Debug.Log(" ");

            Debug.Log("Speed up Key: " + _speedUpKey);
            Debug.Log("Speed down Key: " + _speedDownKey);
            Debug.Log(" ");

            Debug.Log("Go to start Key: " + _homeKey);
            Debug.Log("Go to end Key: " + _endKey);

            Debug.Log(" ");
            Debug.Log(" ");

            OnShowKeyBinds?.Invoke();
        }
    }
}
