using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Sfx Event/Play Sfx Event", fileName = "play_sfx_event")]
    [EditorIcon("scriptable_audio")]
    public class PlaySfxEvent : BaseEvent<SoundData, SoundCache>
    {
    }
}