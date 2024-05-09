#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Audio
{
    public class AudioWindowEditor : EditorWindow
    {
        public static void CreateSoundData()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<SoundData>("/Audio/SoundData", "sound_data");
        }

        public static void CreateSoundComponentPool()
        {
            CreateAsset.CreateScriptableAssets<SoundComponentPool>("/Audio", "sound_component_pool");
        }

        #region Event Music

        public static void CreatePlayMusicEvent()
        {
            CreateAsset.CreateScriptableAssets<PlayMusicEvent>("/Audio/Music_Event", "play_music_event");
        }

        public static void CreatePauseMusicEvent()
        {
            CreateAsset.CreateScriptableAssets<PauseMusicEvent>("/Audio/Music_Event", "pause_music_event");
        }

        public static void CreateResumeMusicEvent()
        {
            CreateAsset.CreateScriptableAssets<ResumeMusicEvent>("/Audio/Music_Event",
                "resume_music_event");
        }

        public static void CreateStopMusicEvent()
        {
            CreateAsset.CreateScriptableAssets<StopMusicEvent>("/Audio/Music_Event", "stop_music_event");
        }

        #endregion

        #region Event Sfx

        public static void CreatePlaySfxEvent()
        {
            CreateAsset.CreateScriptableAssets<PlaySfxEvent>("/Audio/Sfx_Event", "play_sfx_event");
        }

        public static void CreatePauseSfxEvent()
        {
            CreateAsset.CreateScriptableAssets<PauseSfxEvent>("/Audio/Sfx_Event", "pause_sfx_event");
        }

        public static void CreateFinishSfxEvent()
        {
            CreateAsset.CreateScriptableAssets<FinishSfxEvent>("/Audio/Sfx_Event", "finish_sfx_event");
        }

        public static void CreateResumeSfxEvent()
        {
            CreateAsset.CreateScriptableAssets<ResumeSfxEvent>("/Audio/Sfx_Event", "resume_sfx_event");
        }

        public static void CreateStopSfxEvent()
        {
            CreateAsset.CreateScriptableAssets<StopSfxEvent>("/Audio/Sfx_Event", "stop_sfx_event");
        }

        public static void CreateStopAllSfxEvent()
        {
            CreateAsset.CreateScriptableAssets<StopAllSfxEvent>("/Audio/Sfx_Event", "stop_all_sfx_event");
        }

        #endregion

        #region Volume Change

        public static void CreateMusicVolume()
        {
            CreateAsset.CreateScriptableAssets<MusicVolumeChange>("/Audio/Volume_Change", "music_volume");
        }

        public static void CreateSfxVolume()
        {
            CreateAsset.CreateScriptableAssets<SfxVolumeChange>("/Audio/Volume_Change", "sfx_volume");
        }

        #endregion
    }
}
#endif