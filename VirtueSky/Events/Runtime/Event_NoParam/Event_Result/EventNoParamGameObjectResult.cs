using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Event No Param/GameObject Result",
        fileName = "event_no_param_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class EventNoParamGameObjectResult : EventNoParamResult<GameObject>
    {
    }
}