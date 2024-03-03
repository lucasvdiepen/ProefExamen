using ProefExamen.Framework.UI;
using TMPro;
using UnityEngine;

namespace ProefExamen.Framework.PointsSystem
{
    /// <summary>
    /// A class responsible for updating the points text.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PointsTextUpdater : TextUpdater
    {
        private void Start() => UpdatePointsText(PointsSystem.Points);

        private void OnEnable() => PointsSystem.OnPointsChanged += UpdatePointsText;

        private void OnDisable() => PointsSystem.OnPointsChanged -= UpdatePointsText;

        private void UpdatePointsText(int points) => ReplaceTag(points.ToString());
    }
}