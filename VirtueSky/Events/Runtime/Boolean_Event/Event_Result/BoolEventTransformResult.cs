using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/Transform Result",
        fileName = "bool_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventTransformResult : BaseEvent<bool, Transform>
    {
    }
}