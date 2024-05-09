using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Sfx Event/Pause Sfx Event", fileName = "pause_sfx_event")]
    [EditorIcon("scriptable_audio")]
    public class PauseSfxEvent : BaseEvent<SoundCache>
    {
    }
}