using UnityEngine;
using UnityEngine.EventSystems;

using ProefExamen.Framework.Gameplay.LaneSystem;

namespace ProefExamen.Framework.Buttons.LaneManager
{
    /// <summary>
    /// A class responsible for handling the button press of the lane.
    /// </summary>
    [RequireComponent(typeof(Lane))]
    public class LaneButton : MonoBehaviour, IPointerDownHandler
    {
        private Lane _lane;

        private void Awake() => _lane = GetComponent<Lane>();

        /// <summary>
        /// Called when the button is pressed down.
        /// </summary>
        /// <param name="eventData">Event data of the pointer down event.</param>
        public void OnPointerDown(PointerEventData eventData) => _lane.OnButtonPressed();
    }
}