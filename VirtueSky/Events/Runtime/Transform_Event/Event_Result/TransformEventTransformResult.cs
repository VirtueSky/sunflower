using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/Transform Result",
        fileName = "transform_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventTransformResult : BaseEvent<Transform, Transform>
    {
    }
}