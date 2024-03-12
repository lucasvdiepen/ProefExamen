using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine;
using System;

using ProefExamen.Framework.Utils;
using static Unity.VisualScripting.Member;

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
                PrepareSong(_testSong);
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

        public void PrepareSong(AudioClip clip)
        {

            if (CurrentActiveSource.clip != null)
            {
                AudioSource crossFadeSource = gameObject.AddComponent<AudioSource>();

                crossFadeSource.clip = clip;
                crossFadeSource.volume = 0;

                crossFadeSource.Play();
                crossFadeSource.DOFade(1, _songFadeDuration);

                CurrentActiveSource.DOFade(0, _songFadeDuration).OnComplete(() =>
                {
                    Destroy(CurrentActiveSource);
                    CurrentActiveSource = crossFadeSource;
                });
            }
            else
            {
                CurrentActiveSource.volume = 0;
                PlaySong(clip);
            }
        }

        private void PlaySong(AudioClip clip, AudioSource overrideSource = null)
        {
            AudioSource chosenSource = overrideSource == null ? CurrentActiveSource : overrideSource;
            chosenSource.clip = clip;

            chosenSource.Play();
            chosenSource.DOFade(1, _songFadeDuration);

            float audioClipLenght = clip.length;
            DOTween.To(() => _songLenghtCounter, x => _songLenghtCounter = x, 1, audioClipLenght)
                .OnComplete(() =>
                {
                    chosenSource.clip = null;
                });
        }
    }
}
