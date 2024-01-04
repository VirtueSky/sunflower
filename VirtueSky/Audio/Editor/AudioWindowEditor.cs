#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Audio
{
    public class AudioWindowEditor : EditorWindow
    {
        [MenuItem("Sunflower/Audio/Event Audio Handle")]
        public static void CreateEventAudioHandle()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventAudioHandle>("/Audio", "event_handle_audio");
        }

        [MenuItem("Sunflower/Audio/Sound Data")]
        public static void CreateSoundData()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<SoundData>("/Audio/SoundData", "sound_data");
        }
    }
}
#endif