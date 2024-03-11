using UnityEngine;

namespace ProefExamen.Framework.Audio
{
    /// <summary>
    /// Class responsible for managing general audio effect
    /// </summary>
    public class AudioSystem : MonoBehaviour
    {
        [Header("UI Sounds"), SerializeField]
        private AudioCollectionContainer _backAudioClips;

        [SerializeField]
        private AudioCollectionContainer _selectAudioClips;

        [SerializeField]
        private AudioCollectionContainer _confirmAudioClips;

        [SerializeField]
        private AudioCollectionContainer _deniedAudioClips;

    }
}
