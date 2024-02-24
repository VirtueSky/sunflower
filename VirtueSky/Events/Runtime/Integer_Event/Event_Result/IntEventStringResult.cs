using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/String Result",
        fileName = "int_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventStringResult : BaseEvent<int, string>
    {
    }
}