using ProefExamen.Framework.Utils;
using UnityEngine;

namespace ProefExamen.Framework.Audio
{
    /// <summary>
    /// Class responsible for managing general audio effect
    /// </summary>
    [RequireComponent(typeof(AudioSource))] 
    public class AudioSystem : AbstractSingleton<AudioSystem>
    {
        [Header("UI Sounds"), SerializeField]
        private AudioCollectionContainer[] _audioCollectionClips;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomSoundEffect()
        {
            //_audioSource.clip = _audioCollectionClips[Random.Range(0, _audioCollectionClips.Length)].clips[Random.Range(0, ;
        }
    }
}
