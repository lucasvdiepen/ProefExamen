using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Levels")]
public class Levels : ScriptableObject
{
    [SerializeField]
    Dictionary<int, LevelData> allLevels = new Dictionary<int, LevelData>();
}
