using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Events
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Event-Result/GameObject Event/Bool Result",
        fileName = "gameobject_event_bool_result")]
    [EditorIcon("scriptable_event")]
    public class GameObjectEventBoolResult : BaseEvent<GameObject, bool>
    {
    }
}