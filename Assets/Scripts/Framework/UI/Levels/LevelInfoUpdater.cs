using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.UI;

public class LevelInfoUpdater : MonoBehaviour
{
    [SerializeField]
    private Image _songCover;

    [SerializeField]
    private TextMeshProUGUI _songTitleText;

    [SerializeField]
    private TextMeshProUGUI _songArtistsText;

    private void OnEnable()
    {
        UpdateLevelInfo();
        ResetLevelDifficulty();
        MenuStateUpdater.Instance.OnSelectedLevelIDChanged += UpdateLevelInfo;
    }

    private void OnDisable() => MenuStateUpdater.Instance.OnSelectedLevelIDChanged -= UpdateLevelInfo;

    private void ResetLevelDifficulty()
    {
        foreach(MappingData mapData in SessionValues.Instance.currentLevel.mappingData)
        {
            SessionValues.Instance.difficulty = mapData.difficulty;
            MenuStateUpdater.Instance.OnDifficultyChanged?.Invoke(mapData.difficulty);

            return;
        }
    }

    private void UpdateLevelInfo(int levelID) => UpdateLevelInfo();

    private void UpdateLevelInfo()
    {
        LevelData currentLevel = SessionValues.Instance.currentLevel;

        Debug.Log("Epic changes");

        _songCover.sprite = currentLevel.songCover;
        _songTitleText.text = currentLevel.title;
        _songArtistsText.text = currentLevel.artists;

        if(currentLevel.album != "")
            _songArtistsText.text = _songArtistsText.text + " | " + currentLevel.album;
    }
}
