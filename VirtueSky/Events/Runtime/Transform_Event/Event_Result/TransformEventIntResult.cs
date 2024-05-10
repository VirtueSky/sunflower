using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/Int Result",
        fileName = "transform_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventIntResult : BaseEvent<Transform, int>
    {
    }
}