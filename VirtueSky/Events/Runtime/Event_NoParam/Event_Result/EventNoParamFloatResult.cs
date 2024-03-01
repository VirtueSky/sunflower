using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/Float Result",
        fileName = "event_no_param_float_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamFloatResult : EventNoParamResult<float>
    {
    }
}