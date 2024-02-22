#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Audio
{
    public class AudioWindowEditor : EditorWindow
    {
        public static void CreateEventAudioHandle()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventAudioHandle>("/Audio", "event_handle_audio");
        }

        public static void CreateSoundData()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<SoundData>("/Audio/SoundData", "sound_data");
        }

        public static void CreateSoundComponentPool()
        {
            CreateAsset.CreateScriptableAssets<SoundComponentPool>("/Audio", "sound_component_pool");
        }
    }
}
#endif