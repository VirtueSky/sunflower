using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/String Event", fileName = "string_event")]
    [EditorIcon("scriptable_event")]
    public class StringEvent : BaseEvent<string>
    {
    }
}