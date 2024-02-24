using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/Int Result",
        fileName = "string_event_int_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventIntResult : BaseEvent<string, int>
    {
    }
}