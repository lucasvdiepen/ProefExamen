using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class BeatMappingWindow : EditorWindow
{
    private ObjectField soundClipField;
    private string audioClipFilter = "t:AudioClip";
    private ListView songList;

    private AudioSource audioSource;
    GameObject editorAudioSourceObject;

    [MenuItem("Rythm Game/Beat Mapping Tool")]
    public static void CreateWindow()
    {
        BeatMappingWindow window = GetWindow<BeatMappingWindow>();
        window.titleContent = new GUIContent("Beat Mapping Tool");
        window.minSize = new Vector2(450, 200);
        window.maxSize = new Vector2(900, 600);
    }

    public void CreateGUI()
    {
        var allObjectGuids = AssetDatabase.FindAssets(audioClipFilter);
        var allObjects = new List<AudioClip>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        songList = new ListView();
        splitView.Add(songList);
        var rightPane = new VisualElement();
        splitView.Add(rightPane);

        songList.makeItem = () => new Label();
        songList.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        songList.itemsSource = allObjects;

        songList.selectionChanged += OnSongIndexChanged;

        var paddingContainer = new VisualElement();
        paddingContainer.style.paddingTop = 10;

        soundClipField = new ObjectField("Song to map");
        soundClipField.SetEnabled(false);
        soundClipField.objectType = typeof(AudioClip);

        rightPane.Add(soundClipField);

        editorAudioSourceObject = EditorUtility.CreateGameObjectWithHideFlags("Audio Source", HideFlags.HideAndDontSave, typeof(AudioSource));
        audioSource = editorAudioSourceObject.GetComponent<AudioSource>();

        rootVisualElement.Add(paddingContainer);
    }

    private void OnSongIndexChanged(IEnumerable<object> obj)
    {
        var objectList = obj.ToList();
        if (objectList.Count == 0)
            return;

        if (objectList[0] is not AudioClip audioClip || EditorApplication.isPlaying)
            return;

        soundClipField.value = audioClip;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Space)
            {
                if (audioSource.isPlaying) audioSource.Pause();
                else audioSource.UnPause();
            }

            if (Event.current.keyCode == KeyCode.Escape)
                Close();
        }
    }

    private void OnDestroy()
    {
        songList.selectionChanged -= OnSongIndexChanged;
        audioSource.Stop();
        DestroyImmediate(editorAudioSourceObject);
    }
}