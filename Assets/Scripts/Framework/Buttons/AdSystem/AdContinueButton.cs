using ProefExamen.Framework.AdSystem;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    /// <summary>
    /// A class responsible for continuing out of the ad when the button is pressed.
    /// </summary>
    public class AdContinueButton : BasicButton
    {
        private protected override void OnButtonPressed() => RewardedAdSystem.Instance.ContinueButtonPressed();
    }
}