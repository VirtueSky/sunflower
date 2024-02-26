using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/String Result",
        fileName = "vector3_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventStringResult : BaseEvent<Vector3, string>
    {
    }
}