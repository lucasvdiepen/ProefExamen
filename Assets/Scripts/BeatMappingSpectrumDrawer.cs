using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatMappingSpectrumDrawer : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameObject[] _visualizers;
    private float[] samples;

    [Header("Visuals")]
    [SerializeField] private int audioSamples;
    [SerializeField, Range(0f, 20f)] private float lerpAmount;
    [SerializeField] private GameObject imagePrefab;
    [Header("Audio")]
    [SerializeField] private AudioClip audioClip;


    public float maxScale;

    private void Awake()
    {
        //setting necessary values
        samples = new float[audioSamples];
        _audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();

        _visualizers = new GameObject[samples.Length];
        for (int i = 0; i < _visualizers.Length; i++)
        {
            GameObject objToSpawn = Instantiate(imagePrefab);
            objToSpawn.transform.position = transform.position;
            objToSpawn.transform.parent = transform;
            objToSpawn.name = "BandVisualizer " + i;
            objToSpawn.transform.position = new Vector3(i, 0, 0);
            _visualizers[i] = objToSpawn;
        }
    }
    
    private void Update()
    {
        _audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        for (int i = 0; i < _visualizers.Length; i++)
        {
            _visualizers[i].transform.localScale =
            Vector3.Lerp(_visualizers[i].transform.localScale,
            new Vector3(1, (samples[i] * maxScale) + 2, 1), lerpAmount * Time.deltaTime);
        }
    }
}
