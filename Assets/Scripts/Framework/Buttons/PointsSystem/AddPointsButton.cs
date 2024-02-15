using UnityEngine;

namespace ProefExamen.Framework.Buttons.PointsSystem
{
    /// <summary>
    /// A class that adds points to the points system when the button is pressed for testing purposes.
    /// </summary>
    public class AddPointsButton : BasicButton
    {
        [SerializeField]
        private int _pointsToAdd = 1;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnButtonPressed() => Framework.PointsSystem.PointsSystem.Points += _pointsToAdd;
    }
}
