using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/Bool Result",
        fileName = "int_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventBoolResult : BaseEvent<int, bool>
    {
    }
}