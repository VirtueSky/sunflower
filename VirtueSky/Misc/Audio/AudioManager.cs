using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.ObjectPooling;
using VirtueSky.Variables;

namespace VirtueSky.Misc
{
    public class AudioManager : BaseMono
    {
        [SerializeField] private Pools pool;
        [SerializeField] private AudioComponent audioComponentPrefab;

        [Space] [Header("AudioManager Settings")] [SerializeField]
        private FloatVariable musicVolume;

        [SerializeField] FloatVariable sfxVolume;

        private AudioComponent music;
        private List<AudioData> listAudioDatas = new List<AudioData>();
        private List<AudioComponent> listAudioComponents = new List<AudioComponent>();

        private void Awake()
        {
            pool.Initialize();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        private void PlaySfx(AudioData audioData)
        {
            var audio = pool.Spawn(audioComponentPrefab);
            audio.PlayAudioClip(audioData.GetAudioClip(), audioData.loop, audioData.volume);
            if (!audioData.loop) audio.OnCompleted += OnFinishPlayingAudio;
            listAudioDatas.Add(audioData);
            listAudioComponents.Add(audio);
        }

        private void StopSfx(AudioData audioData)
        {
            StopAndCleanAudioComponent(GetAudioComponent(audioData));
            listAudioDatas.Remove(audioData);
        }

        private void PauseSfx(AudioData audioData)
        {
            GetAudioComponent(audioData).Pause();
        }

        private void ResumeSfx(AudioData audioData)
        {
            GetAudioComponent(audioData).Resume();
        }

        private void FinishSfx(AudioData audioData)
        {
            var audioComponent = GetAudioComponent(audioData);
            audioComponent.Finish();
            audioComponent.OnCompleted += OnFinishPlayingAudio;
        }

        private void StopAllSfx()
        {
            foreach (var audioComponent in listAudioComponents)
            {
                audioComponent.Stop();
            }
        }

        private void PlayMusic(AudioData audioData)
        {
            if (music != null && music.IsLooping)
            {
            }
        }

        void OnFinishPlayingAudio(AudioComponent audioComponent)
        {
            StopAndCleanAudioComponent(audioComponent);
        }

        void StopAndCleanAudioComponent(AudioComponent audioComponent)
        {
            if (!audioComponent.IsLooping)
            {
                audioComponent.OnCompleted -= OnFinishPlayingAudio;
            }

            audioComponent.Stop();
            pool.Despawn(audioComponent.gameObject);
        }

        void StopAudioMusic(AudioComponent audioComponent)
        {
            audioComponent.OnCompleted -= StopAudioMusic;
            pool.Despawn(audioComponent.gameObject);
        }

        AudioComponent GetAudioComponent(AudioData audioData)
        {
            int index = listAudioDatas.FindIndex(x => x = audioData);
            if (index < 0)
            {
                return null;
            }

            return listAudioComponents[index];
        }
    }
}