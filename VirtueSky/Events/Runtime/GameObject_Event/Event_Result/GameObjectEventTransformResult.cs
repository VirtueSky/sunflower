using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/Transform Result",
        fileName = "gameobject_event_transform_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventTransformResult : BaseEvent<GameObject, Transform>
    {
    }
}