#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Audio
{
    public class AudioWindowEditor : EditorWindow
    {
        [MenuItem("Sunflower/Audio/Event Audio Handle")]
        public static void CreateEventAudioHandle()
        {
            CreateAsset.CreateScriptableAssets<EventAudioHandle>("/Audio", "event_audio_handle");
        }

        [MenuItem("Sunflower/Audio/Sound Data")]
        public static void CreateSoundData()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<SoundData>("/Audio/SoundData");
        }
    }
}
#endif