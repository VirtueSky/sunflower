using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Misc;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Sound Data", fileName = "sound_data")]
    [EditorIcon("scriptable_audioclip")]
    public class SoundData : BaseSO
    {
        public enum GetType
        {
            Random,
            Sequence
        }

        [Space] public bool loop = false;
        [Range(0f, 1f)] public float volume = 1;

        [Header("Fade Volume - Only Music"), Tooltip("Only Music Background")]
        public bool isMusicFadeVolume = false;

        [ShowIf(nameof(isMusicFadeVolume), true)]
        public float fadeInDuration = .5f;

        [ShowIf(nameof(isMusicFadeVolume), true)]
        public float fadeOutDuration = .5f;

        [Space] public GetType getType = GetType.Random;
        [SerializeField] private List<AudioClip> audioClips;

        private int sequenceIndex = 0;
        public int NumberOfAudioClips => audioClips.Count;
        public List<AudioClip> AudioClips() => audioClips;

        public AudioClip GetAudioClip()
        {
            if (audioClips.Count > 0)
            {
                switch (getType)
                {
                    case GetType.Random:
                        return audioClips[Random.Range(0, audioClips.Count)];
                    case GetType.Sequence:
                        var clip = audioClips[sequenceIndex];
                        if (sequenceIndex < audioClips.Count - 1)
                        {
                            sequenceIndex++;
                        }
                        else
                        {
                            sequenceIndex = 0;
                        }

                        return clip;
                }
            }

            return null;
        }

        public void AddAudioClip(AudioClip audioClip)
        {
            audioClips.Add(audioClip);
        }

        public void AddAudioClips(List<AudioClip> clips)
        {
            audioClips.Adds(clips);
        }

        public void AddAudioClips(AudioClip[] clips)
        {
            audioClips.Adds(clips);
        }

        public void ClearAudioClips()
        {
            if (audioClips.IsNullOrEmpty()) return;
            audioClips.Clear();
        }
    }
}