using TMPro;
using UnityEngine;

namespace ProefExamen.Framework.PointsSystem
{
    /// <summary>
    /// A class responsible for updating the points text.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PointsTextUpdater : MonoBehaviour
    {
        private TextMeshProUGUI _pointsText;
        private string _defaultPointsText;

        private void Awake()
        {
            _pointsText = GetComponent<TextMeshProUGUI>();
            _defaultPointsText = _pointsText.text;
        }

        private void Start() => UpdatePointsText(PointsSystem.Points);

        private void OnEnable() => PointsSystem.OnPointsChanged += UpdatePointsText;

        private void OnDisable() => PointsSystem.OnPointsChanged -= UpdatePointsText;

        private void UpdatePointsText(int points)
            => _pointsText.text = _defaultPointsText.Replace("[points]", points.ToString());
    }
}