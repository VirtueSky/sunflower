using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Volume Change Variable/Music Volume Change",
        fileName = "music_volume")]
    [EditorIcon("scriptable_variable")]
    public class MusicVolumeChange : FloatVariable
    {
    }
}