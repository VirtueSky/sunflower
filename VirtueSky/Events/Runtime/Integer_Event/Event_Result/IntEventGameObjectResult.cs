using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Int Event/GameObject Result",
        fileName = "int_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class IntEventGameObjectResult : BaseEvent<int, GameObject>
    {
    }
}