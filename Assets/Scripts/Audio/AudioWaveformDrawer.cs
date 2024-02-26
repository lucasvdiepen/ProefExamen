using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioWaveformDrawer : MonoBehaviour
{
    [SerializeField] private int _textureWidth = 2048;
    [SerializeField] private int _textureHeight = 512;
    [SerializeField] private float _heightScaleModifier = 100f;
    [SerializeField] private int _renderDownScaleModifier = 4;
    [SerializeField] private Color _renderColor = Color.white;
    [SerializeField] private GameObject _drawerPrefab;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private Transform _cursor;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _cursorOffset;

    private Vector2 _waveformPositionOffset;
    private float _songWidth;
    private float _audioClipDuration;
    private Texture2D _waveformTexture;
    private float[] _dataSamples;
    private Color[] _textureColors;
    public float _timePerSample;

    private void Awake()
    {
        _dataSamples = new float[_audioClip.samples * _audioClip.channels];
        _audioClip.GetData(_dataSamples, 0);

        _waveformTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
        _textureColors = new Color[_waveformTexture.width * _waveformTexture.height];

        _timePerSample = 1f / (_audioClip.frequency * _audioClip.channels);

        GenerateWaveformTexture();
        Renderer renderer = GetComponent<Renderer>();

        _waveformPositionOffset = new Vector2(-(_textureWidth + _textureWidth / _renderDownScaleModifier), -290);
        _songWidth = Mathf.Abs(_waveformPositionOffset.x * 2);

        if (renderer != null)
            renderer.material.mainTexture = _waveformTexture;

        _audioClipDuration = _audioClip.length;
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
        //drawerObject.transform.localPosition = new Vector2(_waveformPositionOffset.x + (_textureWidth + _textureWidth / _renderDownScaleModifier), _waveformPositionOffset.y);

        drawerObject.transform.localScale = new Vector3(_textureWidth / _renderDownScaleModifier, 1, _textureHeight / _renderDownScaleModifier);
        drawerObject.GetComponent<Renderer>().material.mainTexture = _waveformTexture;
        _cursor.transform.position = new Vector3(_waveformPositionOffset.x, _waveformPositionOffset.y);
    }

    private void Update()
    {
        float x = _songWidth / _audioClipDuration * _audioSource.time;
        _cursor.transform.position = new Vector2(x + _cursorOffset, 0) + _waveformPositionOffset;
    }
}
