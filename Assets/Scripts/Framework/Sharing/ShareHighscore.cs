using ProefExamen.Framework.Utils;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

namespace ProefExamen.Framework.Sharing
{
    /// <summary>
    /// A class responsible for sharing the highscore of the player.
    /// </summary>
    public class ShareHighscore : AbstractSingleton<ShareHighscore>
    {
        [SerializeField]
        private Camera _renderCamera;

        [SerializeField]
        private TextMeshProUGUI _highscoreText;

        [SerializeField]
        private string _subject;

        [SerializeField]
        private string _text;

        private string _highscoreDefaultText;

        private void Awake() => _highscoreDefaultText = _highscoreText.text;

        /// <summary>
        /// Shares the highscore of the player.
        /// </summary>
        public void ShareHighscoreScreenshot() => StartCoroutine(ShareHighscoreScreenshotCoroutine());

        private IEnumerator ShareHighscoreScreenshotCoroutine()
        {
            int currentPoints = PointsSystem.PointsSystem.Points;

            _highscoreText.text = _highscoreDefaultText
                .Replace("[points]", currentPoints.ToString())
                .Replace("[pointsText]", currentPoints == 1 ? "point" : "points");

            yield return new WaitForEndOfFrame();

            RenderTexture renderTexture = new(Screen.width, Screen.height, 0)
            {
                name = "Screenshot Render Texture",
                enableRandomWrite = true,
                dimension = UnityEngine.Rendering.TextureDimension.Tex2D
            };
            renderTexture.Create();
            _renderCamera.targetTexture = renderTexture;

            _renderCamera.Render();

            Texture2D screenshot = renderTexture.ToTexture2D();

            _renderCamera.targetTexture = null;

            string filePath = Path.Combine(Application.temporaryCachePath, "highscore_screenshot.png");
            File.WriteAllBytes(filePath, screenshot.EncodeToPNG());

            renderTexture.Release();
            Destroy(screenshot);

            new NativeShare().AddFile(filePath)
                .SetSubject(_subject).SetText(_text)
                .Share();
        }
    }
}