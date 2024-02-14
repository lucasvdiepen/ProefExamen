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
    public class ShareHighscore : MonoBehaviour
    {
        [SerializeField]
        private Camera _renderCamera;

        [SerializeField]
        private TextMeshProUGUI _highscoreText;

        private string _highscoreDefaultText;
        private int _points;

        private void Awake() => _highscoreDefaultText = _highscoreText.text;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _points++;
                StartCoroutine(ShareHighscoreScreenshot(_points));
            }
        }

        private IEnumerator ShareHighscoreScreenshot(int points)
        {
            _highscoreText.text = _highscoreDefaultText.Replace("[points]", points.ToString());

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
                .SetSubject("Look at my highscore").SetText("Try to beat my highscore!")
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
        }
    }
}