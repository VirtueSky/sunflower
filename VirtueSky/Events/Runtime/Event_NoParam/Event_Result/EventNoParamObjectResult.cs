using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/Object Result",
        fileName = "event_no_param_object_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamObjectResult : EventNoParamResult<object>
    {
    }
}