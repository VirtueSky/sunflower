using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Audio/Sound Data", fileName = "sound_data")]
    public class SoundData : BaseSO
    {
        [Space] public bool loop;
        [Range(0f, 1f)] public float volume = 1;
        [SerializeField] private List<AudioClip> audioClips;


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