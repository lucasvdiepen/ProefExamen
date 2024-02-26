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

    [MenuItem("Rythm Game/Beat Mapping Tool")]
    public static void CreateWindow()
    {
        BeatMappingWindow window = GetWindow<BeatMappingWindow>();
        window.titleContent = new GUIContent("Beat Mapping Tool");
        
        window.minSize = new Vector2(300, 450);
        window.maxSize = new Vector2(300, 450);
    }
    
    public void CreateGUI()
    {
        if (!Application.isPlaying)
        {
            Label label = new Label("Enter play-mode");
            rootVisualElement.Add(label);
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 25;
            label.style.paddingTop = 250;
            label.style.color = Color.red;

            rootVisualElement.style.backgroundColor = new Color(.74f, .74f, .74f, .1f);
            return;
        }
        
        var allObjectGuids = AssetDatabase.FindAssets(audioClipFilter);
        var allObjects = new List<AudioClip>();
        
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        _songList = new ListView();
        splitView.Add(_songList);

        var rightPane = new VisualElement();
        splitView.Add(rightPane);

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
        var objectList = obj.ToList();
        if (objectList.Count == 0)
            return;

        if (objectList[0] is not AudioClip audioClip)
            return;

        _soundClipField.value = audioClip;
        if (Application.isPlaying)
        {
            AudioSpectrumDrawer spectrumDrawer = FindAnyObjectByType<AudioSpectrumDrawer>();
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
    
    private void OnDestroy()
    {
        if (_songList == null)
            return;

        _songList.selectionChanged -= OnSongIndexChanged;
    }
}
#endif
