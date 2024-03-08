using UnityEngine;

using ProefExamen.Framework.Buttons;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Gameplay.Values;
using ProefExamen.Framework.UI;

namespace ProefExamen.Framework.Buttons.LevelSelector
{
    /// <summary>
    /// A BasicButton class that is responsible for passing a new Difficulty when pressed.
    /// </summary>
    public class SelectDifficultyButton : BasicButton
    {
        [Header("Difficulty to set when pressed")]
        [SerializeField]
        private Difficulty _difficulty;

        [Header("Visuals")]
        [SerializeField]
        private Sprite _nonSelectedSprite;

        [SerializeField]
        private Sprite _selectedSprite;

        private protected override void OnEnable()
        {
            base.OnEnable();
            ChangeVisualState(SessionValues.Instance.difficulty);
            MenuStateUpdater.Instance.OnDifficultyChanged += ChangeVisualState;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            MenuStateUpdater.Instance.OnDifficultyChanged -= ChangeVisualState;
        }

        private void ChangeVisualState(Difficulty newDifficulty)
        {
            if (newDifficulty == _difficulty)
            {
                Debug.Log("This selected now");
                Image.sprite = _selectedSprite;
                return;
            }

            Image.sprite = _nonSelectedSprite;
            Debug.Log("Not selected now");
        }
        private protected override void OnButtonPressed()
        {
            SessionValues.Instance.difficulty = _difficulty;
            MenuStateUpdater.Instance.OnDifficultyChanged?.Invoke(_difficulty);
        }
    }
}
