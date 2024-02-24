using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/Int Result",
        fileName = "object_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventIntResult : BaseEvent<object, int>
    {
    }
}