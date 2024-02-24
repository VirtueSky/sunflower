using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/Vector3 Result",
        fileName = "bool_event_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventVector3Result : BaseEvent<bool, Vector3>
    {
    }
}