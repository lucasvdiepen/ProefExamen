using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class BeatMappingWindow : EditorWindow
{
    private readonly string audioClipFilter = "t:AudioClip";

    private ObjectField _soundClipField;
    private ListView _songList;

    [MenuItem("Rythm Game/Beat Mapping Tool")]
    public static void CreateWindow()
    {
        BeatMappingWindow window = GetWindow<BeatMappingWindow>();
        window.titleContent = new GUIContent("Beat Mapping Tool");

        window.minSize = new Vector2(50, 100);
    }

    public void CreateGUI()
    {
        if (!Application.isPlaying)
        {
            Label label = new("Enter play-mode");
            rootVisualElement.Add(label);

            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 25;

            label.style.paddingTop = 225;
            label.style.color = Color.red;

            rootVisualElement.style.backgroundColor = new Color(.74f, .74f, .74f, .1f);
            PlayModeCheck();
            return;
        }

        CreateSongListView();

        _soundClipField = new ObjectField("Selected Song");
        _soundClipField.SetEnabled(false);
        _soundClipField.objectType = typeof(AudioClip);

        rootVisualElement.hierarchy.Add(_soundClipField);

        var paddingContainer = new VisualElement();
        paddingContainer.style.paddingTop = 10;
        rootVisualElement.Add(paddingContainer);
    }

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

    private void OnSongIndexChanged(IEnumerable<object> obj)
    {
        List<object> objectList = obj.ToList();
        if (objectList.Count == 0)
            return;

        if (objectList[0] is not AudioClip audioClip)
            return;

        PlayModeCheck();
        _soundClipField.value = audioClip;

        if (Application.isPlaying)
        {
            AudioSpectrumDrawer spectrumDrawer = FindObjectOfType<AudioSpectrumDrawer>();
            if (spectrumDrawer != null)
                spectrumDrawer.VisualizeSongSpectrum(audioClip);
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
        => PlayModeCheck();

    private void PlayModeCheck()
    {
        if (!Application.isPlaying)
            Debug.LogWarning("Enter play-mode to use BeatMapping Tool");
    }

    private void OnDestroy()
    {
        if (_songList == null)
            return;

        _songList.selectionChanged -= OnSongIndexChanged;
    }
}
#endif
