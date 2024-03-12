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
    public class AudioSystem : AbstractSingleton<AudioSystem>
    {
        [Header("Sounds"), SerializeField]
        private AudioCollection[] _audioCollections;

        private int _soundLenghtCounter = 0;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                PlayRandomSoundEffect();
            }
        }

        public void PlayRandomSoundEffect()
        {
            Array availableAudioTypes = Enum.GetValues(typeof(AudioCollectionTypes));   
            int randIndex = Random.Range(0, availableAudioTypes.Length);

            PlayRandomSoundEffectOfType((AudioCollectionTypes)availableAudioTypes.GetValue(randIndex));
        }

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
    }
}
