using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/Transform Event/GameObject Result",
        fileName = "transform_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class TransformEventGameObjectResult : BaseEvent<Transform, GameObject>
    {
    }
}