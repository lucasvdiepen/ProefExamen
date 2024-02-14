using TMPro;
using UnityEngine;

namespace ProefExamen.Framework.PointsSystem
{
    /// <summary>
    /// A class responsible for keeping track of the points of the player.
    /// </summary>
    public class PointsSystem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _highscoreText;

        private int _points;

        /// <summary>
        /// The current points of the player.
        /// </summary>
        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                _highscoreText.text = _points.ToString();
            }
        }
    }
}