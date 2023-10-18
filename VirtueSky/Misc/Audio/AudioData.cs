using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Misc
{
    [CreateAssetMenu(menuName = "Audio/Audio Data", fileName = "audio_data")]
    public class AudioData : BaseSO
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