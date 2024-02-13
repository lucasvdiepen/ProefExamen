using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : AbstractSingleton<LaneManager>
{
    [SerializeField]
    private int selectedLevelID;

    void Start()
    {
        
    }

    public void UpdateSelectedLevel(int id)
    {
        selectedLevelID = id;
    }
}
