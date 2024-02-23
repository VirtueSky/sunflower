using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [EditorIcon("scriptable_event_listener")]
    public class ObjectEventListener : BaseEventListener<Object, ObjectEvent, ObjectEventResponse>
    {
    }
}