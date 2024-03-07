using System.Collections.Generic;
using UnityEngine;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Buttons.LevelSelector;
using ProefExamen.Framework.UI;

namespace ProefExamen.Framework.UI.LevelSelector
{
    public class LevelSelectorSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _levelSelectorPrefab;

        [SerializeField]
        private Transform _levelSelectorParent;

        private readonly List<GameObject> _levelSelectors = new();

        private void OnEnable() => SpawnLevelSelectors();

        private void SpawnLevelSelectors()
        {
            if (_levelSelectors.Count > 0)
                return;

            foreach(LevelData levelData in SessionValues.Instance.Levels.levels)
            {
                GameObject newLevelSelector = Instantiate(_levelSelectorPrefab, _levelSelectorParent);
                newLevelSelector.GetComponent<SelectLevelButton>().levelID = levelData.levelID;

                int visualLevelID = levelData.levelID + 1;

                newLevelSelector.GetComponentInChildren<TextUpdater>().ReplaceTag(visualLevelID.ToString());
                _levelSelectors.Add(newLevelSelector);
            }
        }
    }
}