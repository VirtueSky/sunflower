using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/Transform Result",
        fileName = "int_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventTransformResult : BaseEvent<int, Transform>
    {
    }
}