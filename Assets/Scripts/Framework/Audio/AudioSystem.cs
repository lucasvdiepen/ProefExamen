using UnityEngine;

namespace ProefExamen.Framework.Audio
{
    /// <summary>
    /// Class responsible for managing general audio effect
    /// </summary>
    public class AudioSystem : MonoBehaviour
    {
        [Header("UI Sounds"), SerializeField]
        private AudioClip[] _backAudioClips;

        [SerializeField]
        private AudioClip[] _selectAudioClips;

        [SerializeField]
        private AudioClip[] _clickAudioClips;

        [SerializeField]
        private AudioClip[] _deniedAudioClips;
    }
}
