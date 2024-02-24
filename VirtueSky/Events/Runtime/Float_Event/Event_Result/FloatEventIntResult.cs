using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/Int Result",
        fileName = "float_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventIntResult : BaseEvent<float, int>
    {
    }
}