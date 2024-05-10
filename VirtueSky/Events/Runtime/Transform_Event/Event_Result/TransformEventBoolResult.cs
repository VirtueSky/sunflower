using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/Bool Result",
        fileName = "transform_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventBoolResult : BaseEvent<Transform, bool>
    {
    }
}