using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/Float Result",
        fileName = "float_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventFloatResult : BaseEvent<float, float>
    {
    }
}