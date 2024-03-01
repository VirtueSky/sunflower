using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/String Result",
        fileName = "event_no_param_string_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamStringResult : EventNoParamResult<string>
    {
    }
}