using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/String Result",
        fileName = "object_event_string_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventStringResult : BaseEvent<object, string>
    {
    }
}