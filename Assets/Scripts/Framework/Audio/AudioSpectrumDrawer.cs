using UnityEngine;

using ProefExamen.Audio.WaveFormDrawer;

namespace ProefExamen.Audio.SpectrumDrawer
{
    /// <summary>
    /// Class which is used for drawing the audio spectrum of a song.
    /// </summary>
    public class AudioSpectrumDrawer : MonoBehaviour
    {
        [SerializeField]
        private int _audioSamples = 512;

        [SerializeField]
        private float _lerpAmount = 20;

        [SerializeField]
        private float _maxScale = 1200;

        [SerializeField]
        private Vector3 _offsetPosition = Vector3.zero;

        [Space]
        [SerializeField]
        private GameObject _imagePrefab;

        private GameObject[] _visualizers;
        private AudioWaveformDrawer _waveformDrawer;

        private float[] _samples;

        private void Awake()
        {
            _samples = new float[_audioSamples];
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();
        }

        /// <summary>
        /// Method which analyses the specified audio clip's samples.
        /// </summary>
        /// <param name="clip">Clip used for analysing the audio spectrum data.</param>
        public void VisualizeSongSpectrum(AudioClip clip)
        {
            _visualizers = null;
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            _waveformDrawer.InitializeDrawer(clip);
            _visualizers = new GameObject[_samples.Length];

            CreateBandVisualizers();
        }

        /// <summary>
        /// Method used for creating band visualizers.
        /// </summary>
        private void CreateBandVisualizers()
        {
            for (int i = 0; i < _visualizers.Length; i++)
            {
                GameObject newBandVisualizer = Instantiate(_imagePrefab, transform);
                newBandVisualizer.transform.position = transform.position;
                newBandVisualizer.transform.parent = transform;

                newBandVisualizer.name = "BandVisualizer " + i;
                newBandVisualizer.transform.position = new Vector3(transform.position.x + (i * transform.localScale.x), transform.position.y, 0);

                _visualizers[i] = newBandVisualizer;
            }
        }

        //update all bandvisualizers
        private void Update()
        {
            _waveformDrawer.AudioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
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
}