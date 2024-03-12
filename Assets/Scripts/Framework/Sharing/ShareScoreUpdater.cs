using UnityEngine.UI;
using UnityEngine;
using TMPro;

using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.UI.TextUpdaters;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Utils;

namespace ProefExamen.Framework.Sharing
{
    /// <summary>
    /// A class that is responsible for receiving and distributing information to share.
    /// </summary>
    public class ShareScoreUpdater : AbstractSingleton<ShareScoreUpdater>
    {
        [Header("Difficulty Sprites")]
        [SerializeField]
        private Sprite _easyDiffSprite;

        [SerializeField]
        private Sprite _normalDiffSprite;

        [SerializeField]
        private Sprite _hardDiffSprite;

        [SerializeField]
        private Image _difficultyImage;

        [Header("Song Information")]
        [SerializeField]
        private Image _songCover;

        [SerializeField]
        private TextMeshProUGUI _songTitle;

        [SerializeField]
        private TextMeshProUGUI _songArtists;

        [Header("Score")]
        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [Header("Hits")]
        [SerializeField]
        private TextUpdater _maxStreakText;

        [SerializeField]
        private TextUpdater _perfectHitsText;

        [SerializeField]
        private TextUpdater _niceHitsText;

        [SerializeField]
        private TextUpdater _alrightHitsText;

        [SerializeField]
        private TextUpdater _okHitsText;

        [SerializeField]
        private TextUpdater _missesText;

        /// <summary>
        /// A function that sets the PerformanceResult data on the share screen.
        /// </summary>
        /// <param name="newResult">The performance data to use when updating text.</param>
        public void SetTargetResult(PerformanceResult newResult)
        {
            LevelData resultLevelData = SessionValues.Instance.GetLevelData(newResult.levelID);

            _songCover.sprite = resultLevelData.songCover;
            _songTitle.text = resultLevelData.title;
            _songArtists.text = 
            (
                resultLevelData.album == ""
                ? resultLevelData.artists
                : resultLevelData.artists + " | " + resultLevelData.album
            );

            _scoreText.text = newResult.totalScore.ToString();

            _maxStreakText.ReplaceTag(newResult.maxStreak.ToString());

            _perfectHitsText.ReplaceTag(newResult.perfectHits.ToString());
            _niceHitsText.ReplaceTag(newResult.niceHits.ToString());
            _alrightHitsText.ReplaceTag(newResult.alrightHits.ToString());
            _okHitsText.ReplaceTag(newResult.okHits.ToString());
            _missesText.ReplaceTag((newResult.misses + newResult.missClicks).ToString());

            switch (newResult.difficulty)
            {
                case Difficulty.Easy:
                    _difficultyImage.sprite = _easyDiffSprite;
                    break;
                case Difficulty.Normal:
                    _difficultyImage.sprite = _normalDiffSprite;
                    break;
                case Difficulty.Hard:
                    _difficultyImage.sprite = _hardDiffSprite;
                    break;
            }
        }
    }
}
