using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Volume Change Variable/Sfx Volume Change",
        fileName = "sfx_volume")]
    [EditorIcon("scriptable_variable")]
    public class SfxVolumeChange : FloatVariable
    {
    }
}