using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/Boolean Result",
        fileName = "bool_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventBoolResult : BaseEvent<bool, bool>
    {
    }
}