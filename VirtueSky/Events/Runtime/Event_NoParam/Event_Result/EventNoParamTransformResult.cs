using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/Transform Result",
        fileName = "event_no_param_transform_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamTransformResult : EventNoParamResult<Transform>
    {
    }
}