using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace VirtueSky.Audio
{
    public class AudioManager : BaseMono
    {
        [Space] [SerializeField] private bool isDontDestroyOnLoad;

        [Space] [SerializeField] private SoundComponentPool soundComponentPool;

        [Space] [TitleColor("Music Listening", CustomColor.Aqua, CustomColor.Lime)] [SerializeField]
        private EventAudioHandle eventPlayMusic;

        [SerializeField] private EventNoParam eventStopMusic;
        [SerializeField] private EventNoParam eventPauseMusic;
        [SerializeField] private EventNoParam eventResumeMusic;

        [Space] [TitleColor("Sfx Listening", CustomColor.Orange, CustomColor.Bisque)] [SerializeField]
        private EventAudioHandle eventPlaySfx;

        [SerializeField] private EventAudioHandle eventStopSfx;
        [SerializeField] private EventAudioHandle eventPauseSfx;
        [SerializeField] private EventAudioHandle eventResumeSfx;
        [SerializeField] private EventAudioHandle eventFinishSfx;
        [SerializeField] private EventNoParam eventStopAllSfx;

        [Space] [TitleColor("AudioManager Settings", CustomColor.DeepSkyBlue, CustomColor.Salmon)] [SerializeField]
        private FloatVariable musicVolume;

        [SerializeField] FloatVariable sfxVolume;

        private SoundComponent music;
        private List<SoundData> listAudioDatas = new List<SoundData>();
        private List<SoundComponent> listSoundComponents = new List<SoundComponent>();

        private void Awake()
        {
            if (isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }

            sfxVolume.AddListener(OnSfxVolumeChanged);
            musicVolume.AddListener(OnMusicVolumeChanged);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            eventPlaySfx.AddListener(PlaySfx);
            eventStopSfx.AddListener(StopSfx);
            eventFinishSfx.AddListener(FinishSfx);
            eventResumeSfx.AddListener(ResumeSfx);
            eventPauseSfx.AddListener(PauseSfx);
            eventStopAllSfx.AddListener(StopAllSfx);

            eventPlayMusic.AddListener(PlayMusic);
            eventPauseMusic.AddListener(PauseMusic);
            eventResumeMusic.AddListener(ResumeMusic);
            eventStopMusic.AddListener(StopMusic);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            eventPlaySfx.RemoveListener(PlaySfx);
            eventStopSfx.RemoveListener(StopSfx);
            eventFinishSfx.RemoveListener(FinishSfx);
            eventResumeSfx.RemoveListener(ResumeSfx);
            eventPauseSfx.RemoveListener(PauseSfx);
            eventStopAllSfx.RemoveListener(StopAllSfx);

            eventPlayMusic.RemoveListener(PlayMusic);
            eventPauseMusic.RemoveListener(PauseMusic);
            eventResumeMusic.RemoveListener(ResumeMusic);
            eventStopMusic.RemoveListener(StopMusic);
        }

        void OnMusicVolumeChanged(float volume)
        {
            if (music != null)
            {
                music.Volume = volume;
            }
        }

        void OnSfxVolumeChanged(float volume)
        {
            foreach (var audio in listSoundComponents)
            {
                audio.Volume = volume;
            }
        }

        #region Sfx

        private void PlaySfx(SoundData soundData)
        {
            var sfxComponent = soundComponentPool.Spawn<SoundComponent>();
            sfxComponent.transform.SetParent(this.transform);
            sfxComponent.PlayAudioClip(soundData.GetAudioClip(), soundData.loop, soundData.volume * sfxVolume.Value);
            if (!soundData.loop) sfxComponent.OnCompleted += OnFinishPlayingAudio;
            listAudioDatas.Add(soundData);
            listSoundComponents.Add(sfxComponent);
        }

        private void StopSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null) return;
            StopAndCleanAudioComponent(soundComponent);
            if (listAudioDatas.Count > 0)
            {
                listSoundComponents.Remove(GetSoundComponent(soundData));
                listAudioDatas.Remove(soundData);
            }
        }

        private void PauseSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Pause();
        }

        private void ResumeSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null || soundComponent.IsPlaying) return;
            soundComponent.Resume();
        }

        private void FinishSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Finish();
            soundComponent.OnCompleted += OnFinishPlayingAudio;
        }

        private void StopAllSfx()
        {
            foreach (var soundComponent in listSoundComponents)
            {
                StopAndCleanAudioComponent(soundComponent);
            }

            listSoundComponents.Clear();
            listAudioDatas.Clear();
        }

        #endregion

        #region Music

        private void PlayMusic(SoundData soundData)
        {
            if (music == null || !music.IsPlaying)
            {
                music = soundComponentPool.Spawn<SoundComponent>();
                music.transform.SetParent(this.transform);
            }

            music.FadePlayMusic(soundData.GetAudioClip(), soundData.volume * musicVolume.Value,
                soundData.isMusicFadeVolume ? soundData.fadeOutDuration : 0,
                soundData.isMusicFadeVolume ? soundData.fadeInDuration : 0);
            music.OnCompleted += StopAudioMusic;
        }

        private void StopMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Stop();
                soundComponentPool.DeSpawn(music.gameObject);
            }
        }

        private void PauseMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Pause();
            }
        }

        private void ResumeMusic()
        {
            if (music != null && !music.IsPlaying)
            {
                music.Resume();
            }
        }

        #endregion


        void OnFinishPlayingAudio(SoundComponent soundComponent)
        {
            StopAndCleanAudioComponent(soundComponent);
        }

        void StopAndCleanAudioComponent(SoundComponent soundComponent)
        {
            if (!soundComponent.IsLooping)
            {
                soundComponent.OnCompleted -= OnFinishPlayingAudio;
            }

            soundComponent.Stop();
            soundComponentPool.DeSpawn(soundComponent.gameObject);
            // pool.DeSpawn(soundComponent.gameObject);
        }

        void StopAudioMusic(SoundComponent soundComponent)
        {
            soundComponent.OnCompleted -= StopAudioMusic;
            soundComponentPool.DeSpawn(soundComponent.gameObject);
        }

        SoundComponent GetSoundComponent(SoundData soundData)
        {
            int index = listAudioDatas.FindIndex(x => x = soundData);
            if (index < 0)
            {
                return null;
            }

            return listSoundComponents[index];
        }
    }
}