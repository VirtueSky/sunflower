using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/Vector3 Result",
        fileName = "transform_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventVector3Result : BaseEvent<Transform, Vector3>
    {
    }
}