using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/Vector3 Result",
        fileName = "vector3_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventVector3Result : BaseEvent<Vector3, Vector3>
    {
    }
}