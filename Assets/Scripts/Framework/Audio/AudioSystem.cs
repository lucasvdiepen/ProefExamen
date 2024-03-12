using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine;
using System;

using ProefExamen.Framework.Utils;

namespace ProefExamen.Framework.Audio
{
    /// <summary>
    /// Class responsible for managing general audio effect
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : AbstractSingleton<AudioSystem>
    {
        [Header("Sounds"), SerializeField]
        private AudioCollection[] _audioCollections;

        [SerializeField]
        private float _songFadeDuration = .5f;

        [SerializeField]
        private AudioClip _testSong;

        private int _soundLenghtCounter = 0;
        private int _songLenghtCounter = 0;
        private int _crossFadeSongLenghtCounter = 0;

        private bool _isCrossFading;

        public AudioSource CurrentActiveSource { get; private set; }

        private void Awake() => CurrentActiveSource = GetComponent<AudioSource>();

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayRandomSoundEffect();
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                PlaySong(_testSong);
            }
        }
        
        /// <summary>
        /// Method for playing a random sound effect.
        /// </summary>
        public void PlayRandomSoundEffect()
        {
            Array availableAudioTypes = Enum.GetValues(typeof(AudioCollectionTypes));   
            int randIndex = Random.Range(0, availableAudioTypes.Length);

            PlayRandomSoundEffectOfType((AudioCollectionTypes)availableAudioTypes.GetValue(randIndex));
        }

        /// <summary>
        /// Method for playing a random sound effect based on a specified type.
        /// </summary>
        /// <param name="audioCollectionType">Audio collection type.</param>
        /// <param name="volume">Optional volume param.</param>
        public void PlayRandomSoundEffectOfType(AudioCollectionTypes audioCollectionType, float volume = .75f)
        {
            for (int i = 0; i < _audioCollections.Length; i++)
            {
                if (_audioCollections[i].audioCollectionType != audioCollectionType)
                    continue;

                int randIndex = Random.Range(0, _audioCollections[i].audioClipList.Count);
                PlaySound(_audioCollections[i].audioClipList[randIndex], volume);
            }
        }

        private void PlaySound(AudioClip clip, float volume = .75f, bool fadeIn = true)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = clip;
            source.volume = volume;

            source.Play();

            float audioClipLenght = clip.length;
            DOTween.To(() => _soundLenghtCounter, x => _soundLenghtCounter = x, 1, audioClipLenght)
                .OnComplete(() => Destroy(source));
        }

        /// <summary>
        /// Method for playing a song. Applies automatic crossfading if needed.
        /// </summary>
        /// <param name="clip">Song to be played.</param>
        public void PlaySong(AudioClip clip)
        {
            if (_isCrossFading)
                return;

            if (CurrentActiveSource.clip != null)
            {
                _isCrossFading = true;
                AudioSource crossFadeSource = gameObject.AddComponent<AudioSource>();

                crossFadeSource.clip = clip;
                crossFadeSource.volume = 0;

                crossFadeSource.Play();
                crossFadeSource.DOFade(1, _songFadeDuration);

                CurrentActiveSource.DOFade(0, _songFadeDuration).OnComplete(() =>
                {
                    Destroy(CurrentActiveSource);
                    CurrentActiveSource = crossFadeSource;

                    _isCrossFading = false;
                    float audioClipLenght = clip.length;

                    DOTween.To(() => _crossFadeSongLenghtCounter, x => _crossFadeSongLenghtCounter = x, 1, audioClipLenght)
                        .OnComplete(() => CurrentActiveSource.clip = null);
                });
            }
            else
            {
                CurrentActiveSource.volume = 0;
                CurrentActiveSource.clip = clip;

                CurrentActiveSource.Play();
                CurrentActiveSource.DOFade(1, _songFadeDuration);

                float audioClipLenght = clip.length;
                DOTween.To(() => _songLenghtCounter, x => _songLenghtCounter = x, 1, audioClipLenght)
                    .OnComplete(() => CurrentActiveSource.clip = null);
            }
        }
    }
}
