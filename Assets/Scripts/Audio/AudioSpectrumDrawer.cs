using UnityEngine;

public class AudioSpectrumDrawer : MonoBehaviour
{
    [SerializeField] private int _audioSamples;
    [SerializeField, Range(0f, 20f)] private float _lerpAmount;
    [SerializeField] private float _maxScale;
    [SerializeField] private Vector3 _offsetPosition;

    [Space]
    [SerializeField] private GameObject _imagePrefab;
    
    private float[] _samples;
    
    private GameObject[] _visualizers; 
    private AudioWaveformDrawer _waveformDrawer = null;

    private void Awake()
    {
        _samples = new float[_audioSamples];
        _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();
    }

    public void VisualizeSongSpectrum(AudioClip clip)
    {
        _visualizers = null;
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        _waveformDrawer.InitializeDrawer(clip);
        _visualizers = new GameObject[_samples.Length];

        for (int i = 0; i < _visualizers.Length; i++)
        {
            GameObject objToSpawn = Instantiate(_imagePrefab, transform);
            objToSpawn.transform.position = transform.position;
            objToSpawn.transform.parent = transform;
            objToSpawn.name = "BandVisualizer " + i;
            objToSpawn.transform.position = new Vector3(transform.position.x + (i * transform.localScale.x), transform.position.y, 0);
            _visualizers[i] = objToSpawn;
        }
    }

    private void Update()
    {
        _waveformDrawer.audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
        if (_visualizers == null)
            return;

        for (int i = 0; i < _visualizers.Length; i++)
        {
            _visualizers[i].transform.localScale =
            Vector3.Lerp(_visualizers[i].transform.localScale,
            new Vector3(1, (_samples[i] * _maxScale) + 2, 1), _lerpAmount * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        Vector2 cameraPos = Camera.main.transform.position;
        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z) + _offsetPosition;
    }
}