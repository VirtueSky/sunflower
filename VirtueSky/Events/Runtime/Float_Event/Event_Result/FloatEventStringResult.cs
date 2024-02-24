using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/String Result",
        fileName = "float_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventStringResult : BaseEvent<float, string>
    {
    }
}