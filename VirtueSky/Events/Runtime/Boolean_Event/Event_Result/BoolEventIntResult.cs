using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/Int Result",
        fileName = "bool_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventIntResult : BaseEvent<bool, int>
    {
    }
}