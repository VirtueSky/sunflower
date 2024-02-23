using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event/Object Event", fileName = "object_event")]
    [EditorIcon("scriptable_event")]
    public class ObjectEvent : BaseEvent<Object>
    {
    }
}