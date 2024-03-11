using UnityEngine;

namespace ProefExamen.Framework.Audio
{
    /// <summary>
    /// Class responsible for holding audio clips for a specific collection.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioClipCollection", menuName = "ScriptableObjects/AudioCollectionContainer")]
    public class AudioCollectionContainer : ScriptableObject
    {
        /// <summary>
        /// Array which holds all the clips for it's specified audio collection.
        /// </summary>
        public AudioClip[] clips;

        /// <summary>
        /// Audio collection type.
        /// </summary>
        public AudioCollectionTypes collectionType;
    }
}