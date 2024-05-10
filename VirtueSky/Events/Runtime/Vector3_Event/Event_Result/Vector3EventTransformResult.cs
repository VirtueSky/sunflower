using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/Transform Result",
        fileName = "vector3_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventTransformResult : BaseEvent<Vector3, Transform>
    {
    }
}