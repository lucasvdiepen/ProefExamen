using ProefExamen.Audio.WaveFormDrawer;
using ProefExamen.Audio.SpectrumDrawer;
using ProefExamen.Audio.TimeStamping;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProefExamen.Editor.MappingWindow
{
#if UNITY_EDITOR
    /// <summary>
    /// Editor window for beat mapping.
    /// </summary>
    public class BeatMappingWindow : EditorWindow
    {
        private ObjectField _soundClipField = null;
        private ListView _songList = null;

        private Slider _volumeSlider = null;
        private FloatField _scrubAmoundField = null;

        private Button _keyBindsButton = null;
        private Button _importTimeStampsButton = null;

        private AudioWaveformDrawer _waveformDrawer = null;
        private AudioSpectrumDrawer _spectrumDrawer = null;
        private TimeStamper _timeStamper = null;

        private readonly float _defaultTimeScrubAmount = .1f;
        private readonly string _audioClipFilter = "t:AudioClip";

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
            _timeStamper = FindObjectOfType<TimeStamper>();

            //create UI
            CreateSongListView();
            CreateSoundClipField();

            //create audio control interface if in playmode
            if (Application.isPlaying)
                CreateAudioControlInterface();
        }

        /// <summary>
        /// Creates list view of available audio files.
        /// </summary>
        private void CreateSongListView()
        {
            string[] allObjectGuids = AssetDatabase.FindAssets(_audioClipFilter);
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

            _keyBindsButton = new Button(() => { _waveformDrawer.LogKeybinds(); });
            _keyBindsButton.text = "Keybinds";

            _importTimeStampsButton = new Button(() => { _timeStamper.TryImportTimeStamps(); });
            _importTimeStampsButton.text = "Import Time Stamps";

            rootVisualElement.Add(_volumeSlider);
            rootVisualElement.Add(_scrubAmoundField);
            
            rootVisualElement.Add(_keyBindsButton);
            rootVisualElement.Add(_importTimeStampsButton);
        }

        /// <summary>
        /// Callback method which is called when user clicks on one of the song names in the list view.
        /// </summary>
        /// <param name="obj">Selected audioclip</param>
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
            //update audio control values
            if (Application.isPlaying && _waveformDrawer != null)
            {
                _waveformDrawer.audioSource.volume = _volumeSlider.value;
                _waveformDrawer.timeScrubAmount = _scrubAmoundField.value;
            }
            else
            {
                Close(); //close window if not in playmode
            }
        }

        private void OnGUI()
        {
            //close window on escape key
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
            //remove event listener
            if (_songList == null)
                return;

            _songList.selectionChanged -= OnSongIndexChanged;
        }
    }
#endif
}
