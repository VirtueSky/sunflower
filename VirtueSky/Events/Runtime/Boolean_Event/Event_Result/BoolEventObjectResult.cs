using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/Object Result",
        fileName = "bool_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventObjectResult : BaseEvent<bool, object>
    {
    }
}