using ProefExamen.Framework.AdSystem;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    /// <summary>
    /// A class responsible for exiting the ad when the button is pressed.
    /// </summary>
    public class AdExitButton : BasicButton
    {
        private protected override void OnButtonPressed() => RewardedAdSystem.Instance.ExitButtonPressed();
    }
}