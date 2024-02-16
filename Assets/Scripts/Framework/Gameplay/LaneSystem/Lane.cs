using UnityEngine;
using UnityEngine.UI;
using ProefExamen.Framework.Gameplay.Settings;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class Lane : MonoBehaviour
{
    [SerializeField]
    private int _id = -1;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private List<GameObject> _notes = new();

    void Start()
    {
        _button = GetComponent<Button>();

        if (_id != -1)
        {
            _button.onClick.AddListener(SendButtonPress);
        }
        else
            Debug.LogError("Lane with button " + _button.name + " has no assigned ID!");
    }

    private void SendButtonPress()
    {
        //Hier kan score toegevoegd worden aan de LaneManager
        Debug.Log("Button " + _button.name + " was pressed!");

        GameObject nextNote = _notes[0];
        Note nextNoteScript = nextNote.GetComponent<Note>();

        if (nextNoteScript.LerpAlpha > .4f && nextNoteScript.LerpAlpha < .6f)
        {
            float diff = 1 - (2 * Mathf.Abs(nextNoteScript.LerpAlpha));

            int score = Mathf.RoundToInt(diff * 10f);

        }
    }

    public void Update()
    {
        float x = transform.position.x - (_button.GetComponent<RectTransform>().rect.width / 2);

        float targetX = transform.position.x + (_button.GetComponent<RectTransform>().rect.width / 2);

        Debug.DrawLine(new Vector2(x, transform.position.y), new Vector2(targetX, transform.position.y));
    }

    public void SpawnNote(float timeStamp)
    {
        GameObject newNote = Instantiate(Settings.note);
        _notes.Add(newNote);

        Vector2 initialPos = new Vector2(-2.1f + ((_id * 0.7f) * 2), 6);
        Vector2 targetPos = new Vector2(initialPos.x, -6);

        newNote.transform.position = new Vector3(initialPos.x, initialPos.y, 0);

        newNote.GetComponent<Note>().SetNoteValues(initialPos, targetPos, _id, Settings.currentLevel.levelID, timeStamp);
    }
}
