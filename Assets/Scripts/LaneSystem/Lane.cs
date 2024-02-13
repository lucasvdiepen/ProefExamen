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
}
