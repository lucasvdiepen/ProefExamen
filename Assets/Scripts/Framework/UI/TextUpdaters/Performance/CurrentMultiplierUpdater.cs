using ProefExamen.Framework.Gameplay.PerformanceTracking;
using Unity.VisualScripting;
using UnityEngine;

namespace ProefExamen.Framework.UI.TextUpdaters.Performance
{
    /// <summary>
    /// A TextUpdater class that is responsible for updating the Multiplier text.
    /// </summary>
    public class CurrentMultiplierUpdater : TextUpdater
    {
        [SerializeField]
        private Animator _animator;

        private void OnEnable()
        {
            UpdateMultiplierText(PerformanceTracker.Instance.ScoreMultiplier);
            PerformanceTracker.Instance.OnMultiplierChanged += UpdateMultiplierText;
        }

        private void OnDisable() => PerformanceTracker.Instance.OnMultiplierChanged -= UpdateMultiplierText;

        private void UpdateMultiplierText(int multiplier)
        {
            _animator.SetInteger("multiplier", multiplier);
            
            if(_animator.gameObject.activeSelf != (multiplier != 1))
                _animator.gameObject.SetActive(multiplier != 1);

            ReplaceTag(multiplier.ToString());
        }
    }
}