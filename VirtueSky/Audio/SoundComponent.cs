using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VirtueSky.Core;

namespace VirtueSky.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundComponent : BaseMono
    {
        [SerializeField] private AudioSource audioSource;

        public event UnityAction<SoundComponent> OnCompleted;
        public event UnityAction<SoundComponent> OnPaused;
        public event UnityAction<SoundComponent> OnResumed;
        public event UnityAction<SoundComponent> OnStopped;

        public AudioClip GetClip => audioSource.clip;
        public bool IsPlaying => audioSource.isPlaying;
        public bool IsLooping => audioSource.loop;

        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }

        private void Awake()
        {
            audioSource.playOnAwake = false;
        }

        internal void PlayAudioClip(AudioClip audioClip, bool isLooping, float volume)
        {
            audioSource.clip = audioClip;
            audioSource.loop = isLooping;
            audioSource.volume = volume;
            audioSource.time = 0;
            audioSource.Play();
            if (!isLooping)
            {
                StartCoroutine(Delay(audioClip.length, OnCompletedInvoke));
            }
        }

        IEnumerator Delay(float delayTime, Action action)
        {
            yield return new WaitForSeconds(delayTime);
            action?.Invoke();
        }

        internal void Resume()
        {
            OnResumed?.Invoke(this);
            audioSource.UnPause();
        }

        internal void Pause()
        {
            OnPaused?.Invoke(this);
            audioSource.Pause();
        }

        internal void Stop()
        {
            OnStopped?.Invoke(this);
            audioSource.Stop();
        }

        internal void Finish()
        {
            if (!audioSource.loop) return;
            audioSource.loop = false;
            float remainingTime = audioSource.clip.length - audioSource.time;
            StartCoroutine(Delay(remainingTime, OnCompletedInvoke));
        }

        private void OnCompletedInvoke()
        {
            OnCompleted?.Invoke(this);
        }


#if UNITY_EDITOR
        private void Reset()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
#endif
    }
}