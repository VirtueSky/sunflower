using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/Float Result",
        fileName = "bool_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventFloatResult : BaseEvent<bool, float>
    {
    }
}