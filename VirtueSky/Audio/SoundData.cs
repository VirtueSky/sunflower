using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Core;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Audio/Sound Data", fileName = "sound_data")]
    public class SoundData : BaseSO
    {
        [Space] public bool loop;
        [Range(0f, 1f)] public float volume = 1;

        [Header("Fade Volume - Only Music"), Tooltip("Only Music Background")]
        public bool isMusicFadeInVolume = false;

        [ShowIf(nameof(isMusicFadeInVolume), true)]
        public float fadeInDuration = .5f;

        [ShowIf(nameof(isMusicFadeInVolume), true)]
        public float fadeOutDuration = .5f;

        [Space] [SerializeField] private List<AudioClip> audioClips;


        public int NumberOfAudioClips => audioClips.Count;

        public AudioClip GetAudioClip()
        {
            if (audioClips.Count > 0)
            {
                return audioClips[Random.Range(0, audioClips.Count)];
            }

            return null;
        }
    }
}