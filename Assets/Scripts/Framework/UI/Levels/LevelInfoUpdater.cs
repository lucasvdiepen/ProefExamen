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
        MenuStateUpdater.Instance.OnSelectedLevelIDChanged += UpdateLevelInfo;
    }

    private void OnDisable() => MenuStateUpdater.Instance.OnSelectedLevelIDChanged -= UpdateLevelInfo;

    private void UpdateLevelInfo(int levelID) => UpdateLevelInfo();

    private void UpdateLevelInfo()
    {
        LevelData currentLevel = SessionValues.Instance.currentLevel;

        _songCover.sprite = currentLevel.songCover;
        _songTitleText.text = currentLevel.title;
        _songArtistsText.text = currentLevel.artists + " | " + currentLevel.album;
    }
}
