using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/Vector3 Result",
        fileName = "object_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventVector3Result : BaseEvent<object, Vector3>
    {
    }
}