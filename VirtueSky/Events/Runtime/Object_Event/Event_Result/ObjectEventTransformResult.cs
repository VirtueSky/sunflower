using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/Transform Result",
        fileName = "object_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventTransformResult : BaseEvent<object, Transform>
    {
    }
}