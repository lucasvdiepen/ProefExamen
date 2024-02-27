using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
        string[] allObjectGuids = AssetDatabase.FindAssets(audioClipFilter);
        List<AudioClip> allObjects = new();

        foreach (string guid in allObjectGuids)
            allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));

        TwoPaneSplitView splitView = new(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        _songList = new ListView();
        splitView.Add(_songList);

        VisualElement rightPane = new ScrollView(); // Changed to ScrollView for scrolling functionality
        splitView.Add(rightPane);

        ListView arrayListView = new();
        rightPane.Add(arrayListView);

        Vector2[] myArray = new Vector2[50]; // Reduced the array size for demonstration purposes

        for (int i = 0; i < 50; i++)
        {
            myArray[i] = Random.value > .5 ? Vector2.down : Vector2.up;
        }

        arrayListView.makeItem = () => new Vector2Field();
        arrayListView.bindItem = (item, index) => { (item as Vector2Field).value = myArray[index]; };
        arrayListView.itemsSource = myArray;

        var addButton = new Button() { text = "Add Stamp" };
        var removeButton = new Button() { text = "Remove Last Stamp" };
        var clearButton = new Button() { text = "Clear List" };

        addButton.clicked += () =>
        {
            Vector2[] newArray = new Vector2[myArray.Length + 1];

            for (int i = 0; i < newArray.Length; i++)
            {
                if (i == newArray.Length)
                    continue;

                newArray[Mathf.Abs(i - 1)] = myArray[Mathf.Abs(i - 1)];
            }

            myArray = newArray;

            arrayListView.itemsSource = myArray;
            arrayListView.Rebuild();
        };

        removeButton.clicked += () =>
        {
            if (myArray.Length > 0)
            {
                Vector2[] newArray = new Vector2[myArray.Length - 1];

                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = myArray[i];
                }

                myArray = newArray;

                arrayListView.itemsSource = myArray;
                arrayListView.Rebuild();
            }
        };

        clearButton.clicked += () =>
        {
            myArray = new Vector2[0];
            arrayListView.itemsSource = myArray;
            arrayListView.Rebuild();
        };

        var buttonContainer = new VisualElement();
        buttonContainer.style.flexDirection = FlexDirection.Row;
        
        buttonContainer.style.alignSelf = Align.Center;
        buttonContainer.style.paddingBottom = 30;

        buttonContainer.Add(addButton);
        buttonContainer.Add(removeButton);
        buttonContainer.Add(clearButton);

        // Add the container to your right pane
        rightPane.Add(buttonContainer);

        _songList.makeItem = () => new Label();
        _songList.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        _songList.itemsSource = allObjects;

        _songList.selectionChanged += OnSongIndexChanged;
        _songList.style.paddingTop = 30;

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
