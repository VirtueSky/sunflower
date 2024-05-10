using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/GameObject Result",
        fileName = "gameobject_event_gameobject_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventGameObjectResult : BaseEvent<GameObject, GameObject>
    {
    }
}