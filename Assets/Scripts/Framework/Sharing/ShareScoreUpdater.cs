using UnityEngine.UIElements;
using UnityEngine;
using TMPro;

using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.UI.TextUpdaters;

namespace ProefExamen.Framework.Sharing
{
    /// <summary>
    /// A class that is responsible for receiving and distributing information to share.
    /// </summary>
    public class ShareScoreUpdater : MonoBehaviour
    {
        [Header("Difficulty Sprites")]
        [SerializeField]
        private Sprite _easyDiffSprite;

        [SerializeField]
        private Sprite _normalDiffSprite;

        [SerializeField]
        private Sprite _hardDiffSprite;

        [Header("Score")]
        [SerializeField]
        private TextUpdater _scoreText;

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

        [Header("Difficulty")]
        [SerializeField]
        private Image _difficultyImage;

        /// <summary>
        /// A function that sets the PerformanceResult data on the share screen.
        /// </summary>
        /// <param name="newResult">The performance data to use when updating text.</param>
        public void SetTargetResult(PerformanceResult newResult)
        {
            _scoreText.ReplaceTag(newResult.totalScore.ToString());

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
