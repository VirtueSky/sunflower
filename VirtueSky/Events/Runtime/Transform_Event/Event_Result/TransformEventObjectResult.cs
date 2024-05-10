using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/Object Result",
        fileName = "transform_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventObjectResult : BaseEvent<Transform, object>
    {
    }
}