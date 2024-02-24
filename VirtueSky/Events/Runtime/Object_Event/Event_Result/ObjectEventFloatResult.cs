using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/Float Result",
        fileName = "object_event_float_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventFloatResult : BaseEvent<object, float>
    {
    }
}