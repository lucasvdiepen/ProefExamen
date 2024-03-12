using System.Collections.Generic;
using UnityEngine;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Buttons.LevelSelector;

namespace ProefExamen.Framework.UI.Level
{
    /// <summary>
    /// Class responsible for spawning level selectors.
    /// </summary>
    public class LevelSelectorSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject _levelSelectorPrefab;

        [SerializeField]
        private Transform _levelSelectorParent;

        [Header("Difficulty sprites")]
        [SerializeField]
        private Sprite noDifficulty;

        [SerializeField]
        private Sprite diffEasy;

        [SerializeField]
        private Sprite diffNormal;

        [SerializeField]
        private Sprite diffHard;

        private readonly List<GameObject> _levelSelectors = new();

        private void OnEnable() => SpawnLevelSelectors();

        private void SpawnLevelSelectors()
        {
            if (_levelSelectors.Count > 0)
                return;

            int listLength = SessionValues.Instance.Levels.levels.Count;
            for(int i = 0; i < listLength; i++)
            {
                LevelData levelData = SessionValues.Instance.Levels.levels[i];

                GameObject newLevelSelector = Instantiate(_levelSelectorPrefab, _levelSelectorParent);
                newLevelSelector.GetComponent<SelectLevelButton>().SetLevelInfo
                    (
                        levelData,
                        noDifficulty,
                        diffEasy,
                        diffNormal,
                        diffHard
                    );

                _levelSelectors.Add(newLevelSelector);
            }
        }
    }
}