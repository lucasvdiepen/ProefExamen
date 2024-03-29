using UnityEngine;
using UnityEngine.EventSystems;

using ProefExamen.Framework.Buttons;

namespace ProefExamen.Framework.Audio
{
    /// <summary>
    /// Class responsible for playing a random sound effect on button pointer down.
    /// </summary>
    public class SoundEffectButton : BasicButtonDownPointer
    {
        [SerializeField]
        private AudioCollectionTypes soundTypeOnPress;

        [SerializeField]
        private float volume = 1;

        private readonly AudioCollectionTypes soundTypeOnDenied = AudioCollectionTypes.Denied;

        private protected override void OnButtonPointerDown(PointerEventData eventData)
        {
            AudioCollectionTypes audioType = !Button.interactable ? soundTypeOnDenied : soundTypeOnPress;
            AudioSystem.Instance.PlayRandomSoundEffectOfType(audioType, volume);
        }
    }
}
