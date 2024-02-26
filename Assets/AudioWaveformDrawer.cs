using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioWaveformDrawer : MonoBehaviour
{
    [SerializeField] private int _textureWidth = 2048;
    [SerializeField] private int _textureHeight = 512;
    [SerializeField] private float _heightScaleModifier = 100;
    [SerializeField] private int _renderDownScaleModifier = 4;
    [SerializeField] private Color _renderColor = Color.white;
    [SerializeField] private GameObject _waveformBorder;
    [SerializeField] private GameObject _drawerPrefab;

    private AudioSource _audioSource;
    private Texture2D _waveformTexture;
    private float[] _dataSamples;
    private Color[] _textureColors;
    private float _timePerSample;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _dataSamples = new float[_audioSource.clip.samples * _audioSource.clip.channels];
        _audioSource.clip.GetData(_dataSamples, 0);

        _waveformTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
        _textureColors = new Color[_waveformTexture.width * _waveformTexture.height];

        _timePerSample = 1f / _audioSource.clip.frequency * _audioSource.clip.channels;

        GenerateWaveformTexture();
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
            renderer.material.mainTexture = _waveformTexture;

        _waveformBorder.transform.localScale = new Vector3(_waveformTexture.width, _waveformBorder.transform.localScale.y, _waveformBorder.transform.localScale.z);
        _audioSource.Play();
    }

    private void GenerateWaveformTexture()
    {
        for (int x = 0; x < _waveformTexture.width; x++)
        {
            int startSample = Mathf.FloorToInt(x * (_dataSamples.Length / (float)_waveformTexture.width));
            int endSample = Mathf.Min(startSample + (_dataSamples.Length / _waveformTexture.width), _dataSamples.Length);
            float sum = 0;

            for (int j = startSample; j < endSample; j++)
            {
                sum += Mathf.Abs(_dataSamples[j]);
            }

            float averageSample = sum / (_dataSamples.Length / _waveformTexture.width);
            float scaledAverage = averageSample * _heightScaleModifier;

            for (int y = 0; y < _waveformTexture.height; y++)
            {
                _textureColors[x + y * _waveformTexture.width] = (y < scaledAverage) ? _renderColor : Color.clear;
            }
        }
        _waveformTexture.SetPixels(_textureColors);
        _waveformTexture.filterMode = FilterMode.Point;
        _waveformTexture.Apply();

        GameObject drawerObject = Instantiate(_drawerPrefab, transform);
        drawerObject.transform.localScale = new Vector3(_textureWidth / _renderDownScaleModifier, 1, _textureHeight / _renderDownScaleModifier);
        drawerObject.GetComponent<Renderer>().material.mainTexture = _waveformTexture;
    }
}
    