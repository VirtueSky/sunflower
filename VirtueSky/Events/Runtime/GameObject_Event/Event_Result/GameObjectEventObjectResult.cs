using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/Object Result",
        fileName = "gameobject_event_object_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventObjectResult : BaseEvent<GameObject, object>
    {
    }
}