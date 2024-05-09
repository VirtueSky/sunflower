using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Music Event/Play Music Event", fileName = "play_music_event")]
    [EditorIcon("scriptable_audio")]
    public class PlayMusicEvent : BaseEvent<SoundData>
    {
    }
}