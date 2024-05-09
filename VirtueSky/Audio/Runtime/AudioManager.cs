using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif


namespace VirtueSky.Audio
{
    [EditorIcon("icon_sound_mixer")]
    public class AudioManager : BaseMono
    {
        [Space] [SerializeField] private bool isDontDestroyOnLoad;

        [Space] [SerializeField] private SoundComponentPool soundComponentPool;
        [SerializeField] private Transform audioHolder;

        [Space] [TitleColor("Music Listening", CustomColor.Aqua, CustomColor.Lime)] [SerializeField]
        private PlayMusicEvent eventPlayMusic;

        [SerializeField] private StopMusicEvent eventStopMusic;
        [SerializeField] private PauseMusicEvent eventPauseMusic;
        [SerializeField] private ResumeMusicEvent eventResumeMusic;

        [Space] [TitleColor("Sfx Listening", CustomColor.Orange, CustomColor.Bisque)] [SerializeField]
        private PlaySfxEvent eventPlaySfx;

        [SerializeField] private StopSfxEvent eventStopSfx;
        [SerializeField] private PauseSfxEvent eventPauseSfx;
        [SerializeField] private ResumeSfxEvent eventResumeSfx;
        [SerializeField] private FinishSfxEvent eventFinishSfx;
        [SerializeField] private StopAllSfxEvent eventStopAllSfx;

        [Space] [TitleColor("AudioManager Settings", CustomColor.DeepSkyBlue, CustomColor.Salmon)] [SerializeField]
        private MusicVolumeChange musicVolume;

        [SerializeField] SfxVolumeChange sfxVolume;

        private SoundComponent music;

        private Dictionary<SoundCache, SoundComponent> dictSfxCache =
            new Dictionary<SoundCache, SoundComponent>();

        private int key = 0;

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
            foreach (var cache in dictSfxCache)
            {
                cache.Value.Volume = volume;
            }
        }

        #region Sfx

        private SoundCache PlaySfx(SoundData soundData)
        {
            var sfxComponent = soundComponentPool.Spawn<SoundComponent>(audioHolder);
            sfxComponent.PlayAudioClip(soundData.GetAudioClip(), soundData.loop, soundData.volume * sfxVolume.Value);
            if (!soundData.loop) sfxComponent.OnCompleted += OnFinishPlayingAudio;
            SoundCache soundCache = GetSoundCache(soundData);
            dictSfxCache.Add(soundCache, sfxComponent);
            return soundCache;
        }

        private void StopSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null) return;
            StopAndCleanAudioComponent(soundComponent);
            if (dictSfxCache.ContainsKey(soundCache))
            {
                dictSfxCache.Remove(soundCache);
            }
        }

        private void PauseSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Pause();
        }

        private void ResumeSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || soundComponent.IsPlaying) return;
            soundComponent.Resume();
        }

        private void FinishSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Finish();
            soundComponent.OnCompleted += OnFinishPlayingAudio;
        }

        private void StopAllSfx()
        {
            foreach (var cache in dictSfxCache)
            {
                StopAndCleanAudioComponent(cache.Value);
            }

            dictSfxCache.Clear();
            key = 0;
        }

        #endregion

        #region Music

        private void PlayMusic(SoundData soundData)
        {
            if (music == null || !music.IsPlaying)
            {
                music = soundComponentPool.Spawn<SoundComponent>(audioHolder);
            }

            music.FadePlayMusic(soundData.GetAudioClip(), soundData.loop, soundData.volume * musicVolume.Value,
                soundData.isMusicFadeVolume, soundData.fadeOutDuration, soundData.fadeInDuration);
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
        }

        void StopAudioMusic(SoundComponent soundComponent)
        {
            soundComponent.OnCompleted -= StopAudioMusic;
            soundComponentPool.DeSpawn(soundComponent.gameObject);
        }

        SoundComponent GetSoundComponent(SoundCache soundCache)
        {
            if (!dictSfxCache.ContainsKey(soundCache)) return null;
            foreach (var cache in dictSfxCache)
            {
                if (cache.Key == soundCache)
                {
                    return cache.Value;
                }
            }

            return null;
        }


        SoundCache GetSoundCache(SoundData soundData)
        {
            key++;
            return new SoundCache(key, soundData);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            soundComponentPool =
                CreateAsset.CreateAndGetScriptableAsset<SoundComponentPool>("/Audio", "sound_component_pool", false);
            eventPlayMusic =
                CreateAsset.CreateAndGetScriptableAsset<PlayMusicEvent>("/Audio/Music_Event", "play_music_event",
                    false);
            eventPauseMusic =
                CreateAsset.CreateAndGetScriptableAsset<PauseMusicEvent>("/Audio/Music_Event", "pause_music_event",
                    false);
            eventResumeMusic = CreateAsset.CreateAndGetScriptableAsset<ResumeMusicEvent>("/Audio/Music_Event",
                "resume_music_event", false);
            eventStopMusic =
                CreateAsset.CreateAndGetScriptableAsset<StopMusicEvent>("/Audio/Music_Event", "stop_music_event",
                    false);
            eventPlaySfx =
                CreateAsset.CreateAndGetScriptableAsset<PlaySfxEvent>("/Audio/Sfx_Event", "play_sfx_event", false);
            eventPauseSfx =
                CreateAsset.CreateAndGetScriptableAsset<PauseSfxEvent>("/Audio/Sfx_Event", "pause_sfx_event", false);
            eventFinishSfx =
                CreateAsset.CreateAndGetScriptableAsset<FinishSfxEvent>("/Audio/Sfx_Event", "finish_sfx_event", false);
            eventResumeSfx =
                CreateAsset.CreateAndGetScriptableAsset<ResumeSfxEvent>("/Audio/Sfx_Event", "resume_sfx_event", false);
            eventStopSfx =
                CreateAsset.CreateAndGetScriptableAsset<StopSfxEvent>("/Audio/Sfx_Event", "stop_sfx_event", false);
            eventStopAllSfx =
                CreateAsset.CreateAndGetScriptableAsset<StopAllSfxEvent>("/Audio/Sfx_Event", "stop_all_sfx_event",
                    false);
            musicVolume =
                CreateAsset.CreateAndGetScriptableAsset<MusicVolumeChange>("/Audio/Volume_Change", "music_volume",
                    false);
            sfxVolume = CreateAsset.CreateAndGetScriptableAsset<SfxVolumeChange>("/Audio/Volume_Change", "sfx_volume",
                false);
        }
#endif
    }
}