using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/String Result",
        fileName = "transform_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventStringResult : BaseEvent<Transform, string>
    {
    }
}