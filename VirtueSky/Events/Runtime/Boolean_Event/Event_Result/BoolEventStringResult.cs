using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/String Result",
        fileName = "bool_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventStringResult : BaseEvent<bool, string>
    {
    }
}