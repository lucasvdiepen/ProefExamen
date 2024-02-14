using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareHighscore : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "highscore_screenshot.png");
        File.WriteAllBytes(filePath, screenshot.EncodeToPNG());

        Destroy(screenshot);

        new NativeShare().AddFile(filePath)
            .SetSubject("Look at my highscore").SetText("Try to beat my highscore")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}
