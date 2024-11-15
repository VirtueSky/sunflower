using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [EditorIcon("icon_csharp")]
    public class SoundComponent : CacheComponent<AudioSource>
    {
        [ReadOnly, SerializeField] private int key;
        public event UnityAction<SoundComponent> OnCompleted;
        public event UnityAction<SoundComponent> OnPaused;
        public event UnityAction<SoundComponent> OnResumed;
        public event UnityAction<SoundComponent> OnStopped;

        public AudioClip GetClip => component.clip;
        public bool IsPlaying => component.isPlaying;
        public bool IsLooping => component.loop;

        public float Volume
        {
            get => component.volume;
            set => component.volume = value;
        }

        public int Key
        {
            get => key;
            set => key = value;
        }

        private void Awake()
        {
            component.playOnAwake = false;
        }

        internal void PlayAudioClip(AudioClip audioClip, bool isLooping, float volume)
        {
            if (audioClip == null)
            {
                Debug.LogError($"AudioClip is null");
                return;
            }

            component.clip = audioClip;
            component.loop = isLooping;
            component.volume = volume;
            component.time = 0;
            component.Play();
            if (!isLooping)
            {
                App.Delay(this, audioClip.length, OnCompletedInvoke);
            }
        }

        void FadeInVolumeMusic(AudioClip audioClip, bool isLooping, float endValue, float duration)
        {
            PlayAudioClip(audioClip, isLooping, 0);
            Tween.AudioVolume(component, endValue, duration);
        }

        void FadeOutVolumeMusic(float duration, Action fadeCompleted)
        {
            Tween.AudioVolume(component, 0, duration).OnComplete(fadeCompleted);
        }


        internal void Resume()
        {
            OnResumed?.Invoke(this);
            component.UnPause();
        }

        internal void Pause()
        {
            OnPaused?.Invoke(this);
            component.Pause();
        }

        internal void Stop()
        {
            OnStopped?.Invoke(this);
            component.Stop();
        }

        internal void Finish()
        {
            if (!component.loop) return;
            component.loop = false;
            float remainingTime = component.clip.length - component.time;
            App.Delay(this, remainingTime, OnCompletedInvoke);
        }

        internal void FadePlayMusic(AudioClip audioClip, bool isLooping, float volume, bool isMusicFadeVolume,
            float durationOut,
            float durationIn)
        {
            if (isMusicFadeVolume && volume != 0)
            {
                if (component.isPlaying)
                {
                    FadeOutVolumeMusic(durationOut,
                        () => { FadeInVolumeMusic(audioClip, isLooping, volume, durationIn); });
                }
                else
                {
                    FadeInVolumeMusic(audioClip, isLooping, volume, durationIn);
                }
            }
            else
            {
                PlayAudioClip(audioClip, isLooping, volume);
            }
        }

        private void OnCompletedInvoke()
        {
            OnCompleted?.Invoke(this);
        }
    }
}