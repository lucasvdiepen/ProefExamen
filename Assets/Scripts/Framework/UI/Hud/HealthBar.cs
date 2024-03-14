using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.UI.Hud
{
    /// <summary>
    /// Class responsible for updating the player health bar.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image _primaryBar;

        [SerializeField]
        private Image _secondaryBar;

        [SerializeField, Space]
        private float _secundaryDelay = 1.0f;

        [SerializeField]
        private float _secundaryResetDuration = .4f;

        [SerializeField]
        private Ease _ease = Ease.InOutBounce;

        [SerializeField]
        private RectTransform _rectTransform;

        private void OnEnable() => PerformanceTracker.Instance.OnHealthChanged += UpdateHealthBar;

        private void Awake()
        {
            _primaryBar.fillAmount = 1;
            _secondaryBar.fillAmount = 1;
        }

        /// <summary>
        /// Method responsible for updating health bar visual.
        /// </summary>
        /// <param name="currentHealth">Current player health.</param>
        public void UpdateHealthBar(float currentHealth)
        {
            float max = PerformanceTracker.Instance.MaxHealth;
            float alpha = currentHealth / max;

            _primaryBar.fillAmount = alpha;

            DOTween.Sequence()
                .AppendInterval(_secundaryDelay)
                .Append(_secondaryBar.DOFillAmount(alpha, _secundaryResetDuration))
                .SetEase(_ease);
        }

        private void OnDisable() => PerformanceTracker.Instance.OnHealthChanged -= UpdateHealthBar;
    }
}