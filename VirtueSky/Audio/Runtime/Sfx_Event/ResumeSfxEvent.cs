using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower/Audio/Sfx Event/Resume Sfx Event", fileName = "resume_sfx_event")]
    [EditorIcon("scriptable_audio")]
    public class ResumeSfxEvent : BaseEvent<SoundCache>
    {
    }
}