using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/Int Result",
        fileName = "event_no_param_int_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamIntResult : EventNoParamResult<int>
    {
    }
}