using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/Boolean Result",
        fileName = "event_no_param_bool_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamBoolResult : EventNoParamResult<bool>
    {
    }
}