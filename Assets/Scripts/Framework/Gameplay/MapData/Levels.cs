using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.MapData
{
    [CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Levels")]
    public class Levels : ScriptableObject
    {
        public List<LevelData> levels = new();
    }
}
