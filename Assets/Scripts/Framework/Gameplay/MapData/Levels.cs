using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.MapData
{
    /// <summary>
    /// A scriptable object holding a list of levels.
    /// </summary>
    [CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Levels")]
    public class Levels : ScriptableObject
    {
        public List<LevelData> levels = new();
    }
}
