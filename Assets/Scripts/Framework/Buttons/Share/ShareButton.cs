using ProefExamen.Framework.Sharing;

namespace ProefExamen.Framework.Buttons.Share
{
    /// <summary>
    /// A class responsible for sharing the highscore of the player when the button is pressed.
    /// </summary>
    public class ShareButton : BasicButton
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnButtonPressed() => ShareHighscore.Instance.Share();
    }
}