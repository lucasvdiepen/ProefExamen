using ProefExamen.Audio.WaveFormDrawer;
using ProefExamen.Audio.SpectrumDrawer;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProefExamen.Editor.MappingWindow
{
#if UNITY_EDITOR
    public class BeatMappingWindow : EditorWindow
    {
        private ObjectField _soundClipField = null;
        private ListView _songList = null;

        private Slider _volumeSlider = null;
        private FloatField _scrubAmoundField = null;

        private AudioWaveformDrawer _waveformDrawer = null;
        private AudioSpectrumDrawer _spectrumDrawer = null;

        private float _defaultTimeScrubAmount = .1f;
        private readonly string audioClipFilter = "t:AudioClip";

        /// <summary>
        /// Static method called when window is created.
        /// </summary>
        [MenuItem("Rythm Game/Beat Mapping Tool")]
        public static void CreateWindow()
        {
            BeatMappingWindow window = GetWindow<BeatMappingWindow>();
            window.titleContent = new GUIContent("Beat Mapping Tool");

            window.minSize = new Vector2(50, 100);
        }

        public void CreateGUI()
        {
            //get necessary refs
            _spectrumDrawer = FindObjectOfType<AudioSpectrumDrawer>();
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();

            if (!Application.isPlaying) //if user tries to open tool not in playmode, notify and return early
            {
                ShowEnterPlayModeView();
                return;
            }

            CreateSongListView();
            CreateSoundClipField();
            CreateAudioControlInterface();
        }

        /// <summary>
        /// Method called when user tries to use tool not in playmode
        /// </summary>
        private void ShowEnterPlayModeView()
        {
            Label label = new("Enter play-mode");
            rootVisualElement.Add(label);

            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 25;

            label.style.paddingTop = 75;
            label.style.color = Color.red;

            rootVisualElement.style.backgroundColor = new Color(.74f, .74f, .74f, .1f);
            PlayModeWarning();
        }

        /// <summary>
        /// Creates list view of available audio files.
        /// </summary>
        private void CreateSongListView()
        {
            string[] allObjectGuids = AssetDatabase.FindAssets(audioClipFilter);
            List<AudioClip> allObjects = new();

            foreach (string guid in allObjectGuids)
                allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));

            _songList = new ListView();
            rootVisualElement.Add(_songList);

            _songList.makeItem = () => new Label();
            _songList.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
            _songList.itemsSource = allObjects;

            _songList.selectionChanged += OnSongIndexChanged;
            _songList.style.paddingTop = 30;
            _songList.style.backgroundColor = new Color(.44f, .44f, .44f, .1f);
        }

        /// <summary>
        /// Create object field of type AudioClip.
        /// </summary>
        private void CreateSoundClipField()
        {
            _soundClipField = new ObjectField("Selected Song");
            _soundClipField.SetEnabled(false);

            _soundClipField.objectType = typeof(AudioClip);
            rootVisualElement.hierarchy.Add(_soundClipField);
        }

        /// <summary>
        /// Creates basic audio control interface.
        /// </summary>
        private void CreateAudioControlInterface()
        {
            _volumeSlider = new Slider("Volume Slider", 0, 1);
            _volumeSlider.value = _waveformDrawer.audioSource.volume;

            _scrubAmoundField = new FloatField("Time Scrub Amount");
            _scrubAmoundField.value = _defaultTimeScrubAmount;
            _waveformDrawer.timeScrubAmount = _scrubAmoundField.value;

            rootVisualElement.Add(_volumeSlider);
            rootVisualElement.Add(_scrubAmoundField);
        }

        /// <summary>
        /// Callback method which is called when user clicks on one of the song names in the list view.
        /// </summary>
        /// <param name="obj"></param>
        private void OnSongIndexChanged(IEnumerable<object> obj)
        {
            List<object> objectList = obj.ToList();
            if (objectList.Count == 0) //check if selected song is not empty
                return;

            AudioClip audioClip = (AudioClip)objectList[0];
            if (audioClip == null) //check if cast to audio clip was valid
                return;

            _soundClipField.value = audioClip;
            if (Application.isPlaying)
            {
                if (_spectrumDrawer != null) //tell spectrum data to draw the waveform of the selected audio clip
                    _spectrumDrawer.VisualizeSongSpectrum(audioClip);
            }
            else
            {
                PlayModeWarning();
            }
        }

        /// <summary>
        /// Method which warns the user if they try to use this window not in playmode.
        /// </summary>
        private void PlayModeWarning()
        {
            if (!Application.isPlaying)
                Debug.LogWarning("Enter play-mode to use BeatMapping Tool");
        }

        private void Update()
        {
            if (Application.isPlaying && _waveformDrawer != null)
            {
                _waveformDrawer.audioSource.volume = _volumeSlider.value;
                _waveformDrawer.timeScrubAmount = _scrubAmoundField.value;
            }
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.Escape)
                    Close();
            }
        }

        private void OnFocus()
            => PlayModeWarning();

        private void OnDestroy()
        {
            if (_songList == null)
                return;

            _songList.selectionChanged -= OnSongIndexChanged;
        }
    }
#endif
}
