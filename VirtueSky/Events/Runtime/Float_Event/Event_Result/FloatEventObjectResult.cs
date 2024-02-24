using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Float Event/Object Result",
        fileName = "float_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class FloatEventObjectResult : BaseEvent<float, object>
    {
    }
}