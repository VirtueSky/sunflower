using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/Float Result",
        fileName = "int_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventFloatResult : BaseEvent<int, float>
    {
    }
}