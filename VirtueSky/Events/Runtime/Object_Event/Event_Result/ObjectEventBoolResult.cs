using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/Bool Result",
        fileName = "object_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventBoolResult : BaseEvent<object, bool>
    {
    }
}