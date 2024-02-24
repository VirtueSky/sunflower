using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/Bool Result",
        fileName = "string_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventBoolResult : BaseEvent<string, bool>
    {
    }
}