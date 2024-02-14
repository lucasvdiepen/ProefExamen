using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Lane : MonoBehaviour
{
    [SerializeField]
    private int _id = -1;

    [SerializeField]
    private Button _button;

    //Hier kan een Dictionairy bijgehouden worden voor existing targets

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
    }


    public void SpawnNote(float timeStamp)
    {
        GameObject newNote = Instantiate(Settings.note);

        Vector2 initialPos = new Vector2(-2.1f + (_id * 0.7f), 6);
        Vector2 targetPos = new Vector2(initialPos.x, -6);

        newNote.transform.position = new Vector3(initialPos.x, initialPos.y, 0);

        newNote.GetComponent<Note>().SetNoteValues(initialPos, targetPos, _id, Settings.currentLevel.levelID, timeStamp);
    }
}
