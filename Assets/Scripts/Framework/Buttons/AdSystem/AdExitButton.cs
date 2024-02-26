using ProefExamen.Framework.AdSystem;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    public class AdExitButton : BasicButton
    {
        private protected override void OnButtonPressed() => RewardedAdSystem.Instance.ExitButtonPressed();
    }
}