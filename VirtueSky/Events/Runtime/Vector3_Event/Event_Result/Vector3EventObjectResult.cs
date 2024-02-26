using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/Object Result",
        fileName = "vector3_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventObjectResult : BaseEvent<Vector3, object>
    {
    }
}