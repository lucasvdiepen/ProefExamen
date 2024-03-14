using UnityEngine;
using System.Collections;
using TMPro;

using ProefExamen.Framework.StateMachine.Attributes;
using ProefExamen.Framework.Gameplay.PerformanceTracking;
using UnityEngine.UI;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for winning.
    /// </summary>
    [ParentState(typeof(GameState))]
    public class WinState : MenuState
    {
        [SerializeField]
        private TextMeshProUGUI _stickerText;

        [SerializeField]
        private string _levelClearedText = "Level cleared!";

        [SerializeField]
        private string _newHighscoreText = "New highscore!";

        [SerializeField, Space]
        private RawImage _gameStateView;

        [SerializeField]
        private RawImage _winStateView;

        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            if(PerformanceTracker.Instance.LastCompletionStatus == ScoreCompletionStatus.Beaten)
            {
                _stickerText.text = _newHighscoreText;
                yield break;
            }

            _stickerText.text = _levelClearedText;

            _gameStateView.gameObject.SetActive(false);
            _winStateView.gameObject.SetActive(true);
        }

        public override IEnumerator OnStateExit()
        {
            yield return base.OnStateExit();

            _gameStateView.gameObject.SetActive(true);
            _winStateView.gameObject.SetActive(false);
        }
    }
}