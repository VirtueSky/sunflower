using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Boolean Event/GameObject Result",
        fileName = "bool_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class BoolEventGameObjectResult : BaseEvent<bool, GameObject>
    {
    }
}