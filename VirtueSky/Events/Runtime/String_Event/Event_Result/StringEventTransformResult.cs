using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/Transform Result",
        fileName = "string_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventTransformResult : BaseEvent<string, Transform>
    {
    }
}