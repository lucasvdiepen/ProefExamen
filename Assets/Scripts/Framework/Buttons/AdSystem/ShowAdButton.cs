using ProefExamen.Framework.AdSystem;
using UnityEngine;

namespace ProefExamen.Framework.Buttons.AdSystem
{
    public class ShowAdButton : BasicButton
    {
        private protected override void OnButtonPressed()
        {
            RewardedAdSystem.Instance.ShowAd((success) =>
            {
                Debug.Log("Ad finished state: " + success);
            });
        }
    }
}
