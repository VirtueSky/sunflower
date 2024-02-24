using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/Bool Result",
        fileName = "float_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventBoolResult : BaseEvent<float, bool>
    {
    }
}