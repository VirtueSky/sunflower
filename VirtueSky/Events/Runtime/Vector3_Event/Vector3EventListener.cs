using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [EditorIcon("scriptable_event_listener")]
    public class Vector3EventListener : BaseEventListener<Vector3, Vector3Event, Vector3EventResponse>
    {
    }
}