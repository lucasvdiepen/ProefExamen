using ProefExamen.Framework.AdSystem;
using ProefExamen.Framework.StateMachine.States;
using UnityEngine;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    /// <summary>
    /// A class responsible for showing an ad when the button is pressed.
    /// </summary>
    public class ShowAdButton : BasicButton
    {
        private protected override void OnButtonPressed()
        {
            RewardedAdSystem.Instance.ShowAd((success) =>
            {
                if (!success)
                    return;

                Framework.StateMachine.StateMachine.Instance.GoToState<PauseState>();
            });
        }
    }
}
