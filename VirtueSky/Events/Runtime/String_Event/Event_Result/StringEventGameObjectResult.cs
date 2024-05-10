using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/String Event/GameObject Result",
        fileName = "string_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class StringEventGameObjectResult : BaseEvent<string, GameObject>
    {
    }
}