using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/Vector3 Result",
        fileName = "event_no_param_vector3_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamVector3Result : EventNoParamResult<Vector3>
    {
    }
}