using ProefExamen.Framework.AdSystem;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    public class AdContinueButton : BasicButton
    {
        private protected override void OnButtonPressed() => RewardedAdSystem.Instance.ContinueButtonPressed();
    }
}