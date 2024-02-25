using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/Bool Result",
        fileName = "vector3_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventBoolResult : BaseEvent<Vector3, bool>
    {
    }
}