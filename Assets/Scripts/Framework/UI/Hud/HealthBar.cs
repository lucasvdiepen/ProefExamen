using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

using ProefExamen.Framework.Gameplay.PerformanceTracking;

namespace ProefExamen.Framework.Hud
{
    /// <summary>
    /// Class responsible for updating the player health bar
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image _primaryBar, _secondaryBar;

        [SerializeField]
        private PerformanceTracker _performanceTracker;

        [SerializeField, Space]
        private float _secundaryDelay = 1.0f;

        [SerializeField]
        private float _secundaryResetDuration = .4f;

        [SerializeField]
        private Ease _ease = Ease.Linear;

        [SerializeField]
        private RectTransform _rectTransform;
        
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private float _testMaxHealth = 1000;

        [SerializeField]
        private float _testCurrentHealth = 1000;

        private void Awake()
        {
            //_performanceTracker.OnHealthChanged += UpdateHealthBar;

            _primaryBar.fillAmount = 1;
            _secondaryBar.fillAmount = 1;
        }

        [ContextMenu("Debug Deplete Health")]
        public void DebugHealthDeplete()
        {
            UpdateHealthBar(_testCurrentHealth -= 200);
        }

        public void UpdateHealthBar(float currentHealth)
        {
            //float max = _performanceTracker.MaxHealth;

            float max = _testMaxHealth;
            float alpha = currentHealth / max;

            _primaryBar.fillAmount = alpha;

            DOTween.Sequence()
                .AppendInterval(_secundaryDelay)
                .Append(_secondaryBar.DOFillAmount(alpha, _secundaryResetDuration))
                .SetEase(_ease);
        }

        private void OnDestroy()
        {
            //_performanceTracker.OnHealthChanged -= UpdateHealthBar;
        }
    }
}