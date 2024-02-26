using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Vector3 Event/Float Result",
        fileName = "vector3_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class Vector3EventFloatResult : BaseEvent<Vector3, float>
    {
    }
}