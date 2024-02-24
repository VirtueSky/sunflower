using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/String Result",
        fileName = "string_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventStringResult : BaseEvent<string, string>
    {
    }
}