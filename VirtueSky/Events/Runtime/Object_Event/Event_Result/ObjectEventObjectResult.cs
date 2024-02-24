using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/Object Result",
        fileName = "object_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventObjectResult : BaseEvent<object, object>
    {
    }
}