using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/Int Result",
        fileName = "vector3_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventIntResult : BaseEvent<Vector3, int>
    {
    }
}