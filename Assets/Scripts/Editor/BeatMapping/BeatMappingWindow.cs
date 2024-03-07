using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEditor;
using UnityEngine;

using ProefExamen.Framework.BeatMapping;

namespace ProefExamen.Editor.BeatMapping
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

        /// <summary>
        /// Method for creating the GUI of the window.
        /// </summary>
        public void CreateGUI()
        {
            // Get necessary refs.
            _spectrumDrawer = FindObjectOfType<AudioSpectrumDrawer>();
            _waveformDrawer = FindObjectOfType<AudioWaveformDrawer>();
            _timeStamper = FindObjectOfType<TimeStamper>();

            // Create UI.
            CreateSongListView();
            CreateSoundClipField();

            // Create audio control interface if in playmode.
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
            _volumeSlider.value = _waveformDrawer.AudioSource.volume;

            _scrubAmoundField = new FloatField("Time Scrub Amount");
            _scrubAmoundField.value = _defaultTimeScrubAmount;
            _waveformDrawer.TimeScrubAmount = _scrubAmoundField.value;

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
        /// <param name="obj">The selected audioclip.</param>
        private void OnSongIndexChanged(IEnumerable<object> obj)
        {
            List<object> objectList = obj.ToList();

            // Check if selected song is not empty.
            if (objectList.Count == 0)
                return;

            AudioClip audioClip = (AudioClip)objectList[0];

            // Check if cast to audio clip was valid.
            if (audioClip == null)
                return;

            _soundClipField.value = audioClip;
            if (Application.isPlaying)
            {
                // Tell spectrum data to draw the waveform of the selected audio clip.
                if (_spectrumDrawer != null)
                    _spectrumDrawer.VisualizeSongSpectrum(audioClip);

                return;
            }
            
            PlayModeWarning();
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
            // Update audio control values.
            if (Application.isPlaying && _waveformDrawer != null)
            {
                _waveformDrawer.AudioSource.volume = _volumeSlider.value;
                _waveformDrawer.TimeScrubAmount = _scrubAmoundField.value;
                return;
            }
                
            // Close window if not in playmode.
            Close();
        }

        private void OnGUI()
        {
            // Close window on escape key.
            if (Event.current.type != EventType.KeyDown)
                return;

            if (Event.current.keyCode != KeyCode.Escape)
                return;

            Close();
        }

        private void OnFocus() => PlayModeWarning();

        private void OnDestroy()
        {
            // Remove event listener.
            if (_songList == null)
                return;

            _songList.selectionChanged -= OnSongIndexChanged;
        }
    }
#endif
}
