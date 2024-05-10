using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Object Event/GameObject Result",
        fileName = "object_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class ObjectEventGameObjectResult : BaseEvent<object, GameObject>
    {
    }
}