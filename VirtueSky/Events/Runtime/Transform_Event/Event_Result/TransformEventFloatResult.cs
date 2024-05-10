using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/Float Result",
        fileName = "transform_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventFloatResult : BaseEvent<Transform, float>
    {
    }
}