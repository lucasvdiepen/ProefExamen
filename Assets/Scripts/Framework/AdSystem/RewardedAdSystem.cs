using ProefExamen.Framework.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ProefExamen.Framework.AdSystem
{
    /// <summary>
    /// A class responsible for showing and handling rewarded ads.
    /// </summary>
    public class RewardedAdSystem : AbstractSingleton<RewardedAdSystem>
    {
        [SerializeField]
        private GameObject _adCanvas;

        [SerializeField]
        private TextMeshProUGUI _countdownText;

        [SerializeField]
        private GameObject _continueButton;

        [SerializeField]
        private GameObject _exitButton;

        [SerializeField]
        private int _adDuration = 15;

        private bool _isAdShowing;
        private Action<bool> _callback;
        private Coroutine _countdownCoroutine;
        private string _defaultCountdownText;

        private void Awake() => _defaultCountdownText = _countdownText.text;

        /// <summary>
        /// Shows the ad. When the ad is finished, the callback is called with a boolean indicating if the ad was successful.
        /// </summary>
        /// <param name="callback">The callback to be called when the ad is finished. The boolean indicates if the ad was successful.</param>
        public void ShowAd(Action<bool> callback)
        {
            if (_isAdShowing)
                return;

            _isAdShowing = true;
            _callback = callback;

            _countdownText.gameObject.SetActive(true);
            _continueButton.SetActive(false);
            _exitButton.SetActive(true);
            _adCanvas.SetActive(true);

            ChangeCountdownText(_adDuration);

            _countdownCoroutine = StartCoroutine(StartAdCountdown());
        }

        /// <summary>
        /// Called when the exit button is pressed.
        /// </summary>
        public void ExitButtonPressed() => CloseAd(false);

        /// <summary>
        /// Called when the continue button is pressed.
        /// </summary>
        public void ContinueButtonPressed() => CloseAd(true);

        private void CloseAd(bool success)
        {
            StopCoroutine(_countdownCoroutine);
            _adCanvas.SetActive(false);

            _callback?.Invoke(success);

            _isAdShowing = false;
        }

        private IEnumerator StartAdCountdown()
        {
            int countdown = _adDuration;

            while (countdown > 0)
            {
                yield return new WaitForSeconds(1f);

                countdown--;

                ChangeCountdownText(countdown);
            }

            _countdownText.gameObject.SetActive(false);
            _continueButton.SetActive(true);
            _exitButton.SetActive(false);
        }

        private void ChangeCountdownText(int countdown)
            => _countdownText.text = _defaultCountdownText.Replace("[countdown]", countdown.ToString());
    }
}