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
    private ListView _timeStampView;
    private TwoPaneSplitView _splitView;
    private VisualElement _rightPanel;

    private Vector2[] _timeStampArray = new Vector2[10];

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

        _splitView = new(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(_splitView);
        

        CreateSongListView();
        _rightPanel = new ScrollView();
        _splitView.Add(_rightPanel);

        CreateTimeStampArrayView();
        CreateCustomArrayButtons();

        var paddingContainer = new VisualElement();
        paddingContainer.style.paddingTop = 10;
        rootVisualElement.Add(paddingContainer);
    }

    private void CreateCustomArrayButtons()
    {
        var addButton = new Button() { text = "Add Stamp" };
        var removeButton = new Button() { text = "Remove Last Stamp" };
        var clearButton = new Button() { text = "Clear List" };

        addButton.clicked += OnClickAdButton;
        removeButton.clicked += OnClickRemoveButton;
        clearButton.clicked += OnClickClearButton;

        var buttonContainer = new VisualElement();
        buttonContainer.style.flexDirection = FlexDirection.Row;

        buttonContainer.style.alignSelf = Align.Center;
        buttonContainer.style.paddingBottom = 30;

        buttonContainer.Add(addButton);
        buttonContainer.Add(removeButton);
        buttonContainer.Add(clearButton);

        _rightPanel.Add(buttonContainer);
    }


    private void OnClickRemoveButton() => throw new System.NotImplementedException();
    private void OnClickAdButton() => throw new System.NotImplementedException();
    private void OnClickClearButton()
    {
        throw new System.NotImplementedException();

        /*_timeStampArray = new Vector2[0];
        arrayListView.itemsSource = _timeStampArray;
        arrayListView.Rebuild();*/
    }

    private void CreateTimeStampArrayView()
    {
        _timeStampView = new();
        _rightPanel.Add(_timeStampView);

        _timeStampView.makeItem = () => new Vector2Field();
        _timeStampView.bindItem = (item, index) => { (item as Vector2Field).value = _timeStampArray[index]; };
        _timeStampView.itemsSource = _timeStampArray;
    }

    private void CreateSongListView()
    {
        string[] allObjectGuids = AssetDatabase.FindAssets(audioClipFilter);
        List<AudioClip> allObjects = new();

        foreach (string guid in allObjectGuids)
            allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));

        _songList = new ListView();
        _splitView.Add(_songList);

        _songList.makeItem = () => new Label();
        _songList.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        _songList.itemsSource = allObjects;

        _songList.selectionChanged += OnSongIndexChanged;
        _songList.style.paddingTop = 30;

        _soundClipField = new ObjectField("Song to map");
        _soundClipField.SetEnabled(false);
        _soundClipField.objectType = typeof(AudioClip);

        _songList.hierarchy.Add(_soundClipField);
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
