using UnityEngine.UI;
using UnityEngine;
using TMPro;

using ProefExamen.Framework.Gameplay.PerformanceTracking;
using ProefExamen.Framework.StateMachine.States;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.UI;
using System;
using ProefExamen.Framework.Audio;

namespace ProefExamen.Framework.Buttons.LevelSelector
{
    /// <summary>
    /// A class responsible for handling the button that selects a level.
    /// </summary>
    public class SelectLevelButton : BasicButton
    {
        [SerializeField]
        private TextMeshProUGUI _songTitleText;

        [SerializeField]
        private TextMeshProUGUI _songArtistText;

        [SerializeField]
        private Image _songCover;

        [SerializeField]
        private Image _difficultyImage;

        [SerializeField]
        private LevelData _levelData;

        [SerializeField]
        private float _songPreviewVolume = .65f;

        [Header("Difficulty sprites")]
        [SerializeField]
        private Sprite _noDiffSprite;

        [SerializeField]
        private Sprite _easyDiffSprite;

        [SerializeField]
        private Sprite _normalDiffSprite;

        [SerializeField]
        private Sprite _hardDiffSprite;

        private protected override void OnEnable()
        {
            base.OnEnable();
            UpdateDifficultyIcon();
        }

        /// <summary>
        /// Sets the level info of this levels text and images.
        /// </summary>
        /// <param name="levelData">The level data to use info from.</param>
        /// <param name="noDiff">The sprite when there is no beaten difficulty.</param>
        /// <param name="easyDiff">The sprite when the beaten difficulty is easy difficulty.</param>
        /// <param name="normalDiff">The sprite when the beaten difficulty is normal difficulty.</param>
        /// <param name="hardDiff">The sprite when the beaten difficulty is hard difficulty.</param>
        public void SetLevelInfo(LevelData levelData)
        {
            _levelData = levelData;

            _songTitleText.text = levelData.title;
            _songArtistText.text = levelData.album == ""
                ? levelData.artists
                : levelData.artists + " | " + levelData.album;

            _songCover.sprite = levelData.songCover;

            UpdateDifficultyIcon();
        }

        private void UpdateDifficultyIcon()
        {
            if (_songTitleText.text == "")
                return;

            PerformanceResult highScore = new PerformanceResult(_levelData.levelID, Difficulty.Easy);

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                PerformanceResult scoreToCheck =
                    PerformanceTracker.Instance.GetHighScoreFromLevel(_levelData.levelID, difficulty);

                if (scoreToCheck.maxStreak != 0)
                    highScore = scoreToCheck;
            }

            if (highScore.maxStreak == 0)
            {
                _difficultyImage.sprite = _noDiffSprite;
                return;
            }

            switch (highScore.difficulty)
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

        private protected override void OnButtonPressed()
        {
            SessionValues.Instance.SelectLevel(_levelData);
            MenuStateUpdater.Instance.OnSelectedLevelIDChanged?.Invoke(_levelData.levelID);
            
            Framework.StateMachine.StateMachine.Instance.GoToState<PlayLevelState>();
            AudioSystem.Instance.PlaySong(_levelData.song, _songPreviewVolume);
        }
    }
}