using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/Object Result",
        fileName = "string_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventObjectResult : BaseEvent<string, object>
    {
    }
}