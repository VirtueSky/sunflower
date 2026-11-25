using System;
using VirtueSky.Tweening;
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
        private DelayHandle completeDelay;
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
            if (isLooping) CancelCompleteDelay();
            else ScheduleComplete(audioClip.length);
        }

        void FadeInVolumeMusic(AudioClip audioClip, bool isLooping, float endValue, float duration)
        {
            PlayAudioClip(audioClip, isLooping, 0);
            Tween.Create(0, endValue, duration).Bind(value => component.volume = value);
        }

        void FadeOutVolumeMusic(float duration, Action fadeCompleted)
        {
            float startValue = component.volume;
            Tween.Create(startValue, 0, duration).WithOnComplete(fadeCompleted).Bind(value => component.volume = value);
        }


        internal void Resume()
        {
            OnResumed?.Invoke(this);
            component.UnPause();
            App.ResumeDelay(completeDelay);
        }

        internal void Pause()
        {
            OnPaused?.Invoke(this);
            component.Pause();
            App.PauseDelay(completeDelay);
        }

        internal void Stop()
        {
            OnStopped?.Invoke(this);
            component.Stop();
            CancelCompleteDelay();
        }

        internal void Finish()
        {
            if (!component.loop) return;
            component.loop = false;
            ScheduleComplete(component.clip.length - component.time);
        }

        /// <summary>
        /// Called by the pool right before this object is returned (DeSpawn).
        /// The DelayHandle is only auto-cancelled when the owner is destroyed, not when it is pooled,
        /// so it must be cancelled here, otherwise it would fire on the next reuse of this component.
        /// </summary>
        public override void CleanUp()
        {
            base.CleanUp();
            CancelCompleteDelay();
            OnCompleted = null;
            OnPaused = null;
            OnResumed = null;
            OnStopped = null;
            key = 0;
        }

        /// <summary>
        /// Schedule a completion check after <paramref name="clipSeconds"/> of audio (clip time, before pitch).
        /// Uses real time so Time.timeScale (game pause / slow-mo) does not affect it, since AudioSource ignores timeScale.
        /// </summary>
        void ScheduleComplete(float clipSeconds)
        {
            CancelCompleteDelay();
            float pitch = Mathf.Max(0.01f, Mathf.Abs(component.pitch));
            completeDelay = App.Delay(this, Mathf.Max(0f, clipSeconds) / pitch, CheckCompleted, useRealTime: true);
        }

        void CancelCompleteDelay()
        {
            App.CancelDelay(completeDelay);
            completeDelay = null;
        }

        /// <summary>
        /// The delay is only a "check-in" point; the AudioSource state decides whether the clip really ended.
        /// Covers cases where real time advanced but audio did not: AudioListener.pause (ads),
        /// app backgrounded on mobile, audio thread hiccups.
        /// </summary>
        void CheckCompleted()
        {
            completeDelay = null;

            // Global audio pause: isPlaying is false but the clip has not ended. Poll until unpaused.
            if (AudioListener.pause)
            {
                completeDelay = App.Delay(this, 0.25f, CheckCompleted, useRealTime: true);
                return;
            }

            // Still playing (audio fell behind real time): wait for the actual remainder.
            if (component.isPlaying && component.clip != null)
            {
                ScheduleComplete(component.clip.length - component.time);
                return;
            }

            OnCompletedInvoke();
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