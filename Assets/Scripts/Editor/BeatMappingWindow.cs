using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
public class BeatMappingWindow : EditorWindow
{
    private readonly string audioClipFilter = "t:AudioClip";

    private ObjectField _soundClipField;
    private ListView _songList;
    private ListView _timeStamps;

    [MenuItem("Rythm Game/Beat Mapping Tool")]
    public static void CreateWindow()
    {
        BeatMappingWindow window = GetWindow<BeatMappingWindow>();
        window.titleContent = new GUIContent("Beat Mapping Tool");
        window.minSize = new Vector2(600, 450);
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

        string[] allObjectGuids = AssetDatabase.FindAssets(audioClipFilter);
        List<AudioClip> allObjects = new();
        
        foreach (string guid in allObjectGuids)
            allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));

        TwoPaneSplitView splitView = new(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        _songList = new ListView();
        splitView.Add(_songList);

        VisualElement rightPane = new();
        splitView.Add(rightPane);

        Vector2[] myArray = new Vector2[] { Vector2.zero, Vector2.one, Vector2.right, Vector2.left, Vector2.down };
        ListView arrayListView = new();
        rightPane.Add(arrayListView);

        arrayListView.makeItem = () => new Label();
        arrayListView.bindItem = (item, index) => { (item as Label).text = myArray[index].ToString(); };
        arrayListView.itemsSource = myArray;

        _songList.makeItem = () => new Label();
        _songList.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        _songList.itemsSource = allObjects;

        _songList.selectionChanged += OnSongIndexChanged;
        _songList.style.paddingBottom = 30;

        _soundClipField = new ObjectField("Song to map");
        _soundClipField.SetEnabled(false);
        _soundClipField.objectType = typeof(AudioClip);
        
        _songList.hierarchy.Add(_soundClipField);
       
        var paddingContainer = new VisualElement();
        paddingContainer.style.paddingTop = 10;
        rootVisualElement.Add(paddingContainer);
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
            if(spectrumDrawer != null)
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
