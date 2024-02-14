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
            Texture2D tex = new(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            var old_rt = RenderTexture.active;
            RenderTexture.active = renderTexture;

            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            RenderTexture.active = old_rt;
            return tex;
        }
    }

}
