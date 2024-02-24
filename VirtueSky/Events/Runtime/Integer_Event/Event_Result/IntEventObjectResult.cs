using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/Object Result",
        fileName = "int_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventObjectResult : BaseEvent<int, object>
    {
    }
}