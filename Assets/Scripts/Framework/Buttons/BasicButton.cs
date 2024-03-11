using UnityEngine;
using UnityEngine.UI;

namespace ProefExamen.Framework.Buttons
{
    /// <summary>
    /// An abstract class that is responsible for handling the button click event.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class BasicButton : MonoBehaviour
    {
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(OnButtonPressed);

        private void OnDisable() => _button.onClick.RemoveListener(OnButtonPressed);

        /// <summary>
        /// The method that is called when the button is pressed.
        /// </summary>
        private protected abstract void OnButtonPressed();
    }
}