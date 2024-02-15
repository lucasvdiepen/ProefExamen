using UnityEngine;

namespace ProefExamen.Framework.Utils
{
    /// <summary>
    /// A class containing extension methods for the <see cref="RenderTexture"/> class.
    /// </summary>
    public static class RenderTextureExtensions
    {
        /// <summary>
        /// Converts a <see cref="RenderTexture"/> to a <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="renderTexture">The <see cref="RenderTexture"/> to convert.</param></param>
        /// <returns>The converted <see cref="Texture2D"/>.</returns>
        public static Texture2D ToTexture2D(this RenderTexture renderTexture)
        {
            Texture2D texture = new(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            RenderTexture oldRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;

            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            RenderTexture.active = oldRenderTexture;
            return texture;
        }
    }
}
