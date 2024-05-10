using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/Transform Result",
        fileName = "float_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventTransformResult : BaseEvent<float, Transform>
    {
    }
}