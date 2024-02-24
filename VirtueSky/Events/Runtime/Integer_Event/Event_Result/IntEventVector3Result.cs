using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/Vector3 Result",
        fileName = "int_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventVector3Result : BaseEvent<int, Vector3>
    {
    }
}